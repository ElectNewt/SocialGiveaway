using Netmentor.DiContainer;
using SocialGiveaway.External.Twitter;
using SocialGiveaway.ServiceDependencies.Twitter;
using SocialGiveaway.ServiceDependencies.YouTube;
using SocialGiveaway.Services;
using SocialGiveaway.Services.Twitter;
using SocialGiveaway.Services.YouTube;

namespace SocialGiveaway.ServiceDependencies
{
    public class ServicesDependenciesDI
    {
        public static readonly DiModule DiModule = BuildDependencyInjection();
        private static DiModule BuildDependencyInjection()
        {
            var module = new DiModule(typeof(ServicesDependenciesDI).Assembly);
            return module
                .ApplyModule(ServicesDI.DiModule)
                .ApplyModule(TwitterDependencyInjection.DiModule)
                .ApplyModule(YouTubeDependencyInjection.DiModule)
               .AddScoped<ISelectTwitterWinnerDependencies, SelectTwitterWinnerServiceDependencies>()
               .AddScoped<ITwitterCommentSubRuleValidationDependencies, TwitterCommentSubRuleValidationServiceDependencies>()
               .AddScoped<ISelectYouTubeWinnerDependencies, SelectYouTubeWinnerServiceDependencies>();
        }
    }
}
