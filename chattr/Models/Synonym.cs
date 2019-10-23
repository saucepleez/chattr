using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace chattr.Models
{
    public class Synonym
    {
        public Guid ID { get; set; } = Guid.NewGuid();
        public string FAQWord { get; set; }
        public string SynonymWord { get; set; }
    }
}
