using AdaptiveCards;

namespace Bot.Builder.Community.Components.Dialogs.Input.MultiSelectChoice.Header
{
    public class AdaptiveHeader : IAdaptiveHeader
    {
        public AdaptiveElement PrepareControl(string text)
        {
            var textBlock = new AdaptiveTextBlock(text)
            {
                Size = AdaptiveTextSize.Default,
                Weight = AdaptiveTextWeight.Bolder
            };

            return textBlock;
        }
    }

}