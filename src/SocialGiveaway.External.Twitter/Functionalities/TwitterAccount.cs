using ROP;
using SocialGiveaway.External.Twitter.Credentials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi.Models;
using Tweetinvi;

namespace SocialGiveaway.External.Twitter.Functionalities
{
    public class TwitterAccount
    {
        private readonly ITwitterClientFactory _twitterClientFactory;

        public TwitterAccount(ITwitterClientFactory twitterClientFactory)
        {
            _twitterClientFactory = twitterClientFactory;
        }

        public async Task<Result<(string name, string at)>> GetUsername(long accountid)
        {
            TwitterClient client = await _twitterClientFactory.GetTwitterClient();
            IUser user = await client.Users.GetUserAsync(accountid);
            return (user.Name, user.ScreenName);
        }


        public async Task<Result<long>> GetAccountId(long tweetId)
        {
            TwitterClient client = await _twitterClientFactory.GetTwitterClient();
            var tweet = await client.TweetsV2.GetTweetAsync(tweetId);
            return long.Parse(tweet.Tweet.AuthorId);
        }
    }
}
