using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using AdaptiveExpressions.Properties;
using Bot.Builder.Community.Components.Dialogs.Input.MultiSelectChoice;
using Bot.Builder.Community.Components.Dialogs.Input.MultiSelectChoice.Setting;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Adaptive.Input;
using Microsoft.Bot.Builder.Dialogs.Adaptive.Templates;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Bot.Builder.Community.Components.Dialogs.Input
{
    public class MultiSelectChoiceInput : InputDialog
    {
        [JsonProperty("$Kind")] 
        public const string Kind = "BotBuilderCommunity.MultiSelectChoiceInput";

        [JsonProperty("choices")] 
        public ObjectExpression<ChoiceSet> Choices { get; set; }

        [JsonProperty("Orientation")]
        public EnumExpression<Orientation> OrientationType { get; set; }

        [JsonProperty("ActionName")]
        public StringExpression ActionName { get; set; }

        [JsonProperty("Result")] 
        public StringExpression Result { get; set; }

        private readonly IMultiSelectAdaptiveChoice _multiSelectAdaptiveHandler;

        [JsonConstructor]
        public MultiSelectChoiceInput([CallerFilePath] string callerPath = "",
            [CallerLineNumber] int callerLine = 0)
        {
            _multiSelectAdaptiveHandler = new MultiSelectAdaptiveChoice();
            RegisterSourceLocation(callerPath, callerLine);
        }

        protected override async Task<IActivity> OnRenderPromptAsync(DialogContext dc, InputState state,
            CancellationToken cancellationToken = new CancellationToken())
        {
            var cardSettings = await PrepareInputSetting(dc, state, cancellationToken);

            if (cardSettings.ChoiceList == null || cardSettings.ChoiceList.Count <= 0)
            {
                throw new ArgumentException("List of Choices should not be empty");
            }

            var attachment = _multiSelectAdaptiveHandler.CreateAttachment(cardSettings);

            if (attachment == null)
                throw new ArgumentException("Render card has failed");

            var activity = new StaticActivityTemplate((Activity)MessageFactory.Attachment(attachment));

            switch (state)
            {
                case InputState.Invalid:
                    InvalidPrompt = activity;
                    break;
                case InputState.Unrecognized:
                    UnrecognizedPrompt = activity;
                    break;
                default:
                    Prompt = activity;
                    break;
            }

            return await base.OnRenderPromptAsync(dc, state, cancellationToken);
        }

        private async Task<CardSetting> PrepareInputSetting(DialogContext dc, InputState state,
            CancellationToken cancellationToken)
        {
            var cardSetting = new CardSetting
            {
                ChoiceList = Choices?.TryGetValue(dc.State).Value
            };

            var actionName = ActionName?.TryGetValue(dc.State).Value;
            if (string.IsNullOrEmpty(actionName))
                actionName = "Submit";

            cardSetting.ActionName = actionName;
            cardSetting.OrientationType = OrientationType.GetValue(dc.State);
            cardSetting.Title = await GetPromptText(dc, state, cancellationToken);

            return cardSetting;
        }

        private async Task<string> GetPromptText(DialogContext dc, InputState state, CancellationToken cancellationToken)
        {
            var promptActivity = (Activity)await base.OnRenderPromptAsync(dc, state, cancellationToken);

            var promptText = promptActivity != null && !string.IsNullOrEmpty(promptActivity.Text)
                ? promptActivity.Text
                : "Prompt Text is missing";

            return promptText;
        }

        protected override Task<InputState> OnRecognizeInputAsync(DialogContext dc, CancellationToken cancellationToken)
        {
            var validateText = dc.State.GetValue<object>(VALUE_PROPERTY);

            if (validateText is JObject)
            {
                var result = _multiSelectAdaptiveHandler.ResultSet(validateText);

                if (result != null && result.Count > 0)
                {
                    dc.State.SetValue(Result.GetValue(dc.State), result);
                    return Task.FromResult(InputState.Valid);
                }
            }
            else if (validateText != null)
            {
                return Task.FromResult(InputState.Unrecognized);
            }
            return Task.FromResult(InputState.Invalid);
        }          
    }
}