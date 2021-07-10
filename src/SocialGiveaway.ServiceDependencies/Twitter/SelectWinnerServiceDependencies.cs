using ROP;
using SocialGiveaway.Services.Twitter;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialGiveaway.ServiceDependencies.Twitter
{
    public class SelectWinnerServiceDependencies : ISelectWinnerDependencies
    {
        public Task<Result<List<int>>> GetUserIdWhoLikedATweet(int tweetId)
        {
            throw new NotImplementedException();
        }

        public Task<Result<List<int>>> GetUserIdWhoRetweetedATweet(int tweetId)
        {
            throw new NotImplementedException();
        }

        public Task<Result<List<int>>> GetResponsesOfATweet(int tweetId)
        {
            throw new NotImplementedException();
        }

        public Task<Result<List<int>>> GetFollowersOfTweeterAccount(int twitterAccount)
        {
            throw new NotImplementedException();
        }

        public Task<Result<int>> GetTwitterAccountFromTweetId(int tweetId)
        {
            throw new NotImplementedException();
        }

        public int GetRandomNumber(int start, int end)
        {
            throw new NotImplementedException();
        }
    }
}
