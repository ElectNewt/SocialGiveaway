using Microsoft.Extensions.DependencyInjection;
using Netmentor.DiContainer;
using SocialGiveaway.External.Twitter.Credentials;
using SocialGiveaway.ServiceDependencies;
using System;

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
    public static class ServiceDependenciesDI
    {
        /// <summary>
        /// Todo: move this to the twitter project.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddTwitterHttpContext(this IServiceCollection services)
        {
            services.AddHttpClient(TwitterSettings.HttpFactoryName, client =>
            {
                IServiceProvider serviceProvider = services.BuildServiceProvider();
                TwitterConfiguration configuration = serviceProvider.GetRequiredService<TwitterConfiguration>();
                client.BaseAddress = new Uri(configuration.Settings.ApiUrl);
            });
            return services;
        }
    }
}
