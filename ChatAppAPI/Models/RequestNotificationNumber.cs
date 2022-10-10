using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatAppAPI.Models
{
    public class RequestNotificationNumber
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int NumberOfRequest { get; set; }
    }
}
