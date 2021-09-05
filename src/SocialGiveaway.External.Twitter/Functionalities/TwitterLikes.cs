using ROP;
using SocialGiveaway.External.Twitter.Credentials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace SocialGiveaway.External.Twitter.Functionalities
{
    public class TwitterLikes
    {

        private readonly TwitterConfiguration _twitterConfiguration;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ITwitterClientFactory _twitterClientFactory;

        public TwitterLikes(TwitterConfiguration twitterConfiguration, IHttpClientFactory httpClientFactory, ITwitterClientFactory twitterClientFactory)
        {
            _twitterConfiguration = twitterConfiguration;
            _httpClientFactory = httpClientFactory;
            _twitterClientFactory = twitterClientFactory;
        }



        //TODO: #3 Update Twitter Like Rule to get more than 100 likes.
        public async Task<Result<List<long>>> GetUsersWhoLiked(long tweetId)
        {
            await _twitterClientFactory.GetTwitterClient();

            var client = _httpClientFactory.CreateClient(TwitterSettings.HttpFactoryName);
            client.DefaultRequestHeaders.Authorization
                        = new AuthenticationHeaderValue("Bearer", _twitterConfiguration.Credentials.Token);
            Rootobject? response = await client.GetFromJsonAsync<Rootobject>($"2/tweets/{tweetId}/liking_users");

            if (response != null)
                return response.data.Select(a => a.AsLong()).ToList();

            return new List<long>();
        }
    }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public class Rootobject
    {
        public Data[] data { get; set; }
    }

    public class Data
    {
        public string id { get; set; }
        public long AsLong()
        {
            return Convert.ToInt64(id);
        }
    }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

}
