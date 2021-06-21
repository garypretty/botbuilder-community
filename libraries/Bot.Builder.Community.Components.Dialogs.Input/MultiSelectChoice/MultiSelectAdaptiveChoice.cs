using System.Collections.Generic;
using AdaptiveCards;
using Bot.Builder.Community.Components.Dialogs.Input.MultiSelectChoice.Action;
using Bot.Builder.Community.Components.Dialogs.Input.MultiSelectChoice.ControlStyle;
using Bot.Builder.Community.Components.Dialogs.Input.MultiSelectChoice.Header;
using Bot.Builder.Community.Components.Dialogs.Input.MultiSelectChoice.Setting;
using Microsoft.Bot.Schema;
using Newtonsoft.Json.Linq;

namespace Bot.Builder.Community.Components.Dialogs.Input.MultiSelectChoice
{
    public class MultiSelectAdaptiveChoice : IMultiSelectAdaptiveChoice
    {
        private const int MajorVersion = 1;
        private const int MinorVersion = 3;

        private IAdaptiveControl _adaptiveControl;
        private IAdaptiveAction _adaptiveAction;
        private IAdaptiveHeader _adaptiveHeader;

        public MultiSelectAdaptiveChoice()
        {
            InitializeObject();
        }

        public Attachment CreateAttachment(CardSetting cardSetting)
        {
            var adaptiveCard = PrepareChoiceList(cardSetting);

            var attachment = new Attachment
            {
                ContentType = AdaptiveCard.ContentType,
                Content = JObject.FromObject(adaptiveCard)
            };
            return attachment;
        }

        private AdaptiveCard PrepareChoiceList(CardSetting cardSetting)
        {
            var cardObj = new AdaptiveCard(new AdaptiveSchemaVersion(MajorVersion, MinorVersion))
            {
                Body = new List<AdaptiveElement>()
            };

            cardObj.Body.Add(_adaptiveHeader.PrepareControl(cardSetting.Title));

            var adaptiveElement = _adaptiveControl.PrepareControl(cardSetting.OrientationType, cardSetting.ChoiceList);

            if (adaptiveElement?.Count > 0)
            {
                foreach (var item in adaptiveElement)
                {
                    cardObj.Body.Add(item);
                }                
            }

            cardObj.Actions = _adaptiveAction.PrepareActionList(cardSetting.ActionName);

            return cardObj;
        }

        private void InitializeObject()
        {
            _adaptiveAction = new AdaptiveActionList();
            _adaptiveHeader = new AdaptiveHeader();
            _adaptiveControl = new AdaptiveToggleOption();
        }       

        public List<ResultSet> ResultSet(object validateText)
        {
            return _adaptiveControl.ResultSet(validateText);            
        }
    }
}