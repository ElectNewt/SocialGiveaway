using ROP;
using SocialGiveaway.External.Twitter.Credentials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi.Iterators;
using Tweetinvi;

namespace SocialGiveaway.External.Twitter.Functionalities
{
    public class TwitterRetweets
    {
        private readonly ITwitterClientFactory _twitterClientFactory;

        public TwitterRetweets(ITwitterClientFactory twitterClientFactory)
        {
            _twitterClientFactory = twitterClientFactory;
        }

        public async Task<Result<List<long>>> GetUsersWhoRetweeted(long tweetId)
        {
            List<long> users = new List<long>();
            TwitterClient client = await _twitterClientFactory.GetTwitterClient();
            ITwitterIterator<long> iterator = client.Tweets.GetRetweeterIdsIterator(tweetId);
            while (!iterator.Completed)
            {
                ITwitterIteratorPage<long, string> page = await iterator.NextPageAsync();
                users.AddRange(page);
            }
            return users;
        }
    }
}
