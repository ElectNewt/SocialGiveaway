using System.Collections.Generic;
using System.Threading.Tasks;
using ROP;
using SocialGiveaway.External.Twitter.Functionalities;
using SocialGiveaway.Services.Twitter;

namespace SocialGiveaway.ServiceDependencies.Twitter
{
    public class TwitterFollowSubRuleValidationDependencies : ITwitterFollowSubRuleValidationDependencies
    {
        private readonly TweetFunctionalities _tweets;

        public TwitterFollowSubRuleValidationDependencies(TweetFunctionalities tweets)
        {
            _tweets = tweets;
        }
        
        public async Task<Result<List<long>>> GetFollowersOfTweeterAccount(long twitterAccount)
            => await _tweets.Followers.GetAllFollowers(twitterAccount);

        public async Task<Result<long>> GetTwitterAccountFromTweetId(long tweetId)
            => await _tweets.Account.GetAccountId(tweetId);

        public async Task<Result<long>> GetAccountIdByAccountName(string accountName)
            => await _tweets.Account.GetAccountId(accountName);


    }
}