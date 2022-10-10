using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatAppAPI.Models
{
    public class Message
    {
        public int Id { get; set; }
        public int ChatId { get; set; }
        public string UserId { get; set; }
        public string UserNickName { get; set; }
        public string Text { get; set; }
        public DateTime SendDate { get; set; }
    }
}
