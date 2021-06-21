## Custom Input for Bot Framework Composer 

### Build status
| Branch | Status | Recommended NuGet package version |
| ------ | ------ | ------ |
| master | [![Build status](https://ci.appveyor.com/api/projects/status/b9123gl3kih8x9cb?svg=true)](https://ci.appveyor.com/project/garypretty/botbuilder-community) | [![NuGet version](https://img.shields.io/badge/NuGet-1.0.39-blue.svg)](https://www.nuget.org/packages/Bot.Builder.Community.Components.Dialogs.Input/) |

### Description
This is part of the [Bot Builder Community](https://github.com/garypretty/botbuilder-community) project which contains Bot Framework Components and other projects / packages for use with Bot Framework Composer and the Bot Builder .NET SDK v4.

This package contains additional Inputs , beyond those offered out of the box by the Bot Framework composer. Internally the Inputs uses the [Microsoft Text Recognizers](https://github.com/Microsoft/Recognizers-Text/tree/master/.NET) and [Multi-Select-choice](#Multi-Select-choice) use the Adaptive card.

[Installation](#Composer-component-installation)

Currently the following Inputs are available

| Input | Description |
| ------ | ------ |
| [Phone Number](#Phone-Number-Input) | Input a user for PhoneNumber. |
| [SocialMedia](#Social-Medial-Input) | Input a user for find mention,Hashtag. |
| [Email](#Email-Input) | Input a user for Email. |
| [Multi-Select-choice](#Multi-Select-choice) | Options to user to select one or more values |
| Number with Unit | coming soon |
| Number with Type | coming soon |
| Guid | coming soon | 
| Internet Protocol | coming soon | 

### Composer component installation

1. Navigate to the Bot Framework Composer **Package Manager**.
2. Change the filter to **Community packages**.
3. Search for '***Input***' and install **Bot.Builder.Community.Components.Dialogs.Input**

![image](https://user-images.githubusercontent.com/16264167/122669120-233a1d00-d1bc-11eb-8c48-91fe4bbb0e27.png)


After install the package custom Input's is avaliable in sub-menu of "Ask a question" menu.<BR>
  
###### Note : Composer is not supported tab view for the 3rd party input types so custom inputs display in the flat view. ######

![image](https://user-images.githubusercontent.com/16264167/122669223-93e13980-d1bc-11eb-96bc-003c34802854.png)


#### Email Input

The EmailInput will extract an email address from a message from the user.

##### Design in composer

![image](https://user-images.githubusercontent.com/16264167/122669432-96905e80-d1bd-11eb-84c3-1ed7c17172f6.png)

###### Sample output

![image](https://user-images.githubusercontent.com/16264167/122669747-14a13500-d1bf-11eb-8e7d-20902a7cee14.png)




#### Phone Number Input

The PhoneNumberInput will extract a phone number from a message from the user;

##### Design in composer

![image](https://user-images.githubusercontent.com/16264167/122669543-32ba6580-d1be-11eb-86a0-13262f937027.png)

###### Sample output

![image](https://user-images.githubusercontent.com/16264167/122669556-406feb00-d1be-11eb-9094-2f8fc434fe3d.png)


#### Social Medial Input

The SocialMediaInput will extract one of the following types based on which MediaType enum value is passed in:

| Properties | Description | Example |
| ------ | ------ | ------ |
| Mention | extract mention tag from string | If a user enters my twitter handle is @VinothRajendran <BR> the resulting answers is @vinothrajendran |
| Hashtag | extract hash tag from string | if a user enters "Trends? Does #WM35 count?" <BR> the resulting answers is "#WM35" |


##### Design in Composer

![image](https://user-images.githubusercontent.com/16264167/122669683-c724c800-d1be-11eb-9696-dc54829df674.png)

###### Sample Output

![image](https://user-images.githubusercontent.com/16264167/122716009-02c89c00-d26a-11eb-81ef-d9244f170651.png)




#### Multi-Select-choice
  
The MultiSelectChoice will give options to the user to select one or more values
  
| Properties | Description |
| ------ | ------ |
| List of choices | Add the choices |
| Display Type | choices display option Horizontal or Vertical |
| Action Name | User 'submit' , name like "OK" , "Submit" , "Process" |

##### Design in Composer

![image](https://user-images.githubusercontent.com/16264167/122669978-ecfe9c80-d1bf-11eb-95dc-66aaa69fe8c4.png)

###### Sample output

![image](https://user-images.githubusercontent.com/16264167/122669987-f7209b00-d1bf-11eb-9c6f-4e72fe8850e5.png)

  
###### Coming soon Inputs ###### 
  
Number with Unit<BR>
Number with Type<BR>
Guid<BR>
Internet Protocol<BR>

