
# Bing Spell Check Request for Bot Framework Composer

## Build status
| Branch | Status | Recommended NuGet package version |
| ------ | ------ | ------ |
| master | [![Build status](https://ci.appveyor.com/api/projects/status/b9123gl3kih8x9cb?svg=true)](https://ci.appveyor.com/project/garypretty/botbuilder-community) | [Available via NuGet](https://www.nuget.org/packages/Bot.Builder.Community.Components.BingSpellCheckRequest/) |

## Description

This is part of the [Bot Builder Community](https://github.com/botbuildercommunity) project which contains open source Bot Framework Composer components, along with other extensions, including middleware, recognizers and other components for use with the Bot Builder .NET SDK v4.

This component will spell check text using Bing spell check services on request based and therefore requires a key.

### Composer component installation

1. Navigate to the Bot Framework Composer **Package Manager**.
2. Change the filter to **Community packages**.
3. Search for 'BingSpellRequest' and install **Bot.Builder.Community.Components.BingSpellCheckRequest**

![image](https://user-images.githubusercontent.com/16264167/122235026-a528fe00-cebd-11eb-974a-bba3e02fd64d.png)



#### Add the Compoent <BR>

![image](https://user-images.githubusercontent.com/16264167/121934535-b05a1d80-cd47-11eb-861f-bac87993daff.png)

<BR>

#### Configure settings
  
| Property | Example Value | Description  |
| ---- | ----------- | ----------- |
| Key | 123A12312 | a subscription key for the Bing Spell Check |
|MarketCode | en-US |Bing returns content for only these markets | 
|value | hw ae yu | Input for Bing Spell check | 
|Result | user.result (how are you) | Store the result to the property| 
|Error  | user.error ( key is missing) | Incase any error has occurred store into the property | 

![image](https://user-images.githubusercontent.com/16264167/121934572-be0fa300-cd47-11eb-808c-3816a5d18aaf.png)
 
 
 
#### Sample output
   
 ![output2](https://user-images.githubusercontent.com/16264167/122202104-fe346a00-ce9c-11eb-84c7-a521c33bd1a1.png)
   
    
