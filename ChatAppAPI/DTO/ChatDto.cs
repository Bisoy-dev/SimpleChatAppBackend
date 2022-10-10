using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatAppAPI.DTO
{
    public class ChatDto
    {
        public int ChatId { get; set; }
        public string UserId { get; set; }
        public string ChatUserId { get; set; }
        public string ChatUserNickName { get; set; }
    }
}
