using ChatAppAPI.DTO;
using ChatAppAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ChatAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestController : ControllerBase
    {
        private readonly IRequestService _requestService;
        private readonly INotificationNumberService _notificationService;
        public RequestController(IRequestService requestService, INotificationNumberService notificationService)
        {
            _requestService = requestService;
            _notificationService = notificationService;
        } 

        [HttpPost]
        [Route("/api/request/send-req")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> SendRequest(SendRequestDto model)
        {
            var requestUserId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _requestService.SendRequest(model,requestUserId);
            return Ok();
        }
        [HttpPost]
        [Route("/api/request/confirm")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> ConfirmRequest(ConfirmRequestDto model)
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _requestService.ConfirmRequest(model.IsConfirmed,model, userId);
            return Ok();
        }
        [HttpGet]
        [Route("/api/request/get-requests")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetAllRequest()
        {
            string userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var requests = await _requestService.GetAllRequest(userId);
            var jsonRequestObject = JsonConvert.SerializeObject(requests, new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
            return Ok(jsonRequestObject);
        } 
        [HttpGet]
        [Route("/api/request/get-reqnotif-number")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetNumberRequestNotification()
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var numberOfReqNotif = await _notificationService.GetRequestNotificationNumber(userId);
            return Ok(numberOfReqNotif);
        }
    }
}
