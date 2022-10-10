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
    public class UserChatController : ControllerBase
    {
        private readonly IUserChatService _userChatService;
        private readonly INotificationNumberService _notificationService;
        public UserChatController(IUserChatService userChatService, INotificationNumberService notificationService)
        {
            _userChatService = userChatService;
            _notificationService = notificationService;
        } 

        [HttpPost]
        [Route("/api/chat/send-message")]
        [Authorize(Roles = "User")] 
        public async Task<IActionResult> SendMessage([FromBody] SendMessageDto model)
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            await  _userChatService.SendMessage(model,userId);
            return Ok();
        } 
        [HttpGet]
        [Route("/api/chat/get-chats")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetAllChat()
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var chats = await _userChatService.GetAllChat(userId);
            var jsonChatObjects = JsonConvert.SerializeObject(chats, new JsonSerializerSettings
            { 
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
            return Ok(jsonChatObjects);
        } 

        [HttpGet]
        [Route("/api/chat/get-conversation/{chatId}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetConversation(int chatId)
        {
            var conversation = await _userChatService.GetConversation(chatId);
            var jsonConversationObject = JsonConvert.SerializeObject(conversation, new JsonSerializerSettings 
            {
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
            return Ok(jsonConversationObject);
        } 
        [HttpGet]
        [Route("/api/chat/get-mesnotif-number")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetMessageNotificationNumber()
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var messageNotifNumber = await _notificationService.GetMessageNotificationNumber(userId);
            return Ok(messageNotifNumber);
        }
    }
}
