using ChatAppAPI.Context;
using ChatAppAPI.DTO;
using ChatAppAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatAppAPI.Services
{
    public class RequestService : IRequestService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public RequestService(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task ConfirmRequest(bool isConfirm, ConfirmRequestDto model, string userId)
        {
            var request = await _context.Requests.Where(r => r.Id == model.RequestId).FirstOrDefaultAsync(); 
            if(request != null)
            {
                if (isConfirm == true)
                {
                    var chat = new Chat();
                    _context.Chats.Add(chat);
                    await _context.SaveChangesAsync();
              
                    var userChat = new UserChat()
                    {
                        ChatId = chat.Id,
                        UserId = userId
                    };
                    var userRequestChat = new UserChat()
                    {
                        ChatId = chat.Id,
                        UserId = model.RequestUserId
                    };

                    _context.UserChats.Add(userChat);
                    _context.UserChats.Add(userRequestChat);
                    await UpdateRequest(model.RequestId);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    await DeleteRequest(request.Id);
                    await _context.SaveChangesAsync();
                }
            }
            else
            {
                throw new Exception("Request is not exist.");
            }
        }

        public async Task DeleteRequest(int requestId)
        {
            var request = await _context.Requests.Where(r => r.Id == requestId).FirstOrDefaultAsync();
            if(request != null)
            {
                _context.Remove(request);
            }
            else
            {
                throw new Exception("Failed to delete request.");
            }
            
        }

        public async Task<List<Request>> GetAllRequest(string userId)
        {
            var userRequests = await _context.Requests.Where(request => request.UserId == userId && request.IsConfirmed == false).ToListAsync();
            var requestNotification = await _context.RequestNotifications.Where(notification => notification.UserId == userId)
                .FirstOrDefaultAsync();
            requestNotification.NumberOfRequest = 0;
            await _context.SaveChangesAsync();
            return userRequests;
        }

        public async Task SendRequest(SendRequestDto model, string requestUserId)
        {
            var userRequest = await _userManager.FindByIdAsync(requestUserId);
            var isRequestExist = await _context.Requests.Where(r => r.UserId == model.UserId && r.RequestUserId == requestUserId)
                .FirstOrDefaultAsync();
            if(isRequestExist == null)
            {
                if (userRequest != null)
                {
                    var request = new Request()
                    {
                        UserId = model.UserId,
                        RequestUserId = requestUserId,
                        RequestUserName = userRequest.UserName,
                        IsConfirmed = false
                    };
                    var requestNotification = await _context.RequestNotifications.Where(notification => notification.UserId == model.UserId)
                        .FirstOrDefaultAsync();
                    requestNotification.NumberOfRequest += 1;
                    _context.Requests.Add(request);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    throw new Exception("User that send request is not exist");
                }
            }
            else
            {
                throw new Exception($"You {isRequestExist.RequestUserId} is already sent a request from {isRequestExist.UserId}");
            }
           
        }

        public async Task UpdateRequest(int requestId)
        {
            var request = await _context.Requests.Where(r => r.Id == requestId).FirstOrDefaultAsync();
            request.IsConfirmed = true;
        }
    }
}
