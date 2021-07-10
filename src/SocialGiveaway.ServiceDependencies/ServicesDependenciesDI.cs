using Netmentor.DiContainer;
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
               .AddScoped<ISelectWinnerDependencies, SelectWinnerServiceDependencies>();
        }
    }
}
