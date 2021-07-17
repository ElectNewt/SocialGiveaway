using System;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Models;

namespace SocialGiveaway.External.Twitter.Credentials
{

    public interface ITwitterClientFactory
    {
        Task<TwitterClient> GetTwitterClient();
    }

    public class TwitterClientFactory : ITwitterClientFactory
    {
        //TODO: improve how the factory works,
        //Get the token automatically (currently on appsettings.json)
        //if the token still valid it should not create a new one.

        private readonly TwitterCredentials _credentials;
        private TwitterClient? TwitterClient { get; set; }

        public TwitterClientFactory(TwitterConfiguration config)
        {
            _credentials = config.Credentials;
        }

        public Task<TwitterClient> GetTwitterClient()
        {
            if (TwitterClient == null)
            {
                //documentation to get credentials https://linvi.github.io/tweetinvi/dist/credentials/credentials.html
                //there is a bug to get the bearer token, it is getting  “Unable to verify your credentials.”
                //TODO: investigate the issue, not worht wasting time now.
                //a solution could be getting the token manually.
                var appCredentials = new ConsumerOnlyCredentials(_credentials.ConsumerKey, _credentials.ConsumerSecret)
                {
                    BearerToken = _credentials.Token
                };
                TwitterClient = new TwitterClient(appCredentials);

            }
            return Task.FromResult(TwitterClient ?? throw new Exception("twitterClient cannot be null"));

        }
    }
}
