using ChatAppAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatAppAPI.Services
{
    public interface INotificationNumberService
    {
        Task AddRequestNotification(string userId);
        Task AddMessageNotification(string userId);
        Task<RequestNotificationNumber> GetRequestNotificationNumber(string userId);
        Task<MessageNotificationNumber> GetMessageNotificationNumber(string userId);
    }
}
