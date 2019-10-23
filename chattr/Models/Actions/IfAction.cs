using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace chattr.Models.Actions
{
    public class IfAction : Action
    {
        public List<IfTest> IfTests { get; set; } = new List<IfTest>();

        public override void Execute(ChatContext context)
        {

            //check next node default was set
            if (this.NextNodeID == Guid.Empty)
            {
                throw new Exception("IfNode must have a default node assigned.");
            }

            //loop each test and execute
            foreach (var ifStatement in IfTests)
            {
                //if test came back true
                var result = ifStatement.ExecuteTest(context);

                //if true then update next node id
                if (result)
                {
                    this.NextNodeID = ifStatement.NextNodeID;
                    break;
                }

            }

 
        }
        public void DefaultTo(Action defaultAction)
        {
            LinkTo(defaultAction);
        }

        public void Unless(IfTestType type, string value1, string value2, Action nextAction)
        {
            IfTests.Add(new IfTest()
            {
                IfType = type,
                Value1 = value1,
                Value2 = value2,
                NextNodeID = nextAction.ID,
                CaseSensitiveTest = false
                
            });
        }
    }

    public class IfTest
    {
        public Guid IfTestID { get; set; } = Guid.NewGuid();
        public IfTestType IfType { get; set; }
        public string Value1 { get; set; }
        public string Value2 { get; set; }
        public Guid NextNodeID { get; set; }
        public bool CaseSensitiveTest { get; set; }

        public bool ExecuteTest(ChatContext context)
        {
           
                      
            //convert variables
            var value1 = context.ConvertVariables(Value1);
            var value2 = context.ConvertVariables(Value2);

            //determine case sensitivity
            if (!(CaseSensitiveTest))
            {
                value1 = value1.ToLower();
                value2 = value2.ToLower();
            }

            //perform comparison
            switch (IfType)
            {
                case IfTestType.ContextVariableExists:
                    return context.ContextVariables.ContainsKey(value2);
                case IfTestType.EqualTo:
                    return value1 == value2;
                case IfTestType.NotEqual:
                    return value1 != value2;
                case IfTestType.Contains:
                    return value1.Contains(value2);
                case IfTestType.DoesNotContain:
                    return !(value1.Contains(value2));
                case IfTestType.LessThan:
                    {
                        var cValue1 = int.Parse(value1);
                        var cValue2 = int.Parse(value2);
                        return cValue1 < cValue2;
                    }
                case IfTestType.GreaterThan:
                    {
                        var cValue1 = int.Parse(value1);
                        var cValue2 = int.Parse(value2);
                        return cValue1 > cValue2;
                    }
                case IfTestType.GreaterThanOrEqualTo:
                    {
                        var cValue1 = int.Parse(value1);
                        var cValue2 = int.Parse(value2);
                        return cValue1 >= cValue2;
                    }
                case IfTestType.LessThanOrEqualTo:
                    {
                        var cValue1 = int.Parse(value1);
                        var cValue2 = int.Parse(value2);
                        return cValue1 <= cValue2;
                    }
                default:
                    throw new NotImplementedException("If Type '" + IfType.ToString() + "' not implememented.");
            }
        }
    }
    public enum IfTestType
    {
        EqualTo, LessThan, GreaterThan, Contains, NotEqual, GreaterThanOrEqualTo, LessThanOrEqualTo, DoesNotContain, ContextVariableExists
    }


}
