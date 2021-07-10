using Netmentor.DiContainer;
using SocialGiveaway.ServiceDependencies;

namespace SocialGiveaway.API
{
    public class ApiDI
    {
        public static readonly DiModule DiModule = BuildDependencyInjection();
        private static DiModule BuildDependencyInjection()
        {
            var module = new DiModule(typeof(ApiDI).Assembly);
            return module
               .ApplyModule(ServicesDependenciesDI.DiModule);
        }


    }
}
