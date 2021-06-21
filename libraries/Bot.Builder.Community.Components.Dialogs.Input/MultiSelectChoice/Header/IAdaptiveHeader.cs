using AdaptiveCards;

namespace Bot.Builder.Community.Components.Dialogs.Input.MultiSelectChoice.Header
{
    public interface IAdaptiveHeader
    {
        AdaptiveElement PrepareControl(string text);
    }
}