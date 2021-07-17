using ROP;
using SocialGiveaway.External.Twitter.Functionalities;
using SocialGiveaway.Services.Twitter;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialGiveaway.ServiceDependencies.Twitter
{
    public class SelectWinnerServiceDependencies : ISelectWinnerDependencies
    {
        private readonly Tweets _tweets;
        public SelectWinnerServiceDependencies(Tweets tweets)
        {
            _tweets = tweets;
        }

        public async Task<Result<List<long>>> GetUserIdWhoLikedATweet(long tweetId)
        {
            return await _tweets.GetUsersWhoLiked(tweetId);
        }

        public async Task<Result<List<long>>> GetUserIdWhoRetweetedATweet(long tweetId)
        {
            return await _tweets.GetUsersWhoRetweeted(tweetId);
        }

        public async Task<Result<List<long>>> GetResponsesOfATweet(long tweetId)
        {
            return await _tweets.GetUsersWhoCommented(tweetId);
        }

        public async Task<Result<List<long>>> GetFollowersOfTweeterAccount(long twitterAccount)
        {
            return await _tweets.GetAllFollowers(twitterAccount); 
        }

        public async Task<Result<long>> GetTwitterAccountFromTweetId(long tweetId)
        {
            return await _tweets.GetAccountId(tweetId);
        }

        public int GetRandomNumber(int start, int end)
        {
            Random random = new Random();
            return random.Next(start, end);
        }

        public async Task<Result<string>> GetUsername(long twitterAccountId)
        {
            return await _tweets.GetUsername(twitterAccountId);
        }
    }
}
