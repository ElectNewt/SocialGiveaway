using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
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
        //TODO: #2 improve how the factory works,
        //Get the token automatically (currently on appsettings.json)
        //if the token still valid it should not create a new one.

        private readonly TwitterModificableCredentials _credentials;
        private readonly IHttpClientFactory _httpClientFactory;
        private TwitterClient? TwitterClient { get; set; }

        public TwitterClientFactory(TwitterConfiguration config, IHttpClientFactory httpClientFactory)
        {
            _credentials = config.Credentials;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<TwitterClient> GetTwitterClient()
        {
            if (TwitterClient == null)
            {
                await GetBearerToken();
                var appCredentials = new ConsumerOnlyCredentials(_credentials.ConsumerKey, _credentials.ConsumerSecret)
                {
                    BearerToken = _credentials.Token
                };
                TwitterClient = new TwitterClient(appCredentials);

            }
            return TwitterClient ?? throw new Exception("twitterClient cannot be null");
        }


        private async Task GetBearerToken()
        {
            var client = _httpClientFactory.CreateClient(TwitterSettings.HttpFactoryName);
            byte[] credentials = Encoding.ASCII.GetBytes($"{_credentials.ConsumerKey}:{_credentials.ConsumerSecret}");
            AuthenticationHeaderValue credentialsHeader = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(credentials));
            client.DefaultRequestHeaders.Authorization = credentialsHeader;

            HttpResponseMessage response = await client.PostAsync("oauth2/token", new StringContent("grant_type=client_credentials", Encoding.UTF8, "application/x-www-form-urlencoded"));
            string responseValue = await response.Content.ReadAsStringAsync();


            TokenResponse? tokenResponse = JsonSerializer.Deserialize<TokenResponse>(responseValue);

            _credentials.UpdateToken(tokenResponse?.access_token ?? throw new Exception("Problem getting the bearer token"));
        }

        private class TokenResponse
        {
            public string? token_type { get; set; }
            public string? access_token { get; set; }
        }
    }
}
