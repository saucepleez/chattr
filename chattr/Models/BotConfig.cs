using chattr.Models.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace chattr.Models
{
    public class BotConfig
    {
        public string BotName { get; set; }
        public string ModelFile { get; set; }
        public string BotGreeting { get; set; }
        public bool UnsupervisedLearning { get; set; }
        public List<Conversation> Conversations { get; set; } = new List<Conversation>();
        public Conversation FindFAQResponse(string conversationGuid)
        {
            var guid = Guid.Parse(conversationGuid);
            var faq = Conversations.Where(f => f.ID == guid).FirstOrDefault();

            if (faq != null)
            {
              return faq;
            }
            else
            {
                throw new Exception($"FAQ '{conversationGuid}' was not found!");
            }


        }

        public void AddConversation(Conversation newConversation)
        {
            Conversations.Add(newConversation);
        }
        public void Save()
        {
            //need to use newtonsoft when serializing derived types
            Newtonsoft.Json.JsonSerializerSettings settings = new Newtonsoft.Json.JsonSerializerSettings()
            {
                TypeNameHandling = Newtonsoft.Json.TypeNameHandling.Auto
            };

            var serializedConfig = Newtonsoft.Json.JsonConvert.SerializeObject(this, settings);
            System.IO.File.WriteAllText($"Bots\\Configs\\{this.BotName}.json", serializedConfig);
        }
    }
}
