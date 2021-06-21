using System.Collections.Generic;
using AdaptiveCards;
using Bot.Builder.Community.Components.Dialogs.Input.MultiSelectChoice.Setting;
using Microsoft.Bot.Builder.Dialogs.Adaptive.Input;

namespace Bot.Builder.Community.Components.Dialogs.Input.MultiSelectChoice.ControlStyle
{
    
    public interface IAdaptiveControl
    {
        List<AdaptiveColumnSet> PrepareControl(Orientation orientation,ChoiceSet choices);
        List<ResultSet> ResultSet(object validateText);
    }

}