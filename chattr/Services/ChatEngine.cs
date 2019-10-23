using chattr.Models;
using chattr.Models.Actions;
using chattr.Models.Tasks;
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
                //var faqPrediction = chatContext.BotConfiguration.FAQs.Where(f => f.ID == conversationGuid).FirstOrDefault();
                var conversationPrediction = chatContext.BotConfiguration.Conversations.Where(f => f.ID == conversationGuid).FirstOrDefault();
                
                if (conversationPrediction != null)
                {
                    chatContext.CurrentConversation = conversationPrediction;   
                    StartActionExecution(chatContext);
                }
                else
                {
                    //conversation configuration has changed and was not retrained
                    chatContext.AddBotMessage("I found the topic reference but I was unable to find the information you wanted me to discuss.  Please retrain me on the latest configuration.");
                }
          

            }
            else if (chatContext.CurrentState == ConversationResult.State.AwaitingInput)
            {
                //update input action
                var inputAction = (InputAction)chatContext.CurrentAction;

                if (inputAction.EnforceValidation && !inputAction.AcceptableValues.Contains(userStatement))
                {
                    //validation for values failed
                    chatContext.AddBotMessage("Sorry, that was not a valid value. Valid values are: " + string.Join(", ", inputAction.AcceptableValues));
                    return;
                }
                else
                {
                    //update value
                    inputAction.UpdateInput(userStatement, chatContext);

                    //resume exection
                    StartActionExecution(chatContext);
                }



            }
  





        }
        private void StartActionExecution(ChatContext context)
        {
            //get conversation from context
            var conversation = context.CurrentConversation;
            context.CurrentConversationID = conversation.ID;

            //check current action
            Guid nextNodeID;
            if (context.CurrentState != ConversationResult.State.AwaitingInput)
            {
                //if current action is null it means we are not resuming execution, get linked item from start action
                nextNodeID = conversation.StartNode.NextNodeID;
            }
            else
            {
                //resuming execution
                nextNodeID = context.CurrentAction.NextNodeID;
            }

            //indicate we are progressing
            context.CurrentState = ConversationResult.State.Progressing;

            //determine if FAQ or Custom Conversation and Execute
            if (conversation.IsFAQ())
            {
                conversation.ReplyNode.Execute(context);
                context.CurrentState = ConversationResult.State.Completed;
            }
            else
            {
                //next node id required to execute
                if (nextNodeID == Guid.Empty)
                {
                    throw new Exception("Next Node ID cannot be empty");
                }

                //loop until we complete or break due to input node
                while (context.CurrentState == ConversationResult.State.Progressing)
                {
                    var executionResult = conversation.ExecuteAction(nextNodeID, context);
                    context.CurrentState = executionResult.ConversationState;
                    nextNodeID = executionResult.NextNodeID;         
                }
            }

            //clear the conversation id
            if (context.CurrentState == ConversationResult.State.Completed)
            {
                context.CurrentAction = null;
                context.CurrentConversationID = Guid.Empty;
            }

        }
       
    }
}
