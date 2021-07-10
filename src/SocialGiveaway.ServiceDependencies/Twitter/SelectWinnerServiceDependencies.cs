using ROP;
using SocialGiveaway.Services.Twitter;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialGiveaway.ServiceDependencies.Twitter
{
    public class SelectWinnerServiceDependencies : ISelectWinnerDependencies
    {
        public Task<Result<List<int>>> GetFollowersOfTweeterAccount(int twitterAccount)
        {
            throw new NotImplementedException();
        }

        public Task<List<int>> GetUserIdWhoLikedATweet(int id)
        {
            throw new NotImplementedException();
        }

        public int GetRandomNumber(int start, int end)
        {
            throw new NotImplementedException();
        }

        public Task<List<int>> GetResponsesOfATweet(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<int>> GetUserIdWhoRetweetedATweet(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Result<int>> GetTwitterAccountFromTweetId(int id)
        {
            throw new NotImplementedException();
        }
    }
}
