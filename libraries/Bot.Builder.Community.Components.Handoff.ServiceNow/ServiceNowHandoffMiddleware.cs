using System;
using System.Globalization;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using Bot.Builder.Community.Components.Handoff.ServiceNow.Models;
using Bot.Builder.Community.Components.Handoff.Shared;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Connector.Authentication;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Bot.Builder.Community.Components.Handoff.ServiceNow
{
    public class ServiceNowHandoffMiddleware : HandoffMiddleware
    {
        private readonly ConversationHandoffRecordMap _conversationHandoffRecordMap;
        private readonly IServiceNowCredentialsProvider _creds;
        private readonly BotFrameworkAuthentication _botFrameworkAuth;

        public ServiceNowHandoffMiddleware(ConversationHandoffRecordMap conversationHandoffRecordMap, IServiceNowCredentialsProvider creds, BotFrameworkAuthentication botFrameworkAuth) : base(conversationHandoffRecordMap)
        {
            _conversationHandoffRecordMap = conversationHandoffRecordMap;
            _creds = creds;
            _botFrameworkAuth = botFrameworkAuth;
        }

        public override async Task RouteActivityToExistingHandoff(ITurnContext turnContext, HandoffRecord handoffRecord)
        {
            var serviceNowHandoffRecord = handoffRecord as ServiceNowHandoffRecord;

            // Retrieve an oAuth token for ServiceNow which we'll pass on this turn
            var claimsIdentity = (ClaimsIdentity)turnContext.TurnState.Get<IIdentity>(BotFrameworkAdapter.BotIdentityKey);
            var userTokenClient = await _botFrameworkAuth.CreateUserTokenClientAsync(claimsIdentity, default(CancellationToken));
            var tokenResponse = await userTokenClient.GetUserTokenAsync(turnContext.Activity.From.Id, _creds.ServiceNowAuthConnectionName, null, null, default(CancellationToken));

            if (tokenResponse != null)
            {
                if (serviceNowHandoffRecord != null)
                {
                    var messageText = turnContext.Activity.Text;

                    // If the incoming activity has a value, it may be a response from an Adaptive Card
                    // which we need to process accordingly.
                    if (turnContext.Activity.Value != null)
                    {
                        try
                        {
                            var activityValue = JObject.Parse(turnContext.Activity.Value.ToString());

                            if (activityValue.ContainsKey("dateVal") && activityValue.ContainsKey("timeVal"))
                            {
                                var dateTimeStr = $"{activityValue["dateVal"]} {activityValue["timeVal"]}";
                                if (DateTime.TryParse(dateTimeStr, out DateTime dateTime))
                                {
                                    var baseDate = new DateTime(1970, 1, 1);
                                    var diff = dateTime - baseDate;
                                    messageText = diff.TotalMilliseconds.ToString(CultureInfo.InvariantCulture);
                                }
                            }
                        }
                        catch(JsonReaderException ex)
                        {
                            // Value is not valid Json so continue
                        }
                    }

                    var message = ServiceNowConnector.MakeServiceNowMessage(0,
                        serviceNowHandoffRecord.RemoteConversationId,
                        messageText,
                        serviceNowHandoffRecord.ConversationRecord.Timezone,                       
                        serviceNowHandoffRecord.ConversationRecord.UserId,
                        serviceNowHandoffRecord.ConversationRecord.EmailId);

                    await ServiceNowConnector.SendMessageToConversationAsync(
                        serviceNowHandoffRecord.ConversationRecord.ServiceNowTenant,
                        tokenResponse.Token,
                        message).ConfigureAwait(false);

                    var traceActivity = Activity.CreateTraceActivity("ServiceNowVirtualAgent", label: "ServiceNowHandoff->Activity forwarded to ServiceNow");
                    await turnContext.SendActivityAsync(traceActivity);
                }
            }
            else
            {
                var traceActivity = Activity.CreateTraceActivity("ServiceNowVirtualAgent", label: "ServiceNowHandoff->No ServiceNow authentication token available.");
                await turnContext.SendActivityAsync(traceActivity);

                throw new Exception("No ServiceNow authentication token available for this user");
            }
        }
        public override async Task<HandoffRecord> Escalate(ITurnContext turnContext, IEventActivity handoffEvent)
        {
            var serviceNowTenant = _creds.ServiceNowTenant;
            var serviceNowAuthConnectionName = _creds.ServiceNowAuthConnectionName;

            var conversationRecord = await ServiceNowConnector.EscalateToAgentAsync(turnContext, handoffEvent, serviceNowTenant, serviceNowAuthConnectionName, _conversationHandoffRecordMap);

            // Forward the activating activity onto ServiceNow
            var handoffRecord = new ServiceNowHandoffRecord(turnContext.Activity.GetConversationReference(), conversationRecord);
            await RouteActivityToExistingHandoff(turnContext, handoffRecord);

            var traceActivity = Activity.CreateTraceActivity("ServiceNowVirtualAgent", label: "ServiceNowHandoff->Handoff initiated");
            await turnContext.SendActivityAsync(traceActivity);

            return handoffRecord;
        }

    }
}
