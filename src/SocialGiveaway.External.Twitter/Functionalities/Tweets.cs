using ROP;
using SocialGiveaway.External.Twitter.Credentials;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Iterators;
using Tweetinvi.Models;

namespace SocialGiveaway.External.Twitter.Functionalities
{
    public class Tweets
    {
        private readonly ITwitterClientFactory _twitterClientFactory;
        private readonly TwitterLikes _twitterLikes;

        public Tweets(ITwitterClientFactory twitterClientFactory, TwitterLikes likes)
        {
            _twitterClientFactory = twitterClientFactory;
            _twitterLikes = likes;
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

        public async Task<Result<List<long>>> GetUsersWhoLiked(long tweetId)
        {
            return await _twitterLikes.GetUsersWhoLiked(tweetId);
        }
        public async Task<Result<List<long>>> GetUsersWhoCommented(long tweetId)
        {
            List<long> users = new List<long>();
            TwitterClient client = await _twitterClientFactory.GetTwitterClient();
            var tweet = await client.TweetsV2.GetTweetAsync(tweetId);
            string getconversationId = tweet.Tweet.ConversationId;

            var searchIterator = client.SearchV2.GetSearchTweetsV2Iterator($"conversation_id:{getconversationId}");
            while (!searchIterator.Completed)
            {
                var searchPage = await searchIterator.NextPageAsync();
                var searchResponse = searchPage.Content;
                var tweets = searchResponse.Tweets;

                var userids = tweets.Select(a => long.Parse(a.AuthorId)).ToList();
                users.AddRange(userids);
            }
            return users;
        }

        public async Task<Result<string>> GetUsername(long accountid)
        {
            TwitterClient client = await _twitterClientFactory.GetTwitterClient();
            IUser user = await client.Users.GetUserAsync(accountid);
            return user.Name;
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
        public async Task<Result<long>> GetAccountId(long tweetId)
        {
            TwitterClient client = await _twitterClientFactory.GetTwitterClient();
            var tweet = await client.TweetsV2.GetTweetAsync(tweetId);
            return long.Parse(tweet.Tweet.AuthorId);
        }
    }


}
