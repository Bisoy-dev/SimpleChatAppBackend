using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatAppAPI.Models
{
    public class UserChat
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int ChatId { get; set; }
    }
}
