// Generated with Bot Builder V4 SDK Template for Visual Studio EchoBot v4.10.3

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bot.Builder.Community.Adapters.Infobip.Core;
using Bot.Builder.Community.Adapters.Infobip.Core.Models;
using Bot.Builder.Community.Adapters.Infobip.WhatsApp;
using Bot.Builder.Community.Adapters.Infobip.WhatsApp.Models;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;

namespace Infobip_WhatsApp_Sample.Bots
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
            //Receive message
            if (turnContext.Activity.ChannelId == InfobipWhatsAppConstants.ChannelName)
                await HandleWhatsAppIncomingMessages(turnContext);

            //Send message
            var activity = new Activity
            {
                Type = ActivityTypes.Message,
                Id = "message id",
                Timestamp = DateTimeOffset.Now,
                Conversation = new ConversationAccount { Id = "conversation-id" },
                Recipient = new ChannelAccount { Id = "some-recipient" },
                Entities = new List<Entity>(),
                Attachments = new List<Attachment>()
            };


            activity.ChannelId = InfobipWhatsAppConstants.ChannelName;
            CreateWhatsAppMessage(activity);


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
                    var seenReport = activity.ChannelData as InfobipWhatsAppIncomingResult;
                    break;
                default:
                    return;
            }

            _logger.Log(LogLevel.Information, $"Event received. Name: {turnContext.Activity.Name}. Value: {turnContext.Activity.ChannelData}");
        }

        private static void CreateWhatsAppMessage(Activity activity)
        {
            var messageType = InfobipIncomingMessageTypes.Location;

            switch (messageType)
            {
                case InfobipIncomingMessageTypes.Text:
                    var text = "Some text with *bold*, _italic_, ~strike through~ and ```code``` formatting";
                    activity.Text = text;
                    break;
                case InfobipIncomingMessageTypes.Location:
                    activity.Entities.Add(new GeoCoordinates
                    {
                        Latitude = 12.3456789,
                        Longitude = 23.456789,
                        Name = "caption"
                    });
                    //or
                    activity.Entities.Add(new Place
                    {
                        Address = "Address",
                        Geo = new GeoCoordinates { Latitude = 12.3456789, Longitude = 23.456789, Name = "caption" }
                    });
                    break;
                case InfobipIncomingMessageTypes.Image:
                    //Same code is similar for other media types. 
                    var mediaMessage = new Attachment
                    {
                        ContentType = "image/png",
                        ContentUrl = "https://cdn-www.infobip.com/wp-content/uploads/2019/09/05100710/rebranding-blog-banner-v3.png"
                    };
                    activity.Attachments.Add(mediaMessage);
                    break;
                case "TEMPLATE":
                    var templateMessage = new InfobipWhatsAppTemplateMessage
                    {
                        TemplateNamespace = "",
                        TemplateData = new[] { "one", "two" },
                        TemplateName = "template_name",
                        Language = "en",
                        MediaTemplateData = new InfobipWhatsAppMediaTemplateData
                        {
                            MediaTemplateHeader = new InfobipWhatsAppMediaTemplateHeader
                            {
                                DocumentFilename = "Test file name"
                            },
                            MediaTemplateBody = new InfobipWhatsAppMediaTemplateBody
                            {
                                Placeholders = new[] { "three", "four" }
                            }
                        }
                    };
                    activity.AddInfobipWhatsAppTemplateMessage(templateMessage);
                    break;
            }
        }

        private async Task HandleWhatsAppIncomingMessages(ITurnContext<IMessageActivity> turnContext)
        {
            //MEDIA
            string mediaType;
            var attachment = turnContext.Activity.Attachments.FirstOrDefault();
            if (attachment != null)
            {
                if (attachment.ContentType.Contains("image"))
                    mediaType = "IMAGE";
                else if (attachment.ContentType.Contains("audio"))
                    mediaType = "AUDIO";
                else if (attachment.ContentType.Contains("video"))
                    mediaType = "VIDEO";
                else if (attachment.ContentType.Contains("application"))
                    mediaType = "DOCUMENT";

                //DOWNLOAD MEDIA
                await attachment.DownloadContent();
                var content = attachment.Content;
            }
           

            //LOCATION
            var locationMessage = turnContext.Activity.Entities.FirstOrDefault()?.GetAs<GeoCoordinates>();

            //TEXT
            var textMessage = turnContext.Activity.Text;
        }
    }
}
