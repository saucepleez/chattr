using chattr.Models.Tasks;
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
        public Dictionary<string, string> ContextVariables { get; set; } = new Dictionary<string, string>();
        public List<SessionMessage> BotResponses { get; set; } = new List<SessionMessage>();
        public ConversationResult.State CurrentState { get; set; }
        public Conversation CurrentConversation { get; set; }
        public Guid CurrentConversationID { get; set; }
        public Actions.Action CurrentAction { get; set; }
        public ChatContext(BotConfig config, List<SessionMessage> sessionMessages)
        {
            BotConfiguration = config;
            BotResponses = sessionMessages;
        }
        public void AddBotMessage(string message)
        {
            //variablie check
            var processedMessage = ConvertVariables(message);
            BotResponses.Add(new SessionMessage() { MessageContent = processedMessage, Type = SessionMessage.MessageType.BotMessage });
        }

        public string ConvertVariables(string input)
        {

            var variablePatterns = input.Split('{', '}');

            foreach (var pattern in variablePatterns.Where(f => !string.IsNullOrEmpty(f)))
            {
                //check for variable or if name matches action

                if (ContextVariables.ContainsKey(pattern))
                {
                    var variableLookup = ContextVariables[pattern];
                    input = input.Replace("{" + pattern + "}", variableLookup);
                }
                else
                {
                    var action = CurrentConversation.Actions.Where(f => f.Name != null && f.Name.ToLower() == pattern.ToLower()).FirstOrDefault();

                    if (action != null)
                    {
                        input = input.Replace("{" + pattern + "}", action.GetDefaultValue());
                    }
                }

            }

            return input;
        }
   
    }
}
