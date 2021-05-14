[![Build Status](https://dev.azure.com/BotBuilder-Community/dotnet/_apis/build/status/BotBuilderCommunity.botbuilder-community-dotnet?branchName=master)](https://dev.azure.com/BotBuilder-Community/dotnet/_build/latest?definitionId=1&branchName=master) [![Bot Builder Community NuGet](https://img.shields.io/badge/NuGet-blue.svg)](https://www.nuget.org/profiles/BotBuilderCommunity)

**Latest release: v4.13.0 (05/11/2021) - [Release Notes](https://github.com/BotBuilderCommunity/botbuilder-community-dotnet/releases/tag/v4.13.0)**

# Bot Framework Community - .NET Components &amp; Extensions

This repository is part of the Bot Builder Community Project and contains Bot Framework Components and other extensions for use with Bot Framework Composer and the Bot Framework .NET SDK, including adapters, middleware, dialogs, helpers and more. Other repos within the Bot Builder Community Project exist for extensions for [JavaScript](https://github.com/BotBuilderCommunity/botbuilder-community-js), [Python](https://github.com/BotBuilderCommunity/botbuilder-community-python) and [tools](https://github.com/BotBuilderCommunity/botbuilder-community-tools) - you can find our other repos under [our GitHub organisation for the project](https://github.com/BotBuilderCommunity/).  

You can also read about how to [contribute and report issues](#contributing-and-reporting-issues).

Use the links below to jump to a section to see a list of current components and other projects / packages available for use with Composer and the Bot Builder .NET SDK. 

This repository contains 

- **[Bot Framework Components](#bot-framework-components)** - Primarily used with through [Bot Framework Composer](https://github.com/Microsoft/BotFramework-Composer) and the Package Manager. 
- **[Bot Builder SDK Packages](#other-net-sdk-packages-non-component)** - For use with Bot Builder .NET SDK projects. Require manual installation / configuration and cannot be used with the Bot Framework Composer Package Manager.

## Bot Framework Components

Components are part of the component model for building bots with re-usable building blocks. You can learn more about components and the component model [here](https://github.com/microsoft/botframework-components#bot-framework-components).  You'll primarily use components through [Bot Framework Composer](https://github.com/Microsoft/BotFramework-Composer) - a visual bot authoring canvas for developers. From Composer you can add and remove packages from your bot.

- [Custom Actions](#custom-actions)
- [Adapters / Composer Connections](#composer-connections--adatpers)
- [Live Agent Handoff](#live-agent-handoff)
- [Pre-built Adaptive Dialogs](#pre-built-adaptive-dialogs)
- [Other Components](#other-components)

### Custom Actions

| Name | Description | Sample? | NuGet |
| ------ | ------ | ------ | ------ |
| [Call Dialogs](libraries/Bot.Builder.Community.Components.CallDialogs) | Actions for calling dialogs in parallel. Normally composer dialogs are executed in series one after the other, but using the AddDialogCall action you can build up a list of dialogs to call in parallel using the CallDialogs action. | [Sample](https://github.com/BotBuilderCommunity/botbuilder-community-dotnet/blob/develop/samples/components/CallDialogSample) | [![NuGet version](https://img.shields.io/badge/NuGet-blue.svg)](https://www.nuget.org/packages/Bot.Builder.Community.Components.CallDialogs/) |
| [Storage Actions](libraries/Bot.Builder.Community.Components.Storage) | Actions (read / write / delete) for reading and writing items to the bots configured storage provider. | [Sample](https://github.com/BotBuilderCommunity/botbuilder-community-dotnet/tree/develop/samples/components/StorageSample) | [![NuGet version](https://img.shields.io/badge/NuGet-blue.svg)](https://www.nuget.org/packages/Bot.Builder.Community.Components.Dialogs.Input/) |
| [Input Prompts](libraries/Bot.Builder.Community.Components.Dialogs.Input) | Prompt actions for accepting input from users. Contains actions for accepting email, phone number and social media mentions / hashtags. | [Sample](https://github.com/BotBuilderCommunity/botbuilder-community-dotnet/tree/develop/samples/components/CustomInputSample) | [![NuGet version](https://img.shields.io/badge/NuGet-blue.svg)](https://www.nuget.org/packages/Bot.Builder.Community.Components.Dialogs.Input/) |

### Composer Connections / Adapters

| Name | Description | Sample? | NuGet |
| ------ | ------ | ------ | ------ |
| [Amazon Alexa](libraries/Bot.Builder.Community.Components.Adapters.Alexa) | Add a bot to Amazon Alexa devices, via Alexa Skills. Includes broad support for Alexa Skills capabilities, including devices with displays (Show / Spot), Alexa Cards, access to user profile data and the ability to send Progressive Responses. |  | [![NuGet version](https://img.shields.io/badge/NuGet-blue.svg)](https://www.nuget.org/packages/Bot.Builder.Community.Components.Adapters.Alexa/) |
| [Google Actions SDK (latest)](libraries/Bot.Builder.Community.Components.Adapters.ActionsSDK) | Add a bot to Google Assistant devices using Google Actions and the latest Actions SDK. Includes broad support for Google Actions capabilities, including Cards and suggestion chips |  | [![NuGet version](https://img.shields.io/badge/NuGet-blue.svg)](https://www.nuget.org/packages/Bot.Builder.Community.Components.Adapters.Google/) |
| [Azure Communication Services SMS](libraries/Bot.Builder.Community.Components.Adapters.ACS.SMS) | Allows a bot to communicate via Azure Communication Services SMS, to send / reveive SMS messages, including receiving delivery reports. | | [![NuGet version](https://img.shields.io/badge/NuGet-blue.svg)](https://www.nuget.org/packages/Bot.Builder.Community.Components.Adapters.ACS.SMS/) |
| [Zoom](libraries/Bot.Builder.Community.Components.Adapters.Zoom) | Enable a bot to communicate via Zoom. Specifically developed to handle Zoom chatbot app requests and allow for the sending of messages using Zoom Message Templates. The adapter will also receieve any event subscribed to via Zoom, even those not chatbot related. |  | [![NuGet version](https://img.shields.io/badge/NuGet-blue.svg)](https://www.nuget.org/packages/Bot.Builder.Community.Components.Adapters.Zoom/) |

### Live Agent Handoff

| Name | Description | Sample? | NuGet |
| ------ | ------ | ------ | ------ |
| [LivePerson](libraries/Bot.Builder.Community.Components.Handoff.LivePerson) | Enables handing off a bot conversation to the LivePerson platform, to either a LivePerson virtual agent or a human agent. |  | [![NuGet version](https://img.shields.io/badge/NuGet-blue.svg)](https://www.nuget.org/packages/Bot.Builder.Community.Components.Handoff.LivePerson/) |
| [ServiceNow](libraries/Bot.Builder.Community.Components.Handoff.ServiceNow) | Enables handing off a bot conversation to the ServiceNow platform, allowing the ServiceNow virtual assistant to handle the conversation. |  | [![NuGet version](https://img.shields.io/badge/NuGet-blue.svg)](https://www.nuget.org/packages/Bot.Builder.Community.Components.Handoff.ServiceNow/) |

### Pre-built Adaptive Dialogs

| Name | Description | Sample? | NuGet |
| ------ | ------ | ------ | ------ |
| [Get Weather](libraries/Bot.Builder.Community.Components.Dialogs.GetWeather) | Declarative assets supporting scenarios for "getWeather" utterances. |  | [![NuGet version](https://img.shields.io/badge/NuGet-blue.svg)](https://www.nuget.org/packages/Bot.Builder.Community.Components.Dialogs.GetWeather/) |

### Other Components

| Name | Description | Sample? | NuGet |
| ------ | ------ | ------ | ------ |
| [Token Exchange Skill Handler](libraries/Bot.Builder.Community.Components.TokenExchangeSkillHandler) | A CloudSkillHandler component for enabling Single Sign On Token Exchange between a root bot and skill bot. |  | [![NuGet version](https://img.shields.io/badge/NuGet-blue.svg)](https://www.nuget.org/packages/Bot.Builder.Community.Components.TokenExchangeSkillHandler/) |


## Other .Net SDK Packages (non-component)

The projects and packages contained within this section are available as traditional NuGet packages. i.e. they are not Bot Framework Components and can therefore not be used with Bot Framework Composer's Package Manager.

Over time we plan to make Components for each of these projects available and we will link to those components from each project as they become available. 

- [Adapters](#adapters)
- [Dialogs and Prompts](#dialogs-and-prompts)
- [Cards](#cards)
- [Middleware](#middleware)
- [Recognizers](#recognizers)
- [Storage](#storage)

### Adapters

The following adapters can be used to expose your bot on additional channels not supported by the Azure Bot Service, such as Alexa.

| Name | Description | Sample? | NuGet |
| ------ | ------ | ------ | ------ |
| [Azure Communication Services SMS Adapter](libraries/Bot.Builder.Community.Adapters.ACS.SMS) | An adapter to integrate with Azure Communication Services to send / reveive SMS messages, including receiving delivery reports. | [Sample](https://github.com/BotBuilderCommunity/botbuilder-community-dotnet/tree/master/samples/ACS%20SMS%20Adapter%20Sample) | [![NuGet version](https://img.shields.io/badge/NuGet-blue.svg)](https://www.nuget.org/packages/Bot.Builder.Community.Adapters.ACS.SMS/) |
| [Alexa Adapter](libraries/Bot.Builder.Community.Adapters.Alexa) | An adapter to allow for Alexa Skills to be built using the Bot Builder SDK. Includes broad support for Alexa Skills capabilities, including devices with displays (Show / Spot), Alexa Cards, access to user profile data and the ability to send Progressive Responses. | [Sample](https://github.com/BotBuilderCommunity/botbuilder-community-dotnet/tree/master/samples/Alexa%20Adapter%20Sample) | [![NuGet version](https://img.shields.io/badge/NuGet-blue.svg)](https://www.nuget.org/packages/Bot.Builder.Community.Adapters.Alexa/) |
| [Google Actions SDK (latest) Adapter](libraries/Bot.Builder.Community.Adapters.ActionsSDK) | An adapter to allow for Google Actions to be built using the Bot Builder SDK via the latest Actions SDK (note: DialogFlow and legacy Actions SDK projects should use [this alternate adapter](libraries/Bot.Builder.Community.Adapters.Google). Includes broad support for Google Actions capabilities, including Cards and suggestion chips | [Sample](https://github.com/BotBuilderCommunity/botbuilder-community-dotnet/tree/master/samples/Actions%20SDK%20Adapter%20Sample) | [![NuGet version](https://img.shields.io/badge/NuGet-blue.svg)](https://www.nuget.org/packages/Bot.Builder.Community.Adapters.Google/) |
| [Google DialogFlow and legacy Actions SDK Adapter](libraries/Bot.Builder.Community.Adapters.Google) | This adapter supports integration with DialogFlow and legacy Actions SDK projects with the Bot Builder SDK. Includes broad support for Google Actions capabilities, including Cards and suggestion chips | [Sample](https://github.com/BotBuilderCommunity/botbuilder-community-dotnet/tree/master/samples/Google%20Adapter%20Sample) | [![NuGet version](https://img.shields.io/badge/NuGet-blue.svg)](https://www.nuget.org/packages/Bot.Builder.Community.Adapters.Google/) |
| [Twitter Adapter](libraries/Bot.Builder.Community.Adapters.Twitter) | An adapter that integrates Twitter Direct Messages with the Bot Builder. The adapter sets up the required webhooks and responds to CRC requests. The webhooks code is based on the work by Tweety with modifications to support the Premium tier of the Account Activity API. | [Sample](https://github.com/BotBuilderCommunity/botbuilder-community-dotnet/tree/master/samples/Twitter%20Adapter%20Sample) | [![NuGet version](https://img.shields.io/badge/NuGet-blue.svg)](https://www.nuget.org/packages/Bot.Builder.Community.Adapters.Twitter/) |
| [RingCentral Adapter](libraries/Bot.Builder.Community.Adapters.RingCentral) | An adapter that integrates the RingCentral Platform API with the Bot Builder. The adapter sets up the required webhooks and endpoints to RingCentral requests. Supporting features such as human handoff, activity publishing and WhatsApp support. | [Sample](https://github.com/BotBuilderCommunity/botbuilder-community-dotnet/tree/master/samples/RingCentral%20Adapter%20Sample) | [![NuGet version](https://img.shields.io/badge/NuGet-blue.svg)](https://www.nuget.org/packages/Bot.Builder.Community.Adapters.RingCentral/) |
| [Zoom Adapter](libraries/Bot.Builder.Community.Adapters.Zoom) | An adapter that accepts and handles Zoom app requests. Specifically developed to handle Zoom chatbot app requests and allow for the sending of messages using Zoom Message Templates. The adapter will also receieve any event subscribed to via Zoom, even those not chatbot related. | [Sample](https://github.com/BotBuilderCommunity/botbuilder-community-dotnet/tree/develop/samples/Zoom%20Adapter%20Sample) | [![NuGet version](https://img.shields.io/badge/NuGet-blue.svg)](https://www.nuget.org/packages/Bot.Builder.Community.Adapters.Zoom/) |
| [Infobip WhatsApp Adapter](libraries/Bot.Builder.Community.Adapters.Infobip.WhatsApp) | An adapter that accepts and hanldes WhatsApp app requests via Infobip, including support for various messages and template types. | | [![NuGet version](https://img.shields.io/badge/NuGet-blue.svg)](https://github.com/BotBuilderCommunity/botbuilder-community-dotnet/tree/develop/libraries/Bot.Builder.Community.Adapters.Infobip.WhatsApp/) |
| [Infobip SMS Adapter](libraries/Bot.Builder.Community.Adapters.Infobip.Sms) | An adapter that accepts and hanldes SMS requests via Infobip, including support for various messages. | | [![NuGet version](https://img.shields.io/badge/NuGet-blue.svg)](https://github.com/BotBuilderCommunity/botbuilder-community-dotnet/tree/develop/libraries/Bot.Builder.Community.Adapters.Infobip.Sms/) |
| [Infobip Viber Adapter](libraries/Bot.Builder.Community.Adapters.Infobip.Viber) | An adapter that accepts and hanldes Viber requests via Infobip, including support for various messages. | | [![NuGet version](https://img.shields.io/badge/NuGet-blue.svg)](https://github.com/BotBuilderCommunity/botbuilder-community-dotnet/tree/develop/libraries/Bot.Builder.Community.Adapters.Infobip.Viber/) |
| [MessageBird WhatsApp Adapter](libraries/Bot.Builder.Community.Adapters.MessageBird) | An adapter that accepts and handles WhatsApp app requests via MessageBird, including support for various message types. | [Sample](https://github.com/BotBuilderCommunity/botbuilder-community-dotnet/tree/develop/samples/MessageBird%20Adapter%20Sample) | [![NuGet version](https://img.shields.io/badge/NuGet-blue.svg)](https://www.nuget.org/packages/Bot.Builder.Community.Adapters.MessageBird/) |

### Dialogs and Prompts

The following dialogs are currently available;

| Name | Description | Sample | NuGet |
| ------ | ------ | ------ | ------ |
| [Bot Builder v4 Location Dialog](libraries/Bot.Builder.Community.Dialogs.Location) | An implemention for v4 of the Bot Build .NET SDK of the [Microsoft.Bot.Builder.Location dialog project built for Bot Builder v3](https://github.com/microsoft/botbuilder-location). An open-source location picker control for Microsoft Bot Framework powered by Azure or Bing Maps REST services. This control will allow a user to search for a location, with the ability to specify required fields and also store locations as favorites for the user. | [Sample](https://github.com/BotBuilderCommunity/botbuilder-community-dotnet/tree/master/samples/Location%20Dialog%20Sample) | [![NuGet version](https://img.shields.io/badge/NuGet-blue.svg)](https://www.nuget.org/packages/Bot.Builder.Community.Dialogs.Location/) |
| [Bot Builder ChoiceFlow](libraries/Bot.Builder.Community.Dialogs.ChoiceFlow) |This dialog allows you to provide the user with a series of guides choice prompts in turn (defined in a JSON file or as a collection of ChoiceFlowItem objects), similar to when calling a telephone line with a series of automated options. The dialog returns the user's last choice as a result from the dialog and optionally provide the user with a simple text response depending on which choice they land on. | [Sample](https://github.com/BotBuilderCommunity/botbuilder-community-dotnet/tree/master/samples/ChoiceFlow%20Dialog%20Sample) | [![NuGet version](https://img.shields.io/badge/NuGet-blue.svg)](https://www.nuget.org/packages/Bot.Builder.Community.Dialogs.ChoiceFlow/) |
| [Bot Builder v4 FormFlow](libraries/Bot.Builder.Community.Dialogs.FormFlow) | An implemention for v4 of the Bot Build .NET SDK of the [Microsoft.Bot.Builder.FormFlow dialog project built for Bot Builder v3](https://github.com/Microsoft/BotBuilder-V3/tree/master/CSharp/Library/Microsoft.Bot.Builder/FormFlow). FormFlow automatically generates the dialogs that are necessary to manage a guided conversation, based upon guidelines you specify. | [Sample](https://github.com/BotBuilderCommunity/botbuilder-community-dotnet/tree/master/samples/Form%20Flow%20Sample) | [![NuGet version](https://img.shields.io/badge/NuGet-blue.svg)](https://www.nuget.org/packages/Bot.Builder.Community.Dialogs.FormFlow/) |
| [Bot Builder v4 Prompts](libraries/Bot.Builder.Community.Dialogs.Prompts) | A collection of Prompts for use with Bot Builder v4, providing the ability to prompt using Adaptive Cards and for recognizing currencies, age, distances and temperature. | | [![NuGet version](https://img.shields.io/badge/NuGet-blue.svg)](https://www.nuget.org/packages/Bot.Builder.Community.Dialogs.Prompts/) |
| [Bot Builder v4 Luis Dialog](libraries/Bot.Builder.Community.Dialogs.Luis) | An implementation for v4 of the Bot Builder .NET SDK of the [Microsoft.Bot.Builder.Dialogs.LuisDialog built for Bot Builder V3](https://github.com/microsoft/BotBuilder-V3/tree/master/CSharp/Library/Microsoft.Bot.Builder/Luis) A dialog specialized to handle intents and entities from LUIS. | [Sample](https://github.com/BotBuilderCommunity/botbuilder-community-dotnet/tree/master/samples/Luis%20Dialog%20Sample) | [![NuGet version](https://img.shields.io/badge/NuGet-blue.svg)](https://www.nuget.org/packages/Bot.Builder.Community.Dialogs.Luis/) |
| [Adaptive Dialogs - REST](libraries/Bot.Builder.Community.Dialogs.Adaptive.Rest) | Adaptive package containing additional actions to make calling REST endpoints easier when using Adaptive dialogs | | [![NuGet version](https://img.shields.io/badge/NuGet-blue.svg)](https://www.nuget.org/packages/Bot.Builder.Community.Dialogs.Adaptive.Rest/) |

### Cards

Packages that assist in building bots using Adaptive Cards and Bot Framework Cards;

| Name | Description | Sample? | NuGet |
| ------ | ------ | ------ | ------ |
| [Cards Library](libraries/Bot.Builder.Community.Cards) | The cards library currently has two main features - Both Adaptive Cards and Bot Framework cards can be disabled and Adaptive Cards can be translated | | [![NuGet version](https://img.shields.io/badge/NuGet-blue.svg)](https://www.nuget.org/packages/Bot.Builder.Community.Middleware.HandleActivityType/) |
| [Adaptive Card Prompt](libraries/Bot.Builder.Community.Dialogs.Prompts) | This prompt is available as part of the Bot.Builder.Community.Prompts package. It includes validation for specified required input fields, displays a custom message if user replies via text and not card input and ensures input is only valid if it comes from the appropriate card (not one shown previous to prompt).| | [![NuGet version](https://img.shields.io/badge/NuGet-blue.svg)](https://www.nuget.org/packages/Bot.Builder.Community.Prompts/) |

### Middleware

The following pieces of middleware are currently available;

| Name | Description | Sample? | NuGet |
| ------ | ------ | ------ | ------ |
| [Handle Activity Type Middleware](libraries/Bot.Builder.Community.Middleware.HandleActivityType) | Middleware component which allows you to respond to different types of incoming activities, e.g. send a greeting, or even filter out activities you do not care about altogether. | | [![NuGet version](https://img.shields.io/badge/NuGet-blue.svg)](https://www.nuget.org/packages/Bot.Builder.Community.Middleware.HandleActivityType/) |
| [BestMatch Middleware](libraries/Bot.Builder.Community.Middleware.BestMatch) | A middleware implementation of the popular open source BestMatchDialog for v3 of the SDK. This piece of middleware will allow you to match a message received from a bot user against a list of strings and then carry out an appropriate action. Matching does not have to be exact and you can set the threshold as to how closely the message should match with an item in the list. | [Sample](https://github.com/BotBuilderCommunity/botbuilder-community-dotnet/tree/master/samples/BestMatch%20Middleware%20Sample) | [![NuGet version](https://img.shields.io/badge/NuGet-blue.svg)](https://www.nuget.org/packages/Bot.Builder.Community.Middleware.BestMatch/) |
| [Azure Active Directory Authentication Middleware](libraries/Bot.Builder.Community.Middleware.AzureAdAuthentication) | This middleware will allow your bot to authenticate with Azure AD. It was created to support integration with Microsoft Graph but it will work with any application that uses the OAuth 2.0 authorization code flow. | | [![NuGet version](https://img.shields.io/badge/NuGet-blue.svg)](https://www.nuget.org/packages/Bot.Builder.Community.Middleware.AzureAdAuthentication/) |
| [Sentiment Analysis Middleware](libraries/Bot.Builder.Community.Middleware.SentimentAnalysis) | This middleware uses Cognitive Services Sentiment Analysis to identify the sentiment of each inbound message and make it available for your bot or other middleware component. | [Sample](https://github.com/BotBuilderCommunity/botbuilder-community-dotnet/tree/develop/samples/Sentiment%20Middleware%20Sample) | [![NuGet version](https://img.shields.io/badge/NuGet-blue.svg)](https://www.nuget.org/packages/Bot.Builder.Community.Middleware.SentimentAnalysis/) |
| [Spell Check Middleware](libraries/Bot.Builder.Community.Middleware.SpellCheck) | This middleware uses Cognitive Services Check to automatically correct inbound message text | | [![NuGet version](https://img.shields.io/badge/NuGet-blue.svg)](https://www.nuget.org/packages/Bot.Builder.Community.Middleware.SpellCheck/) |

### Recognizers

The following recognizers are currently available;

| Name | Description | NuGet |
| ------ | ------ | ------ |
| [Fuzzy Matching Recognizer](libraries/Bot.Builder.Community.Recognizers.FuzzyRecognizer) | A recognizer that allows you to use fuzzy matching to compare strings.  Useful in situations such as when a user make a spelling mistake etc. When the recognizer is used a list of matches, along with confidence scores, are returned. | [![NuGet version](https://img.shields.io/badge/NuGet-blue.svg)](https://www.nuget.org/packages/Bot.Builder.Community.Recognizers.FuzzyRecognizer/) |

### Storage

The following Storage implementations are currently available;

| Name | Description | NuGet |
| ------ | ------ | ------ |
| [Elasticsearch storage](libraries/Bot.Builder.Community.Storage.Elasticsearch) | Elasticsearch based storage for bots created using Microsoft Bot Builder SDK. | [![NuGet version](https://img.shields.io/badge/NuGet-blue.svg)](https://www.nuget.org/packages/Bot.Builder.Community.Storage.Elasticsearch/) |
| [EntityFramework storage](libraries/Bot.Builder.Community.Storage.EntityFramework) | EntityFramework based storage and transcript store for bots created using Microsoft Bot Builder SDK. | [![NuGet version](https://img.shields.io/badge/NuGet-blue.svg)](https://www.nuget.org/packages/Bot.Builder.Community.Storage.EntityFramework/) |

## Contributing and Reporting Issues

We welcome and encourage contributions to this project, in the form of bug fixes, enhancements or new extensions. Please fork the repo and raise a PR if you have something you would like us to review for inclusion.  If you want to discuss an idea first then the best way to do this right now is to raise a GitHub issue or reach out to one of us on Twitter.
