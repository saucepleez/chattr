using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace chattr.Models.Nodes
{
    public class _Node
    {
        public Guid ID { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public Guid NextNodeID { get; set; }

    }
}
