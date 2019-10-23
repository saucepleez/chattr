using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace chattr.Models
{
    public class SessionMessage
    {
        public DateTime DateStamp { get; set; }
        public string MessageContent { get; set; }
        public MessageType Type { get; set; }
        public enum MessageType
        {
            UserMessage, BotMessage
        }

        public SessionMessage()
        {
            DateStamp = DateTime.Now;
        }
    }

}
