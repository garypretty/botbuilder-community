using System.Threading.Tasks;

namespace Bot.Builder.Community.Components.Middleware.BingSpellCheck.SpellChecker
{
    public interface IBingSpellCheck
    {
        Task<string> Sentence(string text);

        bool IsEnable { get; }

        bool IsOverwrite { get; }

        string SuccessProperty { get; }

        bool IsSuccess { get; }

        string ErrorProperty { get; }
    }
}
