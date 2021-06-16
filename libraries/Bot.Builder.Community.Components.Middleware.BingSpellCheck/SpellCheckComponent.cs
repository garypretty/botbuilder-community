using System;
using Bot.Builder.Community.Components.Middleware.BingSpellCheck.HttpRequest;
using Bot.Builder.Community.Components.Middleware.BingSpellCheck.SpellChecker;
using Microsoft.Bot.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bot.Builder.Community.Components.Middleware.BingSpellCheck
{
    public class SpellCheckComponent : BotComponent
    {
        public override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            services.AddSingleton<IMiddleware, SpellCheckComponentMiddleware>();

            services.AddSingleton<IBingSpellCheck>(sp => new SpellChecker.BingSpellCheck(configuration,new BingHttpMessage(configuration)));
        }
    }
}
