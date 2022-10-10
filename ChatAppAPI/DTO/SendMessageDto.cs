using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatAppAPI.DTO
{
    public class SendMessageDto
    {
        public int ChatId { get; set; }
        public string Text { get; set; }
    }
}
