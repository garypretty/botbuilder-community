using System.Threading;
using System.Threading.Tasks;
using Bot.Builder.Community.Components.Middleware.BingSpellCheck.SpellChecker;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;

namespace Bot.Builder.Community.Components.Middleware.BingSpellCheck
{
    public class SpellCheckComponentMiddleware : IMiddleware
    {
        private readonly IBingSpellCheck _spellCheck;
        public SpellCheckComponentMiddleware(IBingSpellCheck spellCheck)
        {
            _spellCheck = spellCheck;
        }

        public async Task OnTurnAsync(ITurnContext turnContext, NextDelegate next,
            CancellationToken cancellationToken = new CancellationToken())
        {
            if (_spellCheck.IsEnable && turnContext.Activity.Type is ActivityTypes.Message)
            {
                var response = await _spellCheck.Sentence(turnContext.Activity.Text);

                if (_spellCheck.IsOverwrite && _spellCheck.IsSuccess)
                {
                    turnContext.Activity.Text = response;
                }

                var property = _spellCheck.IsSuccess ? _spellCheck.SuccessProperty : _spellCheck.ErrorProperty;

                ObjectPath.SetPathValue(turnContext.TurnState, property, response);
            }

            await next(cancellationToken);
        }
    }
}
