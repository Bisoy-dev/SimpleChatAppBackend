using ChatAppAPI.Context;
using ChatAppAPI.DTO;
using ChatAppAPI.Hubs;
using ChatAppAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatAppAPI.Services
{
    public class UserChatService : IUserChatService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHubContext<ChatHub> _hubContext;

        public UserChatService(ApplicationDbContext context, UserManager<ApplicationUser> userManager, 
            IHubContext<ChatHub> hubContext)
        {
            _context = context;
            _userManager = userManager;
            _hubContext = hubContext;
        }

        public async Task<List<ChatDto>> GetAllChat(string userId)
        {
            var chatsDto = new List<ChatDto>();
            var chats = await _context.UserChats.Where(chat => chat.UserId == userId).ToListAsync(); 
            
            foreach(var chat in chats)
            {
                var chatUser = await _context.UserChats.Where(c => c.ChatId == chat.ChatId && c.UserId != chat.UserId).FirstOrDefaultAsync();
                if(chatUser != null)
                {
                    var chatUserInfo = await _userManager.FindByIdAsync(chatUser.UserId);
                    chatsDto.Add(new ChatDto { ChatId = chat.ChatId, UserId = chat.UserId, ChatUserId = chatUserInfo.Id, ChatUserNickName = chatUserInfo.UserNickName });
                }
            }
            var messageNotification = await _context.MessageNotifications.Where(n => n.UserId == userId)
                .FirstOrDefaultAsync();
            messageNotification.NumberOfMessage = 0;
            await _context.SaveChangesAsync();
            return chatsDto;
        }

        public async Task<List<Message>> GetConversation(int chatId)
        {
            var messages = await _context.Messages.Where(m => m.ChatId == chatId).ToListAsync();
            return messages;
        }

        public async Task SendMessage(SendMessageDto model, string userId)
        {
            var userChat = await _context.UserChats.Where(user => user.UserId == userId && user.ChatId == model.ChatId)
                .FirstOrDefaultAsync(); 

            if(userChat != null)
            {
                var userInfo = await _userManager.FindByIdAsync(userId);
                //Get user to notify when this user send  a message through to get all user on that specific chat
                var users = await _context.UserChats.Where(user => user.ChatId == model.ChatId).ToListAsync();
                var user = users.Where(user => user.UserId != userId).FirstOrDefault();

                var message = new Message()
                {
                    ChatId = model.ChatId,
                    UserId = userInfo.Id,
                    UserNickName = userInfo.UserNickName,
                    Text = model.Text,
                    SendDate = DateTime.UtcNow
                };

                //And send message notification this user.
                var messageNotification = await _context.MessageNotifications.Where(notification => notification.UserId == user.UserId)
                    .FirstOrDefaultAsync(); 

                messageNotification.NumberOfMessage += 1;
                _context.Messages.Add(message);
                await _context.SaveChangesAsync();

                //add realtime functionality with signalR or hub (temporary use Clients.All.SendAsync)
                await _hubContext.Clients.All.SendAsync("RecieveMessage", message);
                await _hubContext.Clients.All.SendAsync("RecieveMessageNotificationNumber", messageNotification);
                //await _hubContext.Clients.User(userId).SendAsync("RecieveMessage", message);
                //await _hubContext.Clients.User(userId).SendAsync("RecieveMessageNotificationNumber", messageNotification);
            }
            else
            {
                throw new Exception("Something went wrong.");
            }
        }
    }
}
