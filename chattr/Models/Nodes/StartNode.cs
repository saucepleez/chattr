using chattr.Models;
using chattr.Models.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace chattr.Models.Nodes
{
    public class StartNode : _Node
    {
        public List<Utterance> Utterances { get; set; } = new List<Utterance>();
        public List<Synonym> Synonyms { get; set; } = new List<Synonym>();
    }
}
