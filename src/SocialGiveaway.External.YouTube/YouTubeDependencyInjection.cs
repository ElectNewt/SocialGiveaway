using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Netmentor.DiContainer;
using SocialGiveaway.External.Twitter.Credentials;
using SocialGiveaway.External.Twitter.Functionalities;
using System;

namespace SocialGiveaway.External.Twitter
{

    public class YouTubeDependencyInjection
    {
        public static readonly DiModule DiModule = BuildDependencyInjection();
        private static DiModule BuildDependencyInjection()
        {
            var module = new DiModule(typeof(YouTubeDependencyInjection).Assembly);
            return module
               .AddSingleton(provider => BuildYouTubeConfiguration(provider))
               .AddScoped<IYouTubeClientFactory, YouTubeClientFactory>()
               .AddScoped<Youtube>();


        }

        private static YouTubeConfiguration BuildYouTubeConfiguration(IServiceProvider serviceProvider)
        {
            IConfiguration configuration = serviceProvider.GetRequiredService<IConfiguration>();
            IConfigurationSection section = configuration.GetSection("YouTube");
            var credentials = new YouTubeCredentials(section["Credentials:ApiKey"]);
            var settings = new YouTubeSettings(section["settings:ApplicationName"]);

            return new YouTubeConfiguration(credentials, settings);
        }
    }
}
