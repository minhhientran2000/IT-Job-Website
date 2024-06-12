using Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationController : ControllerBase
    {
        private readonly MyDbContext _context;
        public ApplicationController(MyDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Application>>> GetApplication()
        {
            /*return await _context.jobLists.ToListAsync();*/
            return await _context.applications.OrderByDescending(item => item.ApplicationId).Include(j => j.Status).Include(j=>j.User).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Application>> GetApplication(int id)
        {
            var comp = await _context.applications.OrderByDescending(item => item.ApplicationId).Include(j => j.Status).FirstOrDefaultAsync(j => j.ApplicationId == id);

            if (comp == null)
            {
                return NotFound("Khong ton tai application");
            }
            return comp;
        }

        [HttpGet("GetByUserId/{id}")]
        public async Task<ActionResult<Application>> GetByUserId(int id, int id2)
        {
            var comp = await _context.applications.OrderByDescending(item => item.ApplicationId).Include(j => j.Status).Where(x => x.JobId == id && x.UserId == id2).FirstOrDefaultAsync();

            if (comp == null)
            {
                return NotFound("Khong ton tai application");
            }
            return comp;
        }


        [HttpGet("GetByUser/{id}")]
        public async Task<ActionResult<IEnumerable<Application>>> GetByUser(int id)
        {
            var comp = await _context.applications.OrderByDescending(item => item.ApplicationId).Include(j => j.Status).Include(j => j.Job).Where(j => j.UserId == id).ToListAsync();

            if (comp.Count() == 0)
            {
                return NotFound("Khong ton tai application");
            }
            return comp;
        }


        [HttpGet("GetByJobId/{id}")]
        public async Task<ActionResult<IEnumerable<Application>>> GetByJobId(int id)
        {
            var comp = await _context.applications.OrderByDescending(item => item.ApplicationId).Include(j => j.Status).Include(j => j.Job).Where(j => j.JobId == id).ToListAsync();

            if (comp.Count() == 0)
            {
                return NotFound("Khong ton tai application");
            }
            return comp;
        }


        [HttpGet("GetSeekersForJob/{jobId}")]
        public async Task<ActionResult<IEnumerable<Seeker>>> GetSeekersForJob(int jobId, int statusId)
        {

            var seekers = await (from seeker in _context.seekers
                                 join user in _context.users on seeker.UserId equals user.UserId
                                 join application in _context.applications on user.UserId equals application.UserId
                                 join job in _context.jobLists on application.JobId equals job.JobId
                                 where job.JobId == jobId && application.StatusId == statusId
                                 select seeker) // Select only Seeker
                     .ToListAsync();
            if (seekers == null || !seekers.Any())
            {
                return NotFound("Khong co");
            }
            return seekers;    
        }


        [HttpPost]
        public async Task<ActionResult<Application>> CreateJob(ApplicationItem item)
        {
            Application app = new Application();
            
            app.CreateDate = DateTime.Now;
            app.JobId = item.JobId;
            app.UserId = item.UserId;
            app.StatusId = item.StatusId;
            app.IntroducingLetter = item.IntroducingLetter;
           
            /*job.LevelId = item.LevelId;
            job.TypeOfWork = item.TypeOfWork;*/
            _context.applications.Add(app);
            await _context.SaveChangesAsync();
            return Ok(app);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateApplication(int id, int id2, int statusId)
        {
            var comp = await _context.applications.Where(x => x.JobId == id && x.UserId == id2).FirstOrDefaultAsync();
            //var comp = await _context.applications.FindAsync(id);
            if (comp != null)
            {
                comp.StatusId = statusId;
                await _context.SaveChangesAsync();
                return Ok("Updated");
            }
            return NotFound();
        }


    }
}
