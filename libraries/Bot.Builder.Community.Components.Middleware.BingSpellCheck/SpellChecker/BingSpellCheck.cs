using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bot.Builder.Community.Components.Middleware.BingSpellCheck.HttpRequest;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace Bot.Builder.Community.Components.Middleware.BingSpellCheck.SpellChecker
{
    public class BingSpellCheck : IBingSpellCheck
    {
        public bool IsEnable { get; private set; }

        public bool IsOverwrite { get; private set; }

        public string SuccessProperty { get; private set; }

        public bool IsSuccess { get; private set; }

        public string ErrorProperty { get; private set; }

        private IBingHttpMessage _bingHttpMessage = null;

        public BingSpellCheck(IConfiguration configuration, IBingHttpMessage httpMessage)
        {
           InitiateBingSpellCheck(httpMessage, Convert.ToBoolean(configuration["IsEnabled"]),
                Convert.ToBoolean(configuration["IsOverwrite"]));
        }

        public BingSpellCheck(IBingHttpMessage httpMessage)
        {
            if (httpMessage == null)
                throw new ArgumentException(nameof(httpMessage));

            InitiateBingSpellCheck(httpMessage,true,false);
        }

        private void InitiateBingSpellCheck(IBingHttpMessage httpMessage,bool enableSpellCheck,bool canOverwrite)
        {
            SuccessProperty = "turn.SpellCheck";
            ErrorProperty = "turn.SpellErrorInfo";

            IsEnable = enableSpellCheck;
            IsOverwrite = canOverwrite;

            _bingHttpMessage = httpMessage;
        }

        public async Task<string> Sentence(string text)
        {
            var responseText = string.Empty;

            var response = await _bingHttpMessage.SpellCheck(text);

            IsSuccess = _bingHttpMessage.IsSuccess;

            if (response != null)
            {
                responseText = IsSuccess ? PrepareSpellCorrection(text, response) : 
                                           PrepareErrorInformation(response);
            }

            return responseText;
        }

        private string PrepareSpellCorrection(string spellCheckString,Dictionary<string, object> response)
        {
            var updateText = spellCheckString;
            var adjustOffset = 0;
            var isRemoveTokenCase = false;

            var tokens = response["flaggedTokens"] as JToken;

            if (tokens == null) return string.Empty;

            foreach (JToken token in tokens)
            {
                int tokenLength = ((string) token["token"]).Length;

                // Repeat token case
                var word = Convert.ToString(token["suggestions"][0]["suggestion"]);

                if (word == string.Empty)
                {
                    isRemoveTokenCase = true;
                    adjustOffset--;
                    tokenLength++;
                }

                var startIdx = (int) token["offset"] + adjustOffset;

                updateText = updateText.Remove(startIdx, tokenLength);

                if (!isRemoveTokenCase)
                {
                    updateText = updateText.Insert(startIdx, word);
                }
                else
                {
                    isRemoveTokenCase = false;
                    adjustOffset++;
                }

                adjustOffset += word.Length - tokenLength;
            }

            return updateText;
        }

        private string PrepareErrorInformation(Dictionary<string, object> response)
        {
            string errorInformation = string.Empty;

            if (response.TryGetValue("error", out var value))
            {
                errorInformation = PrepareErrorInformation(value as JToken);
            }
            else if (response.TryGetValue("errors", out _))
            {
                foreach (JToken error in response["errors"] as JToken)
                {
                    errorInformation += PrepareErrorInformation(error);
                }               
            }

            return errorInformation;
        }

        private string PrepareErrorInformation(JToken error)
        {
            string errorInfo = " Code: " + error["code"];
            errorInfo += ",Message: " + error["message"];

            string value;
            if ((value = (string)error["parameter"]) != null)
            {
                errorInfo += ",Parameter: " + value;
            }

            if ((value = (string)error["value"]) != null)
            {
                errorInfo += ",Value: " + value;
            }

            return errorInfo;
        }

    }
}
