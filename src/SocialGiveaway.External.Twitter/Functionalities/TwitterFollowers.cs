using ROP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi.Iterators;
using Tweetinvi;
using SocialGiveaway.External.Twitter.Credentials;

namespace SocialGiveaway.External.Twitter.Functionalities
{
    public class TwitterFollowers
    {

        private readonly ITwitterClientFactory _twitterClientFactory;

        public TwitterFollowers(ITwitterClientFactory twitterClientFactory)
        {
            _twitterClientFactory = twitterClientFactory;
        }

        public async Task<Result<List<long>>> GetAllFollowers(long accountId)
        {
            List<long> users = new List<long>();
            TwitterClient client = await _twitterClientFactory.GetTwitterClient();
            ITwitterIterator<long> iterator = client.Users.GetFollowerIdsIterator(accountId);
            while (!iterator.Completed)
            {
                ITwitterIteratorPage<long, string> page = await iterator.NextPageAsync();
                users.AddRange(page);
            }
            return users;
        }
    }
}
