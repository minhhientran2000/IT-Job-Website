using Api.Models;
using Api.Models.Chat;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;
using System.Net;
using System.Text.RegularExpressions;


namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        public class MessageViewModel
        {
            public int MessageId { get; set; }
            public string? Content { get; set; }
            public int? GroupId { get; set; }
            public string? Time { get; set; }
            public string? DataType { get; set; }
            public int? UserType { get; set; }
            public string? FileName { get; set; }
            public string? FileLink { get; set; }
        }

        public class GroupViewModel
        {
            public int GroupId { get; set; }
            public int SeekerId { get; set; }
            public int CompanyId { get; set; }
            public string GroupName { get; set; }
        }

        private readonly MyDbContext _context;

        private readonly IHubContext<ChatHub> _hubContext;
        private readonly IWebHostEnvironment _environment;
        public MessageController(IHubContext<ChatHub> hubContext, IWebHostEnvironment environment, MyDbContext context)
        {
            _hubContext = hubContext;
            _environment = environment;
            _context = context;
        }
       
        

        [HttpGet("GetGroupsBySeeker")]
        public ActionResult<IEnumerable<GroupViewModel>> GetGroupsBySeeker(int seekerId)
        {
            var groups = (from groupChat in _context.groupChats
                          join seeker in _context.seekers on groupChat.SeekerId equals seeker.SeekerId
                          where seeker.SeekerId == seekerId
                          group groupChat by new { groupChat.GroupId, groupChat.GroupName } into g
                          select new GroupViewModel
                          {
                              GroupId = g.Key.GroupId,
                              GroupName = g.Key.GroupName
                              // Thêm các thuộc tính khác nếu cần
                          }).ToList();
            if (!groups.Any())
            {
                return NotFound("Khong ton tai group"); // Trả về HTTP 404 Not Found nếu danh sách groups rỗng
            }
            return groups;
        }

        [HttpGet("GetGroupsByGroupId")]
        public async Task<ActionResult<GroupChat>> GetGroupsByGroupId(int id)
        {
            var group = await _context.groupChats.FirstOrDefaultAsync(j => j.GroupId == id);

            if (group == null)
            {
                return NotFound("Khong ton tai group");
            }
            return group;
        }

        [HttpGet("GetGroupsByCompany")]
        public ActionResult<IEnumerable<GroupViewModel>> GetGroupsByCompany(int companyId)
        {
            var groups = (from groupChat in _context.groupChats
                          join company in _context.companies on groupChat.CompanyId equals company.CompanyId
                          where company.CompanyId == companyId
                          group groupChat by new { groupChat.GroupId, groupChat.GroupName } into g
                          select new GroupViewModel
                          {
                              GroupId = g.Key.GroupId,
                              GroupName = g.Key.GroupName
                              // Thêm các thuộc tính khác nếu cần
                          }).ToList();

            if (!groups.Any())
            {
                return NotFound("Khong ton tai group"); // Trả về HTTP 404 Not Found nếu danh sách groups rỗng
            }
            
            return groups;
        }


        [HttpGet("GetChatHistoryBySeeker")]
        public async Task<ActionResult<List<MessageViewModel>>> GetChatHistoryBySeeker(int groupId, int seekerId)
        {
            var messages = (from message in _context.messages
                            join groupChat in _context.groupChats on message.GroupId equals groupChat.GroupId
                            join seeker in _context.seekers on groupChat.SeekerId equals seeker.SeekerId
                            where message.GroupId == groupId && seeker.SeekerId == seekerId
                            orderby message.Time
                            select new MessageViewModel
                            {
                                MessageId = message.MessageId,
                                Content = message.Content,
                                Time = message.Time,
                                GroupId = message.GroupId,
                                DataType = message.DataType,
                                UserType = message.UserType,
                                FileLink = message.FileLink,
                                FileName = message.FileName
                            }).ToList();

            return messages;
        }


        [HttpGet("GetChatHistoryByCompany")]
        public async Task<ActionResult<List<MessageViewModel>>> GetChatHistoryByCompany(int groupId, int companyId)
        {
            var messages = (from message in _context.messages
                            join groupChat in _context.groupChats on message.GroupId equals groupChat.GroupId
                            join company in _context.companies on groupChat.CompanyId equals company.CompanyId
                            where message.GroupId == groupId && company.CompanyId == companyId
                            orderby message.Time
                            select new MessageViewModel
                            {
                                MessageId = message.MessageId,
                                Content = message.Content,
                                Time = message.Time,
                                GroupId = message.GroupId,
                                DataType = message.DataType,
                                UserType = message.UserType,
                                FileLink = message.FileLink,
                                FileName = message.FileName
                            }).ToList();

            return messages;
        }

        [HttpPost]
        public async Task<ActionResult<Message>> SendMessage(string? Content, int groupId, string dataType, int userType, string? fileName, string? fileLink)
        {
            string formattedDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            var message = new Message
            {
                Content = Content,
                GroupId = groupId,
                Time = formattedDate,
                DataType = dataType,
                UserType = userType,
                FileLink = fileLink,
                FileName = fileName
            };
            _context.messages.Add(message);
            await _context.SaveChangesAsync();

            await _hubContext.Clients.Group($"chat-{groupId}").SendAsync("ReceiveMessage", message);
            return Ok(message);
        }

        [HttpPost("AddGroupChat")]
        public async Task<ActionResult<GroupChat>> AddGroupChat(int seekerId, int companyId, string groupName)
        {
            try
            {
                // Kiểm tra xem đã tồn tại group chat với seekerId và companyId đã cung cấp không
                var existingGroupChat = _context.groupChats.FirstOrDefault(g =>
                    g.SeekerId == seekerId && g.CompanyId == companyId);

                if (existingGroupChat != null)
                {
                    return Conflict("Đã tồn tại");
                }
                var groupChat = new GroupChat
                {
                    SeekerId = seekerId,
                    CompanyId = companyId,
                    GroupName = groupName,
                };

                _context.groupChats.Add(groupChat);
                await _context.SaveChangesAsync();
                //await _hubContext.Clients.All.SendAsync("ReceiveGroup", groupChat);
                //await _hubContext.Clients.All.SendAsync("ReceiveGroup", groupChat.GroupId, "CreateGroupChat", seekerId, companyId);
                return Ok(groupChat);
            }
            catch (Exception ex)
            {
                return BadRequest("Không tồn tại seeker hoặc company");
            }
        }

        /*[HttpPost("DownloadFile")]
        public async Task<IActionResult> DownloadFile(IFormFile file)
        {
            var uploadsRootFolder = Path.Combine(_environment.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsRootFolder))
            {
                Directory.CreateDirectory(uploadsRootFolder);
            }

            // Ensure the filename is unique to avoid overwriting existing files
            var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(file.FileName);
            var filePath = Path.Combine(uploadsRootFolder, uniqueFileName);

            // Save the file
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            // Return the path to the uploaded file
            var uploadedFilePath = Path.Combine("/uploads", uniqueFileName);

            // Return the file
            var memory = new MemoryStream();
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;

            // Set content type based on file extension
            var contentType = "application/octet-stream"; // default content type
            var fileExt = Path.GetExtension(filePath).ToLowerInvariant();
            if (fileExt == ".pdf")
            {
                contentType = "application/pdf";
            }
            else if (fileExt == ".docx")
            {
                contentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
            }
            // Add other content types as needed

            // Delete the file after download
            System.IO.File.Delete(filePath);

            return File(memory, contentType, Path.GetFileName(filePath));
        }*/

        [HttpPost("UploadFile")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            var uploadsRootFolder = Path.Combine(_environment.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsRootFolder))
            {
                Directory.CreateDirectory(uploadsRootFolder);
            }

            // Ensure the filename is unique to avoid overwriting existing files
            var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(file.FileName);
            var filePath = Path.Combine(uploadsRootFolder, uniqueFileName);

            // Save the file
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            // Return the URL to the uploaded file
            var uploadedFilePath = Path.Combine("/uploads", uniqueFileName);
            var downloadUrl = Url.Action("Download", "File", new { fileName = uniqueFileName });

            return Ok(new { downloadUrl });
        }

        [HttpGet("Download/{fileName}")]
        public IActionResult Download(string fileName)
        {
            var uploadsRootFolder = Path.Combine(_environment.WebRootPath, "uploads");
            var filePath = Path.Combine(uploadsRootFolder, fileName);

            // Check if the file exists
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            // Set content type based on file extension
            var contentType = "application/octet-stream"; // default content type
            var fileExt = Path.GetExtension(filePath).ToLowerInvariant();
            if (fileExt == ".pdf")
            {
                contentType = "application/pdf";
            }
            else if (fileExt == ".docx")
            {
                contentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
            }
            // Add other content types as needed

            // Return the file
            var fileStream = new FileStream(filePath, FileMode.Open);
            return File(fileStream, contentType, Path.GetFileName(filePath));
        }


        /*[HttpGet("SearchBySeeker")]
        public ActionResult<IEnumerable<GroupViewModel>> SearchBySeeker(string keyword, int seekerId)
        {
            // Tìm kiếm các công việc dựa trên từ khóa
            var groups = (from groupChat in _context.groupChats
                          join seeker in _context.seekers on groupChat.SeekerId equals seeker.SeekerId
                          where seeker.SeekerId == seekerId
                          group groupChat by new { groupChat.GroupId, groupChat.GroupName } into g
                          select new GroupViewModel
                          {
                              GroupId = g.Key.GroupId,
                              GroupName = g.Key.GroupName
                              // Thêm các thuộc tính khác nếu cần
                          }).ToList();
            var res = groups.Where(j => j.GroupName.Contains(keyword)).ToList();
            if (res.Count == 0)
            {
                return NotFound("Khong co");
            }
            return res;
        }*/
    }

}
