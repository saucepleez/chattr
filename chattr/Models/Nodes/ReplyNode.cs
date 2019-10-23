using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace chattr.Models.Nodes
{
    public class ReplyNode : _Node
    {
        public List<Utterance> Responses { get; set; } = new List<Utterance>();

        public void Execute(ChatContext context)
        {
            if (this.Responses.Count < 1)
            {
                context.AddBotMessage("I am missing a response for this conversation!");
            }

            Random rnd = new Random(DateTime.Now.Millisecond);
            context.AddBotMessage(Responses[rnd.Next(0, Responses.Count)].Statement);
          
        }
    }

}
