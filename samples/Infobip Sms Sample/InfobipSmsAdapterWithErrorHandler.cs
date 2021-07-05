using Bot.Builder.Community.Adapters.Infobip.Sms;
using Microsoft.Extensions.Logging;

namespace Infobip_Sms_Sample
{
    public class InfobipSmsAdapterWithErrorHandler: InfobipSmsAdapter
    {
        public InfobipSmsAdapterWithErrorHandler(InfobipSmsAdapterOptions infobipSmsOptions, IInfobipSmsClient infobipClient, ILogger<InfobipSmsAdapterWithErrorHandler> logger)
            : base(infobipSmsOptions, infobipClient, logger)
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
