using chattr.Models.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace chattr.Models.Tasks
{
    public class Conversation
    {
        public Guid ID { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public StartAction StartNode { get; set; } = new StartAction();
        public ReplyAction ReplyNode { get; set; } = new ReplyAction();

        public List<Actions.Action> Actions = new List<Actions.Action>();
        public void AddUtterance(string utterance)
        {
            this.StartNode.Utterances.Add(new Utterance()
            {
                ID = Guid.NewGuid(),
                Statement = utterance
            });
        }
        public void SetConversationName(string name)
        {
            this.Name = name;
        }
        public void SetStartActionName(string name)
        {
            this.StartNode.Name = name;
        }
        public void LinkStartNodeTo(Actions.Action action)
        {
            this.StartNode.NextNodeID = action.ID;
        }
        public void AddUtterances(string[] utterances)
        {
            foreach (var utterance in utterances)
            {
                this.StartNode.Utterances.Add(new Utterance()
                {
                    ID = Guid.NewGuid(),
                    Statement = utterance
                });
            }
            
        }
        public void AddResponse(string response)
        {
            this.ReplyNode.Responses.Add(new Utterance()
            {
                ID = Guid.NewGuid(),
                Statement = response
            });
        }
        public void AddSynonym(string sourceWord, string synonymWord)
        {
            this.StartNode.Synonyms.Add(new Synonym()
            {
                ID = Guid.NewGuid(),
                FAQWord = sourceWord,
                SynonymWord = synonymWord
            });
        }

        public void AddAction(Actions.Action action)
        {
            this.Actions.Add(action);
        }
        public ConversationResult ExecuteAction(Guid nextNodeID, ChatContext context)
        {
            //create result for execution tracking
            ConversationResult result = new ConversationResult();

            if (nextNodeID == Guid.Empty)
            {
                throw new Exception("Next Node ID cannot be empty");
            }
            
            //find and execute action
            var action = Actions.Where(f => f.ID == nextNodeID).FirstOrDefault();

           
            if (action == null)
            {
                throw new Exception("Next Node not found!");
            }
            else {
                context.CurrentAction = action;
                action.Execute(context);
            }



            //define result      
            if (action is InputAction)
            {
                result.ConversationState = ConversationResult.State.AwaitingInput;
                result.NextNodeID = action.NextNodeID;
            }
            else if (action.NextNodeID == Guid.Empty)
            {
                result.ConversationState = ConversationResult.State.Completed;
            }
            else if(action.NextNodeID == action.ID)
            {
                throw new Exception("Circular Reference");
            }
            else
            {
                result.ConversationState = ConversationResult.State.Progressing;
                result.NextNodeID = action.NextNodeID;
            }

            //return result
            return result;
               
        }
        public bool IsFAQ()
        {
            if (Actions.Count == 0)
            { 
                return true; 
            }
            else
            {
                return false;
            }
             
        }

              

    }

    public class ConversationResult
    {
        public Guid NextNodeID { get; set; }
        public State ConversationState { get; set; }
        public enum State
        {
            Progressing,
            AwaitingInput,
            Completed
        }
    }
}
