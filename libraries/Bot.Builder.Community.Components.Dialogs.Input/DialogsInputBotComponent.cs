using AdaptiveExpressions.Converters;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs.Declarative;
using Microsoft.Bot.Builder.Dialogs.Declarative.Converters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bot.Builder.Community.Components.Dialogs.Input
{
    public class DialogsInputBotComponent : BotComponent
    {
        public override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<DeclarativeType>(new DeclarativeType<EmailInput>(EmailInput.Kind));
            services.AddSingleton<DeclarativeType>(new DeclarativeType<PhoneNumberInput>(PhoneNumberInput.Kind));
            services.AddSingleton<DeclarativeType>(new DeclarativeType<SocialMediaInput>(SocialMediaInput.Kind));
            services.AddSingleton<DeclarativeType>(
                new DeclarativeType<MultiSelectChoiceInput>(MultiSelectChoiceInput.Kind));

            services.AddSingleton<JsonConverterFactory, JsonConverterFactory<ObjectExpressionConverter<object>>>();
        }
    }
}
