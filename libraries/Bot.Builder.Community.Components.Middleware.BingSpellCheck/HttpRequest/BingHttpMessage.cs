using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Bot.Builder.Community.Components.Middleware.BingSpellCheck.HttpRequest
{
    public class BingHttpMessage : IBingHttpMessage
    {
        private static string _subscriptionKey;
        private static readonly string BaseUri = "https://api.bing.microsoft.com/v7.0/spellcheck";

        private readonly string _textMode = "?text=";
        private string MarketCode { get; set; }

        public bool IsSuccess { get; private set; }

        public readonly string Mode = "&mode=proof";  

        public BingHttpMessage(IConfiguration configuration)
        {
            InitiateBingHttpMessage(configuration["Key"], configuration["MarketCode"]);
        }

        public BingHttpMessage(string bingKey, string marketCode)
        {
            InitiateBingHttpMessage(bingKey, marketCode);
        }
        private void InitiateBingHttpMessage(string bingKey, string marketcode)
        {
            _subscriptionKey = bingKey;

            if (string.IsNullOrEmpty(_subscriptionKey))
                throw new ArgumentException(nameof(_subscriptionKey));

            if (string.IsNullOrEmpty(marketcode))
                marketcode = "en-US";

            MarketCode = "&mkt=" + marketcode;
        }

        private async Task<HttpResponseMessage> MakeRequestAsync(string queryString)
        {
            queryString = _textMode + Uri.EscapeDataString(queryString);
            queryString += Mode + MarketCode;

            var client = new HttpClient();

            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _subscriptionKey);

            return (await client.GetAsync(BaseUri + queryString));
        }

        public async Task<Dictionary<string, object>> SpellCheck(string text)
        {
            var response = await MakeRequestAsync(text);

            var contentString = await response.Content.ReadAsStringAsync();

            IsSuccess = response.IsSuccessStatusCode;

            return JsonConvert.DeserializeObject<Dictionary<string, object>>(contentString);            
        }
    }
}
