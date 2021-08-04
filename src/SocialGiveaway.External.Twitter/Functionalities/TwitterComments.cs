using ROP;
using SocialGiveaway.External.Twitter.Credentials;
using SocialGiveaway.Model.Twitter;
using SocialGiveaway.ServiceDependencies.Twitter.Mappers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tweetinvi;

namespace SocialGiveaway.External.Twitter.Functionalities
{
    public class TwitterComments
    {

        private readonly ITwitterClientFactory _twitterClientFactory;

        public TwitterComments(ITwitterClientFactory twitterClientFactory)
        {
            _twitterClientFactory = twitterClientFactory;
        }

        public async Task<Result<List<TweetInformation>>> GetUsersWhoCommented(long tweetId)
        {
            List<TweetInformation> users = new List<TweetInformation>();
            TwitterClient client = await _twitterClientFactory.GetTwitterClient();
            var tweet = await client.TweetsV2.GetTweetAsync(tweetId);
            string getconversationId = tweet.Tweet.ConversationId;

            var searchIterator = client.SearchV2.GetSearchTweetsV2Iterator($"conversation_id:{getconversationId}");
            while (!searchIterator.Completed)
            {
                var searchPage = await searchIterator.NextPageAsync();
                var searchResponse = searchPage.Content;
                var tweets = searchResponse.Tweets;
                users.AddRange(tweets.Select(a => a.ToTweetInformation()));
            }
            return users;
        }
    }
}
