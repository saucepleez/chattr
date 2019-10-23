using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace chattr.Models
{
    public class ChatContext
    {
        public BotConfig BotConfiguration { get; set; }
        public Guid CurrentConversationID { get; set; }
        public List<SessionMessage> BotResponses { get; set; } = new List<SessionMessage>();

        public ChatContext(BotConfig config, List<SessionMessage> sessionMessages)
        {
            BotConfiguration = config;
            BotResponses = sessionMessages;
        }
        public void AddBotMessage(string message)
        {
            BotResponses.Add(new SessionMessage() { MessageContent = message, Type = SessionMessage.MessageType.BotMessage });
        }
   
    }
}
