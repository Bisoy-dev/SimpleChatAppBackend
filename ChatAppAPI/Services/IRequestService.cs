using ChatAppAPI.DTO;
using ChatAppAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatAppAPI.Services
{
    public interface IRequestService
    {
        Task SendRequest(SendRequestDto model, string requestUserId);
        Task DeleteRequest(int requestId);
        Task ConfirmRequest(bool isConfirm, ConfirmRequestDto model, string userId);
        Task<List<Request>> GetAllRequest(string userId);
        Task UpdateRequest(int requestId);
    }
}
