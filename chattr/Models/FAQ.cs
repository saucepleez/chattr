using chattr.Models.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace chattr.Models
{
    public class FAQ : Conversation
    {
        public StartNode StartNode { get; set; } = new StartNode();
        public ReplyNode ReplyNode { get; set; } = new ReplyNode();
        public void AddUtterance(string utterance)
        {
            this.StartNode.Utterances.Add(new Utterance()
            {
                ID = Guid.NewGuid(),
                Statement = utterance
            });
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
    }
}
