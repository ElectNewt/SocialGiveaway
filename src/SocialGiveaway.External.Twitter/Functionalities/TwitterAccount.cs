using ROP;
using SocialGiveaway.External.Twitter.Credentials;
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

        public async Task<Result<(string name, string at)>> GetUsername(long accountId)
        {
            TwitterClient client = await _twitterClientFactory.GetTwitterClient();
            IUser user = await client.Users.GetUserAsync(accountId);
            return (user.Name, user.ScreenName);
        }


        public async Task<Result<long>> GetAccountId(long tweetId)
        {
            TwitterClient client = await _twitterClientFactory.GetTwitterClient();
            var tweet = await client.TweetsV2.GetTweetAsync(tweetId);
            return long.Parse(tweet.Tweet.AuthorId);
        }
        
        public async Task<Result<long>> GetAccountId(string userName)
        {
            TwitterClient client = await _twitterClientFactory.GetTwitterClient();
            IUser user = await client.Users.GetUserAsync(userName);
            return user.Id;
        }
    }
}
