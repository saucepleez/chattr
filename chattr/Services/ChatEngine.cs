using chattr.Models;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace chattr.Services
{
    public class ChatEngine
    {
        Dictionary<Guid, Models.ChatContext> Sessions = new Dictionary<Guid, Models.ChatContext>();
        public void ProcessChat(BotConfig config, Guid sessionID, string userStatement, List<SessionMessage> sessionMessages)
        {
            //create context
            ChatContext chatContext;

            //find session
            if (Sessions.ContainsKey(sessionID))
            {
                //continue chat
                chatContext = Sessions[sessionID];
            }
            else
            {
                //start new chat
                chatContext = new ChatContext(config, sessionMessages);
                Sessions.Add(sessionID, chatContext);
            }

            //process chat
            if (chatContext.CurrentConversationID == Guid.Empty)
            {
               //we are not chatting currently so score user intent
               var aiRequest = Platform.AIService.Predict(chatContext.BotConfiguration, userStatement);

                //parse guid from prediction
                var conversationGuid = Guid.Parse(aiRequest.Prediction);

                //search for FAQ with predicted result
                var faqPrediction = chatContext.BotConfiguration.FAQs.Where(f => f.ID == conversationGuid).FirstOrDefault();

                if (faqPrediction == null)
                {
                    throw new Exception($"Conversation ID '{faqPrediction}' not found");
                }
                else
                {
                    ExecuteNodes(chatContext, faqPrediction);
                }


            }
            else
            {

            }
  





        }
        private void ExecuteNodes(ChatContext context, FAQ faq)
        {
            context.CurrentConversationID = faq.ID;
            faq.ReplyNode.Execute(context);
            context.CurrentConversationID = Guid.Empty;
        }
        private void ExecuteNodes(ChatContext context)
        {


        }
    }
}
