using Microsoft.Bot.Builder.Dialogs.Adaptive.Input;

namespace Bot.Builder.Community.Components.Dialogs.Input.MultiSelectChoice.Setting
{
    public class CardSetting
    {
        public string Title;
        public Orientation OrientationType;
        public string ActionName;
        public ChoiceSet ChoiceList;
    }

    public enum Orientation
    {
        Horizontal,
        Vertical
    }
}
