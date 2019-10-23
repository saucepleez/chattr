using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace chattr.Services
{
    /// <summary>
    /// Tracks services for use at runtime
    /// </summary>
    public static class Platform
    {
        public static AIService AIService = new AIService();
        public static ChatEngine ChatEngine = new ChatEngine();
        public static void Initialize()
        {
            CreateSampleBot();
        }

        public static void CreateSampleBot()
        {

            //create sample data
            var botConfig = new Models.BotConfig();
            botConfig.BotName = "Platform Test Bot";
            botConfig.BotGreeting = "Hi! Welcome to chattr.  chattr is an easy-to-use chatbot platform which easily lets you build and deploy chatbots.  The platform supports exact phrase matches and machine learning to classify a request.";

            //create greeting FAQ
            var helpFAQ = new Models.FAQ();
            helpFAQ.Name = "helpSample";
            helpFAQ.AddUtterance("help");
            helpFAQ.AddUtterance("!help");
            helpFAQ.AddUtterance("what can I ask you");
            helpFAQ.AddUtterance("halp");
            helpFAQ.AddUtterance("what can you do");
            helpFAQ.AddUtterance("what can I ask");
            helpFAQ.AddResponse("This is a sample use case -- you can ask me what chattr is. I also understand various hello/goodbye phrases.");
            botConfig.FAQs.Add(helpFAQ);

            var gitFAQ = new Models.FAQ();
            gitFAQ.Name = "gitSample";
            gitFAQ.AddUtterance("what is github");
            gitFAQ.AddUtterance("whats github");
            gitFAQ.AddUtterance("define github");
            gitFAQ.AddUtterance("github");
            gitFAQ.AddUtterance("tell me about github");
            gitFAQ.AddResponse("GitHub brings together the world's largest community of developers to discover, share, and build better software.");
            botConfig.FAQs.Add(gitFAQ);

            var stackFAQ = new Models.FAQ();
            stackFAQ.Name = "stackFAQ";
            stackFAQ.AddUtterance("tell me about the stack");
            stackFAQ.AddUtterance("what software stack is this");
            stackFAQ.AddUtterance("what is running this");
            stackFAQ.AddUtterance("what powers this application");
            stackFAQ.AddUtterance("what is powering this app");
            stackFAQ.AddResponse("This application is powered by C# -- ML.NET and Blazor!");
            botConfig.FAQs.Add(stackFAQ);

            //create greeting FAQ
            var greetingFAQ = new Models.FAQ();
            greetingFAQ.Name = "greetingSample";
            greetingFAQ.AddUtterance("hello");
            greetingFAQ.AddUtterance("hi");
            greetingFAQ.AddUtterance("hi!");
            greetingFAQ.AddUtterance("hey");
            greetingFAQ.AddUtterance("good morning");
            greetingFAQ.AddUtterance("whats up");
            greetingFAQ.AddUtterance("how are you?");
            greetingFAQ.AddUtterance("Hi! How can I help you today?");
            greetingFAQ.AddResponse("Hi! How can I help you today?");
            greetingFAQ.AddResponse("Hey!");
            greetingFAQ.AddResponse("Hello. What can I do for you?");
            botConfig.FAQs.Add(greetingFAQ);

            //create farewell FAQ
            var farewellFAQ = new Models.FAQ();
            farewellFAQ.Name = "farewellSample";
            farewellFAQ.AddUtterance("goodbye");
            farewellFAQ.AddUtterance("bye");
            farewellFAQ.AddUtterance("later");
            farewellFAQ.AddUtterance("see ya");
            farewellFAQ.AddUtterance("c ya");
            farewellFAQ.AddUtterance("lates");
            farewellFAQ.AddUtterance("Bye!");
            farewellFAQ.AddUtterance("ttyl");
            farewellFAQ.AddResponse("Take Care!");
            botConfig.FAQs.Add(farewellFAQ);

            //create csharp FAQ
            var csharpFAQ = new Models.FAQ();
            csharpFAQ.Name = "chatterFAQ";
            csharpFAQ.AddUtterance("What is chattr");
            csharpFAQ.AddUtterance("Whats chattr");
            csharpFAQ.AddUtterance("tell me about chattr");
            csharpFAQ.AddResponse("chattr is a free and open source platform to build chatbots!");
            csharpFAQ.AddSynonym("chattr", "this platform");
            botConfig.FAQs.Add(csharpFAQ);

            //train a model based on the created configuration
            var modelName = Platform.AIService.Train(botConfig);
            botConfig.ModelFile = modelName;

            //save bot
            botConfig.Save();

        }

    }
}
