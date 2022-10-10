using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatAppAPI.Models
{
    public class Request
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string RequestUserId { get; set; }
        public string RequestUserName { get; set; }
        public bool IsConfirmed { get; set; }
    }
}
