using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatAppAPI.Models
{
    public class MessageNotificationNumber
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int NumberOfMessage { get; set; }
    }
}
