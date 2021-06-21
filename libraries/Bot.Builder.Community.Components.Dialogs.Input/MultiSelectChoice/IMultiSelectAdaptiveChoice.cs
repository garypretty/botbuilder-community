using System.Collections.Generic;
using Bot.Builder.Community.Components.Dialogs.Input.MultiSelectChoice.Setting;
using Microsoft.Bot.Schema;

namespace Bot.Builder.Community.Components.Dialogs.Input.MultiSelectChoice
{
    public interface IMultiSelectAdaptiveChoice
    {
        Attachment CreateAttachment(CardSetting cardSetting);
        List<ResultSet> ResultSet(object validateText);
    }
}
