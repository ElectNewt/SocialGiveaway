using Netmentor.DiContainer;
using SocialGiveaway.External.Twitter;
using SocialGiveaway.ServiceDependencies.Twitter;
using SocialGiveaway.Services;
using SocialGiveaway.Services.Twitter;

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
               .AddScoped<ISelectWinnerDependencies, SelectWinnerServiceDependencies>();
        }
    }
}
