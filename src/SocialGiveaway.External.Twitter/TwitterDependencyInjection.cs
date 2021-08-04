using Microsoft.Extensions.Configuration;
using Netmentor.DiContainer;
using SocialGiveaway.External.Twitter.Credentials;
using Microsoft.Extensions.DependencyInjection;
using System;
using SocialGiveaway.External.Twitter.Functionalities;

namespace SocialGiveaway.External.Twitter
{
    public class TwitterDependencyInjection
    {
        public static readonly DiModule DiModule = BuildDependencyInjection();
        private static DiModule BuildDependencyInjection()
        {
            var module = new DiModule(typeof(TwitterDependencyInjection).Assembly);
            return module
               .AddSingleton(provider => BuildTwitterConfiguration(provider))
               .AddSingleton<ITwitterClientFactory, TwitterClientFactory>()
               .AddScoped<TweetFunctionalities>()
               .AddScoped<TwitterLikes>()
               .AddScoped<TwitterRetweets>()
               .AddScoped<TwitterComments>()
               .AddScoped<TwitterFollowers>()
               .AddScoped<TwitterAccount>();
           
            
        }

        private static TwitterConfiguration BuildTwitterConfiguration(IServiceProvider serviceProvider)
        {
            IConfiguration configuration  = serviceProvider.GetRequiredService<IConfiguration>();
            IConfigurationSection section = configuration.GetSection("Twitter");
            var credentials = new TwitterModificableCredentials(section["Credentials:consumerKey"], section["Credentials:consumerSecret"]);
            var settings = new TwitterSettings(section["settings:ApiUrl"]);

            return new TwitterConfiguration(credentials, settings);
        }
    }
}
