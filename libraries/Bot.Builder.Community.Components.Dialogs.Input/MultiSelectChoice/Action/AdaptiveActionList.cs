using System.Collections.Generic;
using AdaptiveCards;

namespace Bot.Builder.Community.Components.Dialogs.Input.MultiSelectChoice.Action
{
    public class AdaptiveActionList : IAdaptiveAction
    {
        public List<AdaptiveAction> PrepareActionList(string captionName)
        {
            var adaptiveActions = new List<AdaptiveAction>
            {
                new AdaptiveSubmitAction()
                {
                    Type = "Action.Submit",
                    Title = captionName
                }
            };

            return adaptiveActions;
        }
    }
}