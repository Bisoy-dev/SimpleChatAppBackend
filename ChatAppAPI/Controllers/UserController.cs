using ChatAppAPI.DTO;
using ChatAppAPI.Models;
using ChatAppAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ChatAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly INotificationNumberService _notificationService;

        public UserController(UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, INotificationNumberService notificationService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _notificationService = notificationService;
        } 
        [HttpPost] 
        [Route("/api/user/add-role")]
        public async Task<IActionResult> AddRole()
        {
            var role = new IdentityRole()
            {
                Name = "User"
            }; 
            if( await _roleManager.RoleExistsAsync(role.Name))
            {
                return BadRequest(new { Meesage = $"Role {role.Name} is already exist."});
            }
            var result = await _roleManager.CreateAsync(role);
            IActionResult actionResult = result.Succeeded ? Ok(new { Message = $"Role {role.Name} is created successfully." }) : BadRequest();
            return actionResult;
        } 
        [HttpPost]
        [Route("/api/user/register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto model)
        {
            string role = "User";
            var isUserExist = await _userManager.FindByNameAsync(model.UserName);
            if(isUserExist != null)
            {
                return BadRequest(new { Message = "Username is already exist"});
            }

            var user = new ApplicationUser()
            {
                UserName = model.UserName,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserNickName = model.UserNickName,
                JoinedDate = DateTime.UtcNow
            };

            if (!await _roleManager.RoleExistsAsync(role))
                return BadRequest();

            var result = await _userManager.CreateAsync(user, model.ConfirmPassword);
            if (result.Succeeded)
            {
                var appUser = await _userManager.FindByNameAsync(model.UserName);
                await _userManager.AddToRoleAsync(appUser, role);
                await _notificationService.AddRequestNotification(appUser.Id);
                await _notificationService.AddMessageNotification(appUser.Id);
                return Ok(new { Message = "Registered Successfully."});
            }
            return BadRequest(new { Message = "Something went wrong."});
        }
        [HttpPost]
        [Route("/api/user/login")]
        public async Task<IActionResult> Login([FromBody] LogInUserDto model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password,false,false);
            if (result.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(model.UserName);
                var role = await _userManager.GetRolesAsync(user);
                string fullName = $"{user.FirstName} {user.LastName}";
                var token = GenerateToken(user.Id, user.UserName, user.Email, fullName, role);
                return Ok(new { Message = "Login Successfully", Token = token});
            }
            return BadRequest(new { Message = "Username or password invalid."});
        }  
        [HttpGet]
        [Route("/api/user/dashboard")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> DashBoard()
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);
            var userObject = new { UserId = user.Id, NickName = user.UserNickName, Email = user.Email};
            var jsonUserObject = JsonConvert.SerializeObject(userObject, new JsonSerializerSettings 
            {
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
            return Ok(jsonUserObject);
        }
        [HttpGet]
        [Route("/api/user/get-users")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetAllUser()
        {
            var users = await _userManager.Users.ToListAsync();
            var jsonUserObject = JsonConvert.SerializeObject(users, new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });

            return Ok(jsonUserObject);
        }
        private string GenerateToken(string userId, string userName, string email, string fullName, IList<string> role)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("MY_SECRETKEY_OF_THIS_APPLICATION_NAME_CHATAPP@2021_AND_FOREVER");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userId),
                    new Claim(ClaimTypes.Name, userName),
                    new Claim(ClaimTypes.Email, email),
                    new Claim("FullName", fullName),
                    new Claim(ClaimTypes.Role, role.FirstOrDefault())
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var stringToken = tokenHandler.WriteToken(token);
            return stringToken;
        }
    }
}
