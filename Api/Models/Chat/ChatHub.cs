using Microsoft.AspNetCore.SignalR;
using System.Text.RegularExpressions;

namespace Api.Models.Chat
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(int groupId, string content, string dataType, int userType, string fileName, string fileLink)
        {
            string formattedDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            var newMessage = new Message
            {
                GroupId = groupId,
                Content = content,
                DataType = dataType,
                Time = formattedDate,
                UserType = userType,
                FileName = fileName,
                FileLink = fileLink
            };

            /* await _messageRepository.AddMessageAsync(newMessage);
 */
            await Clients.Group(GetChatGroup(groupId)).SendAsync("ReceiveMessage", newMessage);
        }

        public async Task JoinChatGroup(int groupId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, GetChatGroup(groupId));
        }

        public async Task LeaveChatGroup(int groupId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, GetChatGroup(groupId));
        }

        private string GetChatGroup(int groupId)
        {
            return $"chat-{groupId}";
        }

        public async Task AddGroupChat(int groupId, int seekerId, int companyId, string groupName)
        {
            var newGroup = new GroupChat
            {
                GroupId = groupId,
                SeekerId = seekerId,
                CompanyId = companyId,
                GroupName = groupName
            };

            // Broadcast group creation notification to all connected clients (optional)
            await Clients.All.SendAsync("ReceiveGroup", newGroup);
        }

       /* public async Task NotifyDatabaseUpdated()
        {
            // Gửi thông báo đến tất cả client rằng dữ liệu đã được cập nhật
            await Clients.All.SendAsync("DatabaseUpdated");
        }*/
    }
}
