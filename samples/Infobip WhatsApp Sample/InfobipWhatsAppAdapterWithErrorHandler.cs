using Bot.Builder.Community.Adapters.Infobip.WhatsApp;
using Microsoft.Extensions.Logging;

namespace Infobip_WhatsApp_Sample
{
    public class InfobipWhatsAppAdapterWithErrorHandler: InfobipWhatsAppAdapter
    {
        public InfobipWhatsAppAdapterWithErrorHandler(InfobipWhatsAppAdapterOptions infobipWhatsAppOptions, IInfobipWhatsAppClient infobipClient, ILogger<InfobipWhatsAppAdapterWithErrorHandler> logger)
            : base(infobipWhatsAppOptions, infobipClient, logger)
        {
            OnTurnError = async (turnContext, exception) =>
            {
                OnTurnError = async (turnContext, exception) =>
                {
                    // Log any leaked exception from the application.
                    logger.LogError($"Exception caught : {exception.Message}");

                    // Send a catch-all apology to the user.
                    await turnContext.SendActivityAsync("Sorry, it looks like something went wrong.");
                };
            };
        }
    }
}
