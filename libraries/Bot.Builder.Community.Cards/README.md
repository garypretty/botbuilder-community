# Cards Library for Bot Builder v4 .NET SDK

## Build status
| Branch | Status | Recommended NuGet package version |
| ------ | ------ | ------ |
| master | [![Build status](https://ci.appveyor.com/api/projects/status/b9123gl3kih8x9cb?svg=true)](https://ci.appveyor.com/project/garypretty/botbuilder-community) | [Available via NuGet](https://www.nuget.org/packages/Bot.Builder.Community.Cards/) |

## Description

This is part of the [Bot Builder Community](https://github.com/botbuildercommunity) project which contains Bot Framework Components and other projects / packages for use with Bot Framework Composer and the Bot Builder .NET SDK v4.

The cards library currently has two main features:

1. Both Adaptive Cards and Bot Framework cards can be disabled
2. Adaptive Cards can be translated

More information about the cards library can be found in its original ideation thread: https://github.com/BotBuilderCommunity/botbuilder-community-dotnet/issues/137

## Installation

Available via NuGet package [Bot.Builder.Community.Cards](https://www.nuget.org/packages/Bot.Builder.Community.Cards/).

Install into your project use the following command in the package manager. 

```
    PM> Install-Package Bot.Builder.Community.Cards
```

## Sample

A sample bot showcasing the features of the cards library is available [here](../../samples/Cards%20Library%20Sample).

## Usage

### Disabling and deleting

The cards library can automatically decide whether to disable or delete a card based on the capabilities of the channel, much like how `ChoiceFactory` automatically decides how to display choices in the Bot Builder SDK. If a channel supports message updates then the cards library will try to delete the card because that's assumed to be the preferred user experience, whereas if the channel does not support message updates then the card will simply be disabled using bot state which works on every channel.

The classes used for disabling and deleting cards are in the `Bot.Builder.Community.Cards.Management` namespace, because the functionality makes use of a *card manager*. The card manager uses *card manager state* to track ID's and save activities.

Tracking ID's is how the cards library effectively disables cards even when the channel does not support message updates. There are two *tracking styles*: `TrackEnabled` and `TrackDisabled`. When the tracking style is `TrackEnabled` then the cards library will treat any ID tracked in bot state as enabled and any ID not tracked in bot state as disabled. Likewise, when the tracking style is `TrackDisabled` then the cards library will treat any ID tracked in bot state as disabled and any ID not tracked in bot state as enabled.

In order for ID-tracking to work, each ID needs to be present in a card's *action data*. The cards library defines action data as an object found both in an outgoing card attachment sent from the bot to a channel and in an incoming activity sent from the channel to the bot when a user interacts with the card. In an Adaptive Card, action data is what's in a submit action's `data` property. In a Bot Framework card action you can usually expect action data to be in the `value` property and you can likewise usually expect action data to be in the `value` property of an incoming activity. We have to say "usually" because some channels behave a little differently but the cards library is able to automatically adapt to the behavior of those channels.

The cards library can automatically put ID's in action data and it calls these ID's *action data ID's* or just *data ID's* for short. A data ID found in an incoming activity can be compared to the ID's tracked in card manager state to see whether the ID is enabled or not. If the ID is disabled then the bot can ignore the incoming activity, making it look as though nothing happened when the card was clicked. That is the "disabled" effect the cards library provides.

There are four kinds of data ID's to account for the different *data ID scopes*. The scopes refer to how much is getting disabled or deleted.

1. An *action ID* will only disable or delete one action, whether it's a Bot Framework card action or an Adaptive Card submit action. This means the ID is scoped to an action, i.e. it has the action scope.
2. A *card ID* will disable or delete an entire card which may contain multiple actions. This means the ID is scoped to a card, i.e. it has the card scope.
3. A *carousel ID* will disable or delete an entire carousel which may contain multiple cards. This means the ID is scoped to a carousel, i.e. it has the carousel scope.
4. A *batch ID* will disable or delete an entire batch of activities which may contain multiple carousels (i.e. multiple activities). This means the ID is scoped to a batch, i.e. it has the batch scope.

Note that the cards library is using the word "carousel" to refer to all of the card attachments contained in a single activity even if the channel doesn't support carousels or if the activity is not using the carousel attachment layout, because the term "activity ID" was already taken. Also, the word "batch" is a term borrowed from Bot Builder v3 to just mean a collection of activities. These activities can be grouped together because they were all sent by the same call to `SendActivitiesAsync` or they can have some arbitrary grouping defined by the developer.

#### Setting data ID's

You can use the static "set" methods of the `DataId` class to insert data ID's into various objects. For example, using `DataId.SetInBatch` will put data ID's in every action in every card in every activity in a batch:

```c#
DataId.SetInBatch(activities);
```

By default, the set methods will only insert an action ID. If you want to use a different data ID scope, you can pass a `DataIdOptions` object to the method. The following code will put a card ID in every action in every card in an activity:

```c#
DataId.SetInActivity(activity, new DataIdOptions(DataIdScopes.Card));
```

The cards library generates random GUID-based ID's for you, and it makes sure different ID's are generated for different objects according to their scopes. For example, if you have two cards in a carousel and each card has two actions and you want to use the carousel, card, and action scopes, all four actions will have different action ID's in their action data but they'll all have the same carousel ID, and two card ID's will be generated so that different actions in the same card will have the same card ID but different actions in different cards will have different card ID's. Your code might look like this:

```c#
DataId.SetInCarousel(attachments, new DataIdOptions(new List<string>
{
    DataIdScopes.Action,
    DataIdScopes.Card,
    DataIdScopes.Carousel,
}));
```

And you might end up with the following four objects in the action data of your four actions (notice which ID's are the same and which are different):

```json
{
    "action": "action-f5becd5f-60ed-40af-8567-5c6404ea92ee",
    "card": "card-3f0818f0-0c6c-4b62-9736-39c024576bae",
    "carousel": "carousel-6f4223be-c370-480c-a7b0-fdcfa33878a2"
}
```

```json
{
    "action": "action-a8e13e43-a0d4-4a82-a64c-2962a2c26842",
    "card": "card-3f0818f0-0c6c-4b62-9736-39c024576bae",
    "carousel": "carousel-6f4223be-c370-480c-a7b0-fdcfa33878a2"
}
```

```json
{
    "action": "action-d28b669f-674a-4756-b1b5-000d8e156409",
    "card": "card-ad6cd62d-2de7-4849-96f6-beabc0beb466",
    "carousel": "carousel-6f4223be-c370-480c-a7b0-fdcfa33878a2"
}
```

```json
{
    "action": "action-cee648cb-2074-4d26-98bd-6b3fc1c59981",
    "card": "card-ad6cd62d-2de7-4849-96f6-beabc0beb466",
    "carousel": "carousel-6f4223be-c370-480c-a7b0-fdcfa33878a2"
}
```

If you want to use a specific ID instead of generating a random ID then you can use the `DataIdOptions.Set` method:

```c#
var options = new DataIdOptions();

options.Set(DataIdScopes.Batch, "My batch ID");
```

The `DataId` class has 16 static set methods including the three you've seen so far:

- `SetInBatch`
- `SetInActivity`
- `SetInCarousel`
- `SetInAttachment`
- `SetInAdaptiveCard`
- `SetInAnimationCard`
- `SetInAudioCard`
- `SetInHeroCard`
- `SetInOAuthCard`
- `SetInReceiptCard`
- `SetInSigninCard`
- `SetInThumbnailCard`
- `SetInVideoCard`
- `SetInSubmitAction`
- `SetInCardAction`
- `SetInActionData`

Three of these methods take a `ref object` as their first argument. This indicates that the method may create a new object with the data ID's in it instead of modifying the existing object. If the existing object was already assigned to a property of another object then you will need to account for this by reassigning the new object to whatever property it was contained in, like this:

```c#
DataId.SetInAdaptiveCard(ref card);

attachment.Content = card;
```

Internally, the cards library uses something it calls the [*card tree*](./mermaid-diagram.md) to facilitate recursion for operations like these. At the top you have batches and each batch has indexed activities and each activity has an `Attachments` property and the `Attachments` property has indexed attachments and each attachment has a `Content` property and so on. The card tree can be entered at any of those "nodes" and it will recurse down into indexes and properties and sub-properties of objects until it reaches its "exit" node.

#### Tracking and forgetting

![Messenger](https://user-images.githubusercontent.com/41968495/124056044-dd5e2f80-d9d9-11eb-8195-6a078439c54b.gif)

Once you've set ID's in your action data, remember that you also need to track them in card manager state in order to disable them. Forgetting ID's is the opposite of tracking ID's, so it refers to removing an ID from card manager state instead of adding it. When it comes to tracking and forgetting, the question of which one enables and which one disables is determined by the tracking style. The `CardManager` class has 5 methods for disabling in this way:

- `EnableIdAsync` - Tracks or forgets depending on tracking style
- `DisableIdAsync` - Tracks or forgets depending on tracking style
- `TrackIdAsync` - Adds an ID to card manager state
- `ForgetIdAsync` - Removes an ID from the tracked ID's in card manager state
- `ClearTrackedIdsAsync` - Forgets all ID's in card manager state

If you want to treat all ID's as disabled by default and you only want to enable specific ID's (such as in the most recently sent card), you should use `TrackingStyle.TrackEnabled`. This means you'd have to enable an ID before it can be used, like this:

```c#
await cardManager.EnableIdAsync(
    turnContext,
    new DataId(DataIdScopes.Action, actionId),
    TrackingStyle.TrackEnabled);
```

Calling `EnableIdAsync` with the `TrackEnabled` tracking style will call `TrackIdAsync` internally, and you can instead call `TrackIdAsync` directly if you like:

```c#
await cardManager.TrackIdAsync(
    turnContext,
    new DataId(DataIdScopes.Action, actionId));
```

You can disable an ID whenever you want, but a common case will be to disable an ID after an action is used so that it can only be used once. When you disable an ID, make sure you use the same tracking style that you use to enable them:

```c#
await cardManager.DisableIdAsync(
    turnContext,
    new DataId(DataIdScopes.Action, actionId),
    TrackingStyle.TrackEnabled);
```

Since calling `DisableIdAsync` with the `TrackEnabled` tracking style will call `TrackIdAsync` internally, you can instead call `ForgetIdAsync` directly if you like:

```c#
await cardManager.ForgetIdAsync(
    turnContext,
    new DataId(DataIdScopes.Action, actionId));
```

In some situations you may want to forget all ID's at once, such as if you want all previously sent cards to be disabled for each new turn. You can easily do this with `ClearTrackedIdsAsync`:

```c#
await cardManager.ClearTrackedIdsAsync(turnContext);
```

#### Saving and deleting

![Slack](https://user-images.githubusercontent.com/41968495/124056116-fc5cc180-d9d9-11eb-92fb-22fe8435f3e7.gif)

If you're using a channel that supports message updates then you can use a card manager to delete actions, cards, carousels, and batches. Because a carousel is the same thing as an activity in this context, if you delete an action or a card without deleting the carousel then the containing activity will have to be updated rather than deleted. For example, if an activity contains three card attachments then updating the activity so that it only contains two of those attachments will effectively delete the third card. In order to update previously sent activities with modifications like that, the activities must be saved in card manager state. The `CardManager` class provides the `SaveActivitiesAsync` method for this purpose:

```c#
await cardManager.SaveActivitiesAsync(turnContext, activities);
```

Just like with tracking and disabling, you will want to make sure your saved activities contain action data ID's. In this case, the ID's will be used to identify which activity an action came from. Once your bot receives the action, you can call `DeleteActionSourceAsync` like this:

```c#
await cardManager.DeleteActionSourceAsync(turnContext, DataIdScopes.Card);
```

It has the phrase "delete action source" in its name instead of "delete card" etc. because it can delete an action or a card or a carousel or a batch depending on the scope you pass to it. The method looks at the incoming activity in the turn context and determines if the activity contains action data and uses the data ID from the scope you specify to update or delete the associated activities accordingly.

Note that "deleting activities" in this context means actually removing them from the channel conversation on the client side. The cards library uses the term *unsave* to mean removing an activity from card manager state. If any activities are deleted when you call `DeleteActionSourceAsync` then they get unsaved automatically, but if you want to unsave an activity manually then you can use `UnsaveActivityAsync`:

```c#
await cardManager.UnsaveActivityAsync(turnContext, activityId);
```

#### Middleware

The cards library provides card manager middleware that does everything discussed so far automatically and more. Just provide it with a card manager and add it to your adapter like any other middleware:

```c#
adapter.Use(new CardManagerMiddleware(cardManager));
```

Card manager middleware automatically determines whether a channel supports message updates or not. If the channel does not support message updates, card manager middleware does the following:

- Sets data ID's in outgoing activities
- Enables and disables data ID's as needed
- Short-circuits the bot logic if an incoming ID is disabled

If the channel does support message updates, card manager middleware does the following:

- Sets data ID's in outgoing activities
- Saves outgoing activities
- Deletes actions, cards, carousels, and batches based on incoming actions

Card manager middleware is very configurable, but out of the box it will disable or delete every action as soon as the action is used. There are a few other miscellaneous features of card manager middleware that can also be used outside of card manager middleware as extension methods. These extension methods are:

- `List<Activity>.SeparateAttachments` - In some channels like Teams, no activity ID is returned when an activity is sent with both text and attachments. This method gets around that problem by automatically separating any such activities into multiple activities so that an activity ID can be retrieved for each of them.
- `IEnumerable<IMessageActivity>.ConvertAdaptiveCards` - This method converts Adaptive Card objects to generic objects in order to get around a longstanding bug that prevents Adaptive Cards from being deserialized in bot state.
- `IEnumerable<IMessageActivity>.AdaptOutgoingCardActions` - This method makes sure the action data in Bot Framework cards is correctly formatted to work on the specific channel being used. Adaptive Cards are standardized so no modifications are needed for them.

Besides these features that can be turned on or off, the two other extension methods used by card manager middleware are:

- `IEnumerable<IMessageActivity>.GetIdsFromBatch` - This method uses the card tree to retrieve all data ID's from all cards in all activities in a batch.
- `ITurnContext.GetIncomingActionData` - This method retrieves the action data from an incoming activity based on the specific channel being used, deserializing strings as needed. This method will return null if no action data is found, so it can be effectively used to determine if an activity came from an action or not.

#### Behaviors

The cards library uses the term *deactivate* to mean "disable or delete." While card manager middleware can be configured to deactivate all actions, cards, carousels, and batches, you may want specific actions to behave differently from how you've configured card manager middleware. You can achieve this using the auto-deactivate *action behavior*. Action behaviors, or just *behaviors* for short, allow specific actions to be treated by card manager manager middleware in a special way. Auto-deactivate is currently the only behavior available in the cards library, though more may be added later. You can set it using the static methods of the `ActionBehavior` class which have the same names as the static methods of the `DataId` class described earlier. For example, you can use the following code to make sure card manager middleware won't automatically deactivate anything when a specific hero card is clicked:

```c#
ActionBehavior.SetInHeroCard(
    card,
    Behaviors.AutoDeactivate,
    BehaviorSwitch.Off);
```

The three possible values for the auto-deactivate behavior are `"on"`, `"off"`, and `"default"`. Using `"default"` will make the action behave as though it doesn't even have the auto-deactivate behavior at all, which means the cards library will just behave the way it's been configured to behave for all actions.

### Translation

![Teams](https://user-images.githubusercontent.com/41968495/124056122-ff57b200-d9d9-11eb-85d2-cafdd89db0fb.gif)

Multilingual bots have been able to use the [translation middleware](https://github.com/microsoft/BotBuilder-Samples/blob/main/samples/csharp_dotnetcore/17.multilingual-bot/Translation/TranslationMiddleware.cs) found in the multilingual bot sample to automatically translate incoming and outgoing activities, but that sample only translates the activities' text properties and ignores their attachments. Translating cards is more complicated because a card contains multiple strings and some of them should be translated and some of them shouldn't. As an example of something that shouldn't be translated, consider the ID string of an input element in an Adaptive Card. If that were translated into another language then the bot might not be able to recognize the input in an incoming activity's action data correctly.

The cards library solves this problem by providing an Adaptive Card translator in the `Bot.Builder.Community.Cards.Translation` namespace. The Adaptive Card translator intelligently detects which strings to translate using a configurable list of property names and in some cases checking the type of the element that property is in. For an example of when it's necessary to check the type of the property's parent element, consider that a "value" property should be translated in an `Input.Text` element but not in an `Input.ChoiceSet` element. Currently, the cards library only translates Adaptive Cards and not Bot Framework cards.

The Adaptive Card translator provides four static overloads and two instance overloads of the `TranslateAsync` method. This allows it to be used in a variety of ways depending on your needs. The instance overloads are for when you want to create an instance of the `AdaptiveCardTranslator` class that contains its own settings so you don't have to pass them as an argument every time you call `TranslateAsync`. The `AdaptiveCardTranslator` class can even be used with dependency injection, and its settings can be loaded from your bot's configuration file (which is probably appsettings.json).

In addition to letting you decide what gets translated, the Adaptive Card translator also allows you to decide how it gets translated. The Adaptive Card translator has built-in compatibility with the Microsoft Translator API, but it also lets you pass in a delegate that translates strings in some other way. If you want to use Microsoft Translator, there are three settings that the Adaptive Cards translator can read from your configuration file: `MicrosoftTranslatorKey`, `MicrosoftTranslatorLocale`, and `MicrosoftTranslatorEndpoint`. So if you're using Microsoft Translator with the Adaptive Cards translator, your bot's configuration file might look like this:

```jsonc
{
  // Bot credentials
  "MicrosoftAppId": "<guid>",
  "MicrosoftAppPassword": "<password>",
  // Microsoft Translator configuration
  "MicrosoftTranslatorKey": "<guid>",
  "MicrosoftTranslatorLocale": "en-us",
  "MicrosoftTranslatorEndpoint": "https://api.cognitive.microsofttranslator.com"
}
```

Just like in the multilingual bot sample, in order to use Microsoft Translator with the Adaptive Cards translator you will need to follow the instructions [here](https://docs.microsoft.com/en-us/azure/cognitive-services/translator/translator-how-to-signup) to get a translator key to use in your bot. The other two Microsoft Translator settings are optional. `MicrosoftTranslatorLocale` is just the default language that you want your translator to use if no language option is provided when `TranslateAsync` is called, and `MicrosoftTranslatorEndpoint` allows you to provide a custom domain in case you want the Adaptive Cards translator to call a translator service that's hosted somewhere else. In the above example configuration, you can see that the default locale is US English and the endpoint is just the ordinary Microsoft Translator endpoint that would be used by default even if no endpoint was provided.

If you don't put any Microsoft Translator settings in your configuration file, you can just pass a `MicrosoftTranslatorConfig` object as an argument to `TranslateAsync`:

```c#
var translatedCard = await AdaptiveCardTranslator.TranslateAsync(
    untranslatedCard,
    new MicrosoftTranslatorConfig("YOUR TRANSLATOR KEY"));
```

Alternatively, you can pass the settings as their own arguments outside of a `MicrosoftTranslatorConfig` object:

```c#
var translatedCard = await AdaptiveCardTranslator.TranslateAsync(
    untranslatedCard,
    "es-es",
    "YOUR TRANSLATOR KEY",
    httpClient);
```

The `httpClient` parameter is how you would provide a custom endpoint if you wanted to, but since it's a whole `HttpClient` object you can configure the client to send its requests however you want instead of just providing the endpoint.

The other two static `TranslateAsync` overloads are for when you want to provide a delegate that does the translation your own way instead of using the default Microsoft Translator functionality. As a contrived example, here's how you might translate all the strings in an Adaptive Card by reversing them (so `["hello", "world"]` becomes `["olleh", "dlrow"]`):

```c#
var translatedCard = await AdaptiveCardTranslator.TranslateAsync(
    untranslatedCard,
    async (input, cancellationToken) => input.Reverse().ToArray().ToString());
```

The delegate passed to that `TranslateAsync` overload takes one string and returns one string, so it just translates each string one at a time. In case you want to translate all the translatable strings at once (such as when you only want to make one API call perhaps), you can use a delegate that takes and returns an `IEnumerable<string>`. The following (again contrived) example replaces all the translatable strings in an Adaptive Card with the first one (so `["hello", "world"]` becomes `["hello", "hello"]`):

```c#
var translatedCard = await AdaptiveCardTranslator.TranslateAsync(
    untranslatedCard,
    async (inputs, cancellationToken) => Enumerable.Repeat(inputs.First(), inputs.Count()));
```

Finally, the two instance overloads of `TranslateAsync` are for when you want to create an instance of the `AdaptiveCardTranslator` class with persistent properties so you don't have to pass Microsoft Translator settings to it every time you call it. This works well with dependency injection. If the language you want to translate the card into is already in your configuration, you can pass only the card and nothing more to `TranslateAsync`:

```c#
var translatedCard = await adaptiveCardTranslator.TranslateAsync(untranslatedCard);
```

If you want to provide a language, you can do that with the other instance overload:

```c#
var translatedCard = await adaptiveCardTranslator.TranslateAsync(
    untranslatedCard,
    turnContext.Activity.Locale);
```

Note that the Adaptive Card translator will always return a newly created object and never modify the card in place. Because the `TranslateAsync` methods are all generic, they are able to return an object of the same type as the one you pass as an argument. This means you can provide the Adaptive Card in any form you like, whether it be as a string or as an `AdaptiveCard` object from the Adaptive Cards library or something else.

If you want to specify which parts of Adaptive Cards get translated, you can use the `PropertiesToTranslate` property of an `AdaptiveCardTranslatorSettings` object. The following example makes it so that only fallback text gets translated:

```c#
var settings = new AdaptiveCardTranslatorSettings
{
    PropertiesToTranslate = new[]
    {
        "fallbackText",
    },
};
```

All four static `TranslateAsync` overloads have an optional `settings` parameter that allows you to pass an `AdaptiveCardTranslatorSettings` object, and if you're using the instance overloads then you can modify the `Settings` property of the `AdaptiveCardTranslator` instance.
