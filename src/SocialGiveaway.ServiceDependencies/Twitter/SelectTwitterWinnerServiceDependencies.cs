using ROP;
using SocialGiveaway.External.Twitter.Functionalities;
using SocialGiveaway.Services.Twitter;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialGiveaway.ServiceDependencies.Twitter
{
    public class SelectTwitterWinnerServiceDependencies : ISelectTwitterWinnerDependencies
    {
        private readonly TweetFunctionalities _tweets;
        public SelectTwitterWinnerServiceDependencies(TweetFunctionalities tweets)
        {
            _tweets = tweets;
        }

        public async Task<Result<List<long>>> GetUserIdWhoLikedATweet(long tweetId)
            => await _tweets.Likes.GetUsersWhoLiked(tweetId);


        public async Task<Result<List<long>>> GetUserIdWhoRetweetedATweet(long tweetId)
            => await _tweets.Retweets.GetUsersWhoRetweeted(tweetId);

        public async Task<Result<List<long>>> GetFollowersOfTweeterAccount(long twitterAccount)
            => await _tweets.Followers.GetAllFollowers(twitterAccount);

        public async Task<Result<long>> GetTwitterAccountFromTweetId(long tweetId)
            => await _tweets.Account.GetAccountId(tweetId);

        public int GetRandomNumber(int start, int end)
        {
            Random random = new Random();
            return random.Next(start, end);
        }

        public async Task<Result<(string name, string at)>> GetUsername(long twitterAccountId)
            => await _tweets.Account.GetUsername(twitterAccountId);
    }
}
