using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace chattr.Models
{
    public class Utterance
    {
        public Guid ID { get; set; } = Guid.NewGuid();
        public string Statement { get; set; }

    }
}
