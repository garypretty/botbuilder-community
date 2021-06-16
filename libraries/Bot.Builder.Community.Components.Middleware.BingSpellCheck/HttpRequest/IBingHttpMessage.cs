using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bot.Builder.Community.Components.Middleware.BingSpellCheck.HttpRequest
{
    public interface IBingHttpMessage
    {
        Task<Dictionary<string, object>> SpellCheck(string text);

        bool IsSuccess { get; }
    }
}
