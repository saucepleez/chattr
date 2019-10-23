using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using chattr.Models.Actions;
using chattr.Models.Tasks;
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
            var helpFAQ = new Conversation();
            helpFAQ.Name = "helpSample";
            helpFAQ.AddUtterance("help");
            helpFAQ.AddUtterance("!help");
            helpFAQ.AddUtterance("what can I ask you");
            helpFAQ.AddUtterance("halp");
            helpFAQ.AddUtterance("what can you do");
            helpFAQ.AddUtterance("what can I ask");
            helpFAQ.AddResponse("This is a sample use case -- you can ask me what chattr is. I also understand various hello/goodbye phrases.");
            botConfig.Conversations.Add(helpFAQ);

            var gitFAQ = new Conversation();
            gitFAQ.Name = "gitSample";
            gitFAQ.AddUtterance("what is github");
            gitFAQ.AddUtterance("whats github");
            gitFAQ.AddUtterance("define github");
            gitFAQ.AddUtterance("github");
            gitFAQ.AddUtterance("tell me about github");
            gitFAQ.AddResponse("GitHub brings together the world's largest community of developers to discover, share, and build better software.");
            botConfig.Conversations.Add(gitFAQ);

            var stackFAQ = new Conversation();
            stackFAQ.Name = "stackFAQ";
            stackFAQ.AddUtterance("tell me about the stack");
            stackFAQ.AddUtterance("what software stack is this");
            stackFAQ.AddUtterance("what is running this");
            stackFAQ.AddUtterance("what powers this application");
            stackFAQ.AddUtterance("what is powering this app");
            stackFAQ.AddResponse("This application is powered by C# -- ML.NET and Blazor!");
            botConfig.Conversations.Add(stackFAQ);

            //create greeting FAQ
            var greetingFAQ = new Conversation();
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
            botConfig.Conversations.Add(greetingFAQ);

            //create farewell FAQ
            var farewellFAQ = new Conversation();
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
            botConfig.Conversations.Add(farewellFAQ);

            //create csharp FAQ
            var csharpFAQ = new Conversation();
            csharpFAQ.Name = "chatterFAQ";
            csharpFAQ.AddUtterance("What is chattr");
            csharpFAQ.AddUtterance("Whats chattr");
            csharpFAQ.AddUtterance("tell me about chattr");
            csharpFAQ.AddResponse("chattr is a free and open source platform to build chatbots!");
            csharpFAQ.AddSynonym("chattr", "this platform");
            botConfig.Conversations.Add(csharpFAQ);

            //create custom conversation with two responses
            var weatherRequest = new Conversation();
            weatherRequest.Name = "weatherRequest";
            weatherRequest.StartNode.Name = "weatherStart";

            //add utterances to trigger this intent
            weatherRequest.AddUtterance("Check the weather");
            weatherRequest.AddUtterance("Weather check");
            weatherRequest.AddUtterance("weather");
            weatherRequest.AddUtterance("Check Weather");

            //create reply action
            var firstReply = new ReplyAction();
            firstReply.AddResponse("The weather is 85 degrees in Florida");
            weatherRequest.Actions.Add(firstReply);
            weatherRequest.StartNode.LinkTo(firstReply);

            //create another reply
            var secondReply = new ReplyAction();
            secondReply.AddResponse("The weather is 70 degrees in New York");
            weatherRequest.Actions.Add(secondReply);
            firstReply.LinkTo(secondReply);

            botConfig.Conversations.Add(weatherRequest);


            
            //Showcase ability to conditionally check data, and ask/store what the user's color preference is.
            //On the second time, the bot will tell the stored data instead of asking for it
            var favColorConvo = new Conversation();
            favColorConvo.SetConversationName("favoriteColorPref");
            favColorConvo.SetStartActionName("favoriteColorPrefStart");
            favColorConvo.AddUtterances(new[] { "What color do I prefer?", "Ask me my favorite color", "Ask what my favorite color is", "Ask me my color preference" });

            //create input action when color is unknown
            var askForColorPreference = new InputAction();
            askForColorPreference.SetActionName("favoriteColor");
            askForColorPreference.AddResponse("Ok, tell me your color preference and I will store it between conversations.  What is your favorite color?  You can say 'red', 'blue', or 'green'.  I will not allow any other values.");
            askForColorPreference.EnableContextStorage(true, "colorpref");
            askForColorPreference.RequireValidation(true, new[] { "red", "blue", "green" });
            favColorConvo.AddAction(askForColorPreference);
      
            //create confirmation when color is unknown
            var confirmColorPreference = new ReplyAction();
            confirmColorPreference.SetActionName("replyPrefSet");
            confirmColorPreference.AddResponse("Ok, I now know your favorite color which is {colorpref}.  You can ask me this same question again and I will reply it back to you!");
            favColorConvo.AddAction(confirmColorPreference);
 
            //create confirmation reply when color is known
            var knownReply = new ReplyAction();
            knownReply.SetActionName("replyPrefKnown");
            knownReply.AddResponse("Ha! I remember you told my that your favorite color is {colorpref}.");
            favColorConvo.AddAction(knownReply);

            //create conditional check
            var performColorCheck = new IfAction();
            performColorCheck.SetActionName("colorCheck");
            favColorConvo.AddAction(performColorCheck);
           
            //Link all of the actions above together

            //Link Start > Conditional Check
            favColorConvo.LinkStartNodeTo(performColorCheck);

            //Conditional Check:
            // > Default to Input Action to get data
            // > If colorpref variable exists (this question was asked already) then skip to 'Known' Reply Node amd
            performColorCheck.DefaultTo(askForColorPreference);
            performColorCheck.Unless(IfTestType.ContextVariableExists, "", "colorpref", knownReply);

            //Link Input Action > 'Confirmation' Reply Node
            askForColorPreference.LinkTo(confirmColorPreference);

            //add this conversation to the collection
            botConfig.AddConversation(favColorConvo);


           //train a model based on the created configuration
           var modelName = Platform.AIService.Train(botConfig);
            botConfig.ModelFile = modelName;

            //save bot
            botConfig.Save();
     
        }

    }
}
