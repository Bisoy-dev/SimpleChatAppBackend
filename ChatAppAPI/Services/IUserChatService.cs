using ChatAppAPI.DTO;
using ChatAppAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatAppAPI.Services
{
    public interface IUserChatService
    {
        Task SendMessage(SendMessageDto model, string userId);
        Task<List<ChatDto>> GetAllChat(string userId);
        Task<List<Message>> GetConversation(int chatId);
    }
}
