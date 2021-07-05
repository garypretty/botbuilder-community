// Generated with Bot Builder V4 SDK Template for Visual Studio EchoBot v4.11.1

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bot.Builder.Community.Adapters.Infobip.Core;
using Bot.Builder.Community.Adapters.Infobip.Core.Models;
using Bot.Builder.Community.Adapters.Infobip.Viber;
using Bot.Builder.Community.Adapters.Infobip.Viber.Models;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;

namespace Infobip_Viber_Sample.Bots
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
            if (turnContext.Activity.ChannelId == InfobipViberConstants.ChannelName)
                HandleViberIncomingMessages(turnContext);

            //Send message
            var activity = new Activity
            {
                Type = ActivityTypes.Message,
                Id = "message id",
                Timestamp = DateTimeOffset.Now,
                Conversation = new ConversationAccount { Id = "some conversation id" },
                Recipient = new ChannelAccount { Id = "some recipient" },
                Entities = new List<Entity>(),
                Attachments = new List<Attachment>()
            };

            activity.ChannelId = InfobipViberConstants.ChannelName;

            activity.AddInfobipViberMessage(new InfobipOmniViberMessage
            {
                Text = "Test text",
                ImageUrl = "https://p.bigstockphoto.com/GeFvQkBbSLaMdpKXF1Zv_bigstock-Aerial-View-Of-Blue-Lakes-And--227291596.jpg",
                ButtonText = "Button text",
                ButtonUrl = "https://www.infobip.com/docs/api#programmable-communications/omni-failover/send-omni-failover-message"
            });



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
                    var deliveryReport = activity.ChannelData as InfobipIncomingResultBase;
                    //CALLBACK DATA
                    var callbackData = activity.Entities
                        .First(x => x.Type == InfobipEntityType.CallbackData);
                    var sentData = callbackData.GetAs<Dictionary<string, string>>();
                    break;
                case InfobipReportTypes.SEEN:
                    var seenReport = activity.ChannelData;
                    break;
                default:
                    return;
            }

            _logger.Log(LogLevel.Information, $"Event received. Name: {turnContext.Activity.Name}. Value: {turnContext.Activity.ChannelData}");
        }

        private void HandleViberIncomingMessages(ITurnContext<IMessageActivity> turnContext)
        {
            var messageText = turnContext.Activity.Text;
            _logger.Log(LogLevel.Information, $"Received Viber TEXT message with text: {messageText} ");
        }
    }
}
