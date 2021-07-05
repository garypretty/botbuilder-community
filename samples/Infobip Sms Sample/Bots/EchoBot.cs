// Generated with Bot Builder V4 SDK Template for Visual Studio EchoBot v4.10.3

using Bot.Builder.Community.Adapters.Infobip.Core;
using Bot.Builder.Community.Adapters.Infobip.Core.Models;
using Bot.Builder.Community.Adapters.Infobip.Sms;
using Bot.Builder.Community.Adapters.Infobip.Sms.Models;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Infobip_Sms_Sample.Bots
{
    public class EchoBot : ActivityHandler
    {
        private readonly ILogger<EchoBot> _logger;

        public EchoBot(ILogger<EchoBot> logger)
        {
            _logger = logger;
        }

        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext,
            CancellationToken cancellationToken)
        {
            if (turnContext.Activity.ChannelId == InfobipSmsConstants.ChannelName)
                HandleSmsIncomingMessages(turnContext);

            //Send message
            var activity = new Activity
            {
                Type = ActivityTypes.Message,
                Id = "message id",
                Timestamp = DateTimeOffset.Now,
                Conversation = new ConversationAccount { Id = "subscriber-number" },
                Recipient = new ChannelAccount { Id = "subscriber-number" },
                Entities = new List<Entity>(),
                Attachments = new List<Attachment>()
            };

            activity.ChannelId = InfobipSmsConstants.ChannelName;
            CreateSmsMessage(activity);
 
            

            var shouldIncludeCallbackData = true;
            if (shouldIncludeCallbackData)
            {
                var callbackData = new Dictionary<string, string> { { "first", "second" } };
                activity.AddInfobipCallbackData(callbackData);
            }

            await turnContext.SendActivityAsync(activity, cancellationToken);

        }

        protected override async Task OnEventActivityAsync(ITurnContext<IEventActivity> turnContext, CancellationToken cancellationToken)
        {
            var activity = turnContext.Activity;

            //RECEIVE

            switch (turnContext.Activity.Name)
            {
                case InfobipReportTypes.DELIVERY:
                    _logger.Log(LogLevel.Information, $"Received DLR for message sent via {activity.ChannelId} channel");
                    var deliveryReport = activity.ChannelData as InfobipSmsIncomingResult;
                    //CALLBACK DATA
                    var callbackData = activity.Entities
                        .FirstOrDefault(x => x.Type == InfobipEntityType.CallbackData);
                    if (callbackData != null)
                    {
                        var sentData = callbackData.GetAs<Dictionary<string, string>>();
                        //DO SOME WORK
                    }
                        
                    break;
                default:
                    return;
            }

            _logger.Log(LogLevel.Information, $"Event received. Name: {turnContext.Activity.Name}. Value: {turnContext.Activity.ChannelData}");
        }

        private static void CreateSmsMessage(Activity activity)
        {
            var shouldAddSmsOptions = true;

            if (shouldAddSmsOptions)
            {
                activity.Text = "Text with options";
                var smsOptions = new InfobipOmniSmsMessageOptions
                {
                    ValidityPeriodTimeUnit = InfobipSmsOptions.ValidityPeriodTimeUnitTypes.Hours,
                    ValidityPeriod = 10,
                    Transliteration = InfobipSmsOptions.TransliterationTypes.All,
                    Language = new InfobipOmniSmsLanguage { LanguageCode = InfobipSmsOptions.LanguageCode.None }
                };
                activity.AddInfobipOmniSmsMessageOptions(smsOptions);
            }
            else
            {
                activity.Text = "Text without options";
            }
        }

        private void HandleSmsIncomingMessages(ITurnContext<IMessageActivity> turnContext)
        {
            var messageText = turnContext.Activity.Text;
            _logger.Log(LogLevel.Information, $"Received SMS message with text: {messageText} ");
        }
    }
}
