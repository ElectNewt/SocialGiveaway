using Netmentor.DiContainer;
using SocialGiveaway.Services.Twitter;
using SocialGiveaway.Services.YouTube;

namespace SocialGiveaway.Services
{
    public class ServicesDI
    {

        public static readonly DiModule DiModule = BuildDependencyInjection();
        private static DiModule BuildDependencyInjection()
        {
            var module = new DiModule(typeof(ServicesDI).Assembly);
            return module
               .AddScoped<SelectTwitterWinner>()
               .AddScoped<ITwitterCommentSubRuleValidation, TwitterCommentSubRuleValidation>()
               .AddScoped<ITwitterFollowSubRuleValidation, TwitterFollowSubRuleValidation>()
               .AddScoped<SelectYouTubeWinner>();
        }

    }
}
