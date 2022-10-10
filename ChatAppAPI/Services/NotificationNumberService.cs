using ChatAppAPI.Context;
using ChatAppAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatAppAPI.Services
{
    public class NotificationNumberService : INotificationNumberService
    {
        private readonly ApplicationDbContext _context;
        public NotificationNumberService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddMessageNotification(string userId)
        {
            var messageNotification = new MessageNotificationNumber()
            {
                UserId = userId,
                NumberOfMessage = 0
            };
            _context.MessageNotifications.Add(messageNotification);
            await _context.SaveChangesAsync();
        }

        public async Task AddRequestNotification(string userId)
        {
            var requestNotification = new RequestNotificationNumber()
            {
                UserId = userId,
                NumberOfRequest = 0
            };

            _context.RequestNotifications.Add(requestNotification);
            await _context.SaveChangesAsync();
        }

        public async Task<MessageNotificationNumber> GetMessageNotificationNumber(string userId)
        {
            var numberOfMessageNotif = await _context.MessageNotifications.Where(n => n.UserId == userId)
                .FirstOrDefaultAsync();
            return numberOfMessageNotif;
        }

        public async Task<RequestNotificationNumber> GetRequestNotificationNumber(string userId)
        {
            var numberOfReqNotif = await _context.RequestNotifications.Where(n => n.UserId == userId)
                .FirstOrDefaultAsync();
            return numberOfReqNotif;
        }
    }
}
