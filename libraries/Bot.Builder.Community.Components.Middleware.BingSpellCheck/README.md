## Bing Spell Check Middleware 
 
### Build status
| Branch | Status | Recommended NuGet package version |
| ------ | ------ | ------ |
| master | [![Build status](https://ci.appveyor.com/api/projects/status/b9123gl3kih8x9cb?svg=true)](https://ci.appveyor.com/project/garypretty/botbuilder-community) | [![NuGet version](https://img.shields.io/badge/NuGet-1.0.61-blue.svg)](https://www.nuget.org/packages/Bot.Builder.Community.Middleware.BingSpellCheck/) |
<BR>


### Description

This is part of the [Bot Builder Community](https://github.com/garypretty/botbuilder-community) project which contains Bot Framework Components and other projects / packages for use with Bot Framework Composer and the Bot Builder .NET SDK v4.

This middleware will spell check inbound text using ***Bing spell check*** services and therefore requires a key. Once you've configured the middleware component and enabled then this component will run on every message received (ActivityType is Message).


### Composer component installation

1. Navigate to the Bot Framework Composer **Package Manager**.
2. Change the filter to **Community packages**.
3. Search for 'BingSpellCheck' and install **Bot.Builder.Community.Components.Middleware.BingSpellCheck**

![image](https://user-images.githubusercontent.com/16264167/121935397-aedd2500-cd48-11eb-8f78-b6f72433a119.png)



### Middleware settings

```json
"components": [
{
    "name": "Bot.Builder.Community.Components.Middleware.BingSpellCheck",
    "settingsPrefix": "Bot.Builder.Community.Components.Middleware.BingSpellCheck"
}
```

### Bing Spell check Settings

| Property | Value | Description  |
| ---- | ----------- | ----------- |
|IsEnabled | true | Enable Bing Spell check Middleware|
|IsOverwrite | true | Replaces inbound text automatically |
|MarketCode | ex : en-US |Bing returns content for only these markets | 
| Key | ex : 123A12312 | a subscription key for the Bing Spell Check |

<BR>
 
```json
 "Bot.Builder.Community.Components.Middleware.BingSpellCheck": {
    "IsEnabled": true,
    "Key": "Bing Key",
    "MarketCode": "en-US",
    "IsOverwrite": false
  }
```

### Getting the Bing Spell check result from Middleware

You can now get the results using below settings;

Get the Result <BR>
`${turn.SpellCheck}`

Get the Error information <BR>
`${turn.SpellErrorInfo}`


#### Sample output
 
 
 ![output1](https://user-images.githubusercontent.com/16264167/122166868-7473a500-ce7a-11eb-9596-eaee6d2ae119.png)


