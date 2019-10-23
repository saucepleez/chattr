# chattr

**EARLY ALPHA -- This is currently a "Work In Progress" and has no production-grade version yet**

chattr is an easy-to-use chatbot service platform which enables everyone to build conversational chatbots! chattr is powered by ML.NET and Blazor (Razor Components) and is fully free and open-source with no licensing or usage limits. 

## How To Get Started

- Ensure .NET Core 3 is installed and Visual Studio 2019 (For Full Support), no database required
- Download Solution, Restore Packages, Run Solution
- At Startup, PlatformServices will automatically build a sample chatbot configuration (json) and train a model (zip)
- Once created, the bot will be automatically shown on the home page which you can conversate with -- please note the vocabulary and samples are limited, see the **Testing** section below for more info

## Testing
Currently, this demo is limited as it supports a single bot and only supports FAQ-style conversations (Simple Question and Response).

Once project is debugging, you can ask the bot some statements that have been created such as "What is this platform?".  Note that due to the machine learning you can also make grammatical mistakes such as "What izz this plattform?" and the bot will still attempt to identify the best match. Additional trained FAQs in the sample are "What powers you"/"What is the stack" and "what is github". You can also greet the bot multiple times with "Hi", "Hiya", "Hey", "Heya" or similar and it will return one of multiple replies.

You may also load the editor page which allows you to customize frequently asked questions (with custom conversations coming soon!) as well as view the sample ones that were created.  If you decide to make any changes, press the "Save" button then "Train" to train the bot on your changes.  Lastly you can navigate back to the Home screen to chat with your newly trained version.

Please note that every time the project is loaded, it recreates and retrains the sample use case and will wipe your changes!

## What is the goal?
The goal of this project is to enable everyone to be able to build and create chatbots or "digital assistants" with ease. Typically, conversations are created and based around specific domain data, such as a bot that can provide helpful links, regurgitate definitions, or perform odd jobs such as check the weather.  The bot will need to be given sample phrases similar to the ones actual users would use to resolve what the user is asking for.

Powered by open-source, your data will never leave the server as it uses ML.NET to internally train machine learning models for use in creating predictions.

## Screenshots

![Home Page](https://i.imgur.com/mrx10ax.png)

![Edit](https://i.imgur.com/f8p7uRh.png)
