using System.Threading;
using System.Threading.Tasks;
using AdaptiveExpressions.Properties;
using Bot.Builder.Community.Components.Middleware.BingSpellCheck.HttpRequest;
using Bot.Builder.Community.Components.Middleware.BingSpellCheck.SpellChecker;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Memory;
using Newtonsoft.Json;

namespace Bot.Builder.Community.Components.BingSpellCheckRequest
{
    public class BingSpellCheckRequest : Dialog
    {
        [JsonProperty("$Kind")]
        public const string Kind = "BotBuilderCommunity.BingSpellCheckRequest";

        [JsonProperty("BingKey")]
        public StringExpression BingKey { get; set; }

        [JsonProperty("MarketCode")]
        public StringExpression MarketCode { get; set; }

        [JsonProperty("Value")]
        public StringExpression Value { get; set; }

        [JsonProperty("resultProperty")]
        public StringExpression ResultProperty { get; set; }

        [JsonProperty("ErrorInfo")]
        public StringExpression ErrorInfo { get; set; }

        private IBingSpellCheck _spellCheck;

        private IBingHttpMessage _bingHttpMessage;

        public BingSpellCheckRequest(string path="",int lineNumber=0) : base()
        {
            this.RegisterSourceLocation(path,lineNumber);
        }

        public override async Task<DialogTurnResult> BeginDialogAsync(DialogContext dc, object options = null,
            CancellationToken cancellationToken = new CancellationToken())
        {
            var text = GetRequestText(dc.State);
            if (string.IsNullOrEmpty(text))
            {
                if (ErrorInfo != null) 
                    dc.State.SetValue(ErrorInfo.GetValue(dc.State), "Request text is missing");

                return await dc.EndDialogAsync(null, cancellationToken).ConfigureAwait(false);
            }

            var bingKey = GetBingKey(dc.State);

            if (string.IsNullOrEmpty(bingKey))
            {
                if (ErrorInfo != null)
                    dc.State.SetValue(ErrorInfo.GetValue(dc.State), "Bing subscription key is missing");

                return await dc.EndDialogAsync(null, cancellationToken).ConfigureAwait(false);
            }
            
            var marketcode = GetMarketCode(dc.State);

            _bingHttpMessage = new BingHttpMessage(bingKey, marketcode);
            _spellCheck = new BingSpellCheck(_bingHttpMessage);

            var response = await _spellCheck.Sentence(text);

            switch (_bingHttpMessage.IsSuccess)
            {
                case true when ResultProperty != null:
                    dc.State.SetValue(ResultProperty.GetValue(dc.State), response);
                    break;
                case false when ErrorInfo != null:
                    dc.State.SetValue(ErrorInfo.GetValue(dc.State), response);
                    break;
            }
        
            return await dc.EndDialogAsync(null, cancellationToken).ConfigureAwait(false);
        }

        private string GetRequestText(DialogStateManager stateManager)
        {
            if (Value == null)
            {
                return string.Empty;
            }

            var text = Value.TryGetValue(stateManager);

            return string.IsNullOrEmpty(text.Value) ? string.Empty : text.Value;
        }

        private string GetBingKey(DialogStateManager stateManager)
        {
            if (BingKey == null)
            {
                return string.Empty;
            }

            var bingKey = BingKey.TryGetValue(stateManager);

            return string.IsNullOrEmpty(bingKey.Value) ? string.Empty : bingKey.Value;
        }

        private string GetMarketCode(DialogStateManager stateManager)
        {
            if (MarketCode == null)
            {
                return "en-US";
            }

            var marketValue = MarketCode.TryGetValue(stateManager);

            return string.IsNullOrEmpty(marketValue.Value) ? "en-US" : marketValue.Value;
        }
    }
}
