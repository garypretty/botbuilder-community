using System.Collections.Generic;
using AdaptiveCards;

namespace Bot.Builder.Community.Components.Dialogs.Input.MultiSelectChoice.Action
{
    public interface IAdaptiveAction
    {
        List<AdaptiveAction> PrepareActionList(string captionName);
    }
}