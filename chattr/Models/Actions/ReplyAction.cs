using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace chattr.Models.Actions
{
    public class ReplyAction : Action
    {
        public List<Utterance> Responses { get; set; } = new List<Utterance>();
        public string SelectedResponse { get; set; }
        public override void Execute(ChatContext context)
        {
            if (this.Responses.Count < 1)
            {
                context.AddBotMessage("I am missing a response for this conversation!");
                return;
            }

            Random rnd = new Random(DateTime.Now.Millisecond);
            SelectedResponse = Responses[rnd.Next(0, Responses.Count)].Statement;
            context.AddBotMessage(SelectedResponse);
          
        }
        public void AddResponse(string response)
        {
            this.Responses.Add(new Utterance()
            {
                ID = Guid.NewGuid(),
                Statement = response
            });
        }
        public override string GetDefaultValue()
        {
            return SelectedResponse;
        }
    }

}
