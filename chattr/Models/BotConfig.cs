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
        public List<FAQ> FAQs { get; set; } = new List<FAQ>();

        public FAQ FindFAQResponse(string conversationGuid)
        {
            var guid = Guid.Parse(conversationGuid);
            var faq = FAQs.Where(f => f.ID == guid).FirstOrDefault();

            if (faq != null)
            {
              return faq;
            }
            else
            {
                throw new Exception($"FAQ '{conversationGuid}' was not found!");
            }


        }
        public void Save()
        {
            var serializedConfig = System.Text.Json.JsonSerializer.Serialize(this);
            System.IO.File.WriteAllText($"Bots\\Configs\\{this.BotName}.json", serializedConfig);
        }
    }
}
