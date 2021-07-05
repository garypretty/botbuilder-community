using Bot.Builder.Community.Adapters.Infobip.Viber;
using Microsoft.Extensions.Logging;

namespace Infobip_Viber_Sample
{
    public class InfobipViberAdapterWithErrorHandler : InfobipViberAdapter
    {
        public InfobipViberAdapterWithErrorHandler(InfobipViberAdapterOptions infobipViberOptions, IInfobipViberClient viberClient, ILogger<InfobipViberAdapterWithErrorHandler> logger)
            : base(infobipViberOptions, viberClient, logger)
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
