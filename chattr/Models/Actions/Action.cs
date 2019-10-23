using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace chattr.Models.Actions
{
    public class Action
    {
        public Guid ID { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public Guid NextNodeID { get; set; }

        public virtual void Execute(ChatContext context)
        {

        }

        public void LinkTo(Actions.Action nextAction)
        {
            this.NextNodeID = nextAction.ID;
        }
        public virtual string GetDefaultValue()
        {
            return string.Empty;
        }
        public void SetActionName(string name)
        {
            this.Name = name;
        }
    }
}
