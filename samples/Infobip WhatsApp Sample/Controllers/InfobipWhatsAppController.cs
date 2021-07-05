using Bot.Builder.Community.Adapters.Infobip.Core;
using Bot.Builder.Community.Adapters.Infobip.WhatsApp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Infobip_WhatsApp_Sample.Controllers
{
    // This ASP Controller is created to handle a request. Dependency Injection will provide the Adapter and IBot
    // implementation at runtime. Multiple different IBot implementations running at different endpoints can be
    // achieved by specifying a more specific type for the bot constructor argument.
    [Route("api/infobip/whatsapp")]
    [ApiController]
    public class InfobipWhatsAppController: ControllerBase
    {
        private readonly InfobipWhatsAppAdapter Adapter;
        private readonly IBot Bot;

        public InfobipWhatsAppController(InfobipWhatsAppAdapter adapter, IBot bot)
        {
            Adapter = adapter;
            Bot = bot;
        }

        [HttpPost]
        public async Task PostAsync()
        {
            // Delegate the processing of the HTTP POST to the adapter.
            // The adapter will invoke the bot.
            await Adapter.ProcessAsync(Request, Response, Bot);
        }

        [HttpGet]
        public async Task GetAsync()
        {
            var activity = new Activity
            {
                Type = ActivityTypes.Message,
                Id = "message id",
                Timestamp = DateTimeOffset.Now,
                ChannelId = InfobipWhatsAppConstants.ChannelName,
                Conversation = new ConversationAccount { Id = "conversation-id" },
                From = new ChannelAccount { Id = "from-person" },
                Recipient = new ChannelAccount { Id = "for-recipient" }
            };

            activity.Text = "Some message";
            activity.AddInfobipCallbackData(new Dictionary<string, string>{{"a", "b"}});
            var turnContext = new TurnContext(Adapter, activity);
            await Adapter.SendActivitiesAsync(turnContext, new[] { activity }, CancellationToken.None);
        }
    }
}
