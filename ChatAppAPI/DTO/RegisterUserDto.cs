using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ChatAppAPI.DTO
{
    public class RegisterUserDto
    {
        [Required(ErrorMessage = "Firstname is reuired")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Lastname is reuired")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Username is reuired")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Email is reuired")]
        public string Email { get; set; }
        [Required(ErrorMessage = "User nickname is reuired")]
        public string UserNickName { get; set; }
        [Required(ErrorMessage = "Password is reuired")]
        public string Password { get; set; } 
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}
