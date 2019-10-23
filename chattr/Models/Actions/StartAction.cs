using chattr.Models;
using chattr.Models.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace chattr.Models.Actions
{
    public class StartAction : Action
    {
        public List<Utterance> Utterances { get; set; } = new List<Utterance>();
        public List<Synonym> Synonyms { get; set; } = new List<Synonym>();

    }
}
