using chattr.Models;
using chattr.Models.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace chattr.Models.Actions
{
    public class InputAction : ReplyAction
    {
        public string UserSays { get; set; }
        public bool StoreInContext { get; set; }
        public string ContextVariableName { get; set; }
        public bool EnforceValidation { get; set; }
        public List<string> AcceptableValues { get; set; } = new List<string>();
        public override void Execute(ChatContext context)
        {
            if (this.Responses.Count < 1)
            {
                context.AddBotMessage("I am missing a response for this conversation!");
                return;
            }

            Random rnd = new Random(DateTime.Now.Millisecond);
            context.AddBotMessage(Responses[rnd.Next(0, Responses.Count)].Statement);

        }
        public void UpdateInput(string input, ChatContext context)
        {
            this.UserSays = input;

            if (StoreInContext)
            {
                if (ContextVariableName == string.Empty)
                    ContextVariableName = this.Name;

                //overwrite or append
                if (context.ContextVariables.ContainsKey(ContextVariableName))
                {
                    context.ContextVariables[ContextVariableName] = input;
                }
                else
                {
                    context.ContextVariables.Add(ContextVariableName, input);
                }

              
            }


        }
        public void EnableContextStorage(bool value, string variableName = "")
        {
            this.StoreInContext = value;

            if (value && variableName != "")
            {
                this.ContextVariableName = variableName;
            }
        }

        public void RequireValidation(bool value, string[] acceptableValues = null)
        {
            this.EnforceValidation = value;
            this.AcceptableValues.AddRange(acceptableValues);
        }

        public override string GetDefaultValue()
        {
            return UserSays;
        }

    }
}
