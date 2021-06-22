using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Bot.Builder.Community.Components.Handoff.ServiceNow.Models;
using Bot.Builder.Community.Components.Handoff.Shared;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Bot.Builder.Community.Components.Handoff.ServiceNow
{
    // Connector class for ServiceNow integration
    public static class ServiceNowConnector
    {
        public static async Task<ServiceNowConversationRecord> EscalateToAgentAsync(ITurnContext turnContext, IEventActivity handoffEvent, string serviceNowTenant, string serviceNowAuthConnectionName, ConversationHandoffRecordMap conversationHandoffRecordMap)
        {
            var context = handoffEvent.Value as JObject;

            // These can be null, it may not be necessary to pass these if ServiceNow can infer from the logger in user (SSO) but shows the concept
            // of how a Bot can pass information as part of the handoff event for use by ServiceNow.
            var timeZone = context?.Value<string>("timeZone");               
            var userId = context?.Value<string>("userId");
            var emailId = context?.Value<string>("emailId");

            return new ServiceNowConversationRecord { ConversationId = turnContext.Activity.Conversation.Id, ServiceNowTenant = serviceNowTenant, ServiceNowAuthConnectionName = serviceNowAuthConnectionName, Timezone = timeZone, UserId = userId, EmailId = emailId};
        }

        public static ServiceNowRequestMessage MakeServiceNowMessage(int id, string conversationId, string text, string timeZone,  string userId, string emailId)
        {
            // https://docs.servicenow.com/bundle/paris-application-development/page/integrate/inbound-rest/concept/bot-api.html

            return new ServiceNowRequestMessage
            {
                action = null, // Indicates to send to VA
                requestId = Guid.NewGuid().ToString(),
                clientSessionId = conversationId,
                botToBot = true,
                message = new ServiceNowRequestMessageContent
                {
                    text = text,
                    typed = true,
                },
                userId = userId,
                emailId = emailId,
                timestamp = DateTimeOffset.Now.ToUnixTimeSeconds(),
                timezone = timeZone,
                
                clientVariables = new ClientVariables
                {
                    conversationId = conversationId
                }
            };
        }
        public static async Task<int> SendMessageToConversationAsync(string serviceNowTenant, string token, ServiceNowRequestMessage message)
        {
            using (var client = new HttpClient())
            {
                var stringPayload = JsonConvert.SerializeObject(message, Formatting.Indented, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });                

                var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");

                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    Content = httpContent
                };
               
                request.Headers.Add("Authorization", $"Bearer {token}");

                request.RequestUri = new Uri($"https://{serviceNowTenant}/api/sn_va_as_service/bot/integration");

                var response = await client.SendAsync(request).ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {                  
                    return 0;
                }
                else
                {
                    throw new Exception($"Failed to send message to conversation. Response code {response.StatusCode}");
                }
            }
        }
        public static string Base64Encode(string textToEncode)
        {
            byte[] textAsBytes = Encoding.UTF8.GetBytes(textToEncode);
            return Convert.ToBase64String(textAsBytes);
        }
    }
}
