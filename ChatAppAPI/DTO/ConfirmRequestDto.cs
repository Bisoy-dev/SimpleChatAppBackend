using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatAppAPI.DTO
{
    public class ConfirmRequestDto
    {
        public int RequestId { get; set; }
        public string RequestUserId { get; set; }
        public bool IsConfirmed { get; set; }
    }
}
