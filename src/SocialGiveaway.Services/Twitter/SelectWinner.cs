using ROP;
using SocialGiveAway.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialGiveaway.Services.Twitter
{
    public interface ISelectWinnerDependencies
    {
        //https://developer.twitter.com/en/docs/twitter-api/tweets/likes/api-reference/get-tweets-id-liking_users
        Task<List<int>> GetUserIdWhoLikedATweet(int id);
        Task<List<int>> GetUserIdWhoRetweetedATweet(int id);

        //https://developer.twitter.com/en/docs/twitter-api/conversation-id
        Task<List<int>> GetResponsesOfATweet(int id);

        //https://developer.twitter.com/en/docs/twitter-api/v1/accounts-and-users/follow-search-get-users/api-reference/get-followers-ids
        Task<Result<List<int>>> GetFollowersOfTweeterAccount(int twitterAccount);
        Task<Result<int>> GetTwitterAccountFromTweetId(int id);

        int GetRandomNumber(int start, int end);

    }

    public class SelectWinner
    {
        private readonly ISelectWinnerDependencies _dependencies;

        public SelectWinner(ISelectWinnerDependencies dependencies)
        {
            _dependencies = dependencies;
        }

        public async Task<Result<int>> Execute(int tweetId, List<TweetTicketDto> tweetTickets)
        {

            List<int> userIDWhoOnTickets = new List<int>();
            foreach (var tweetTicket in tweetTickets)
            {
                List<List<int>> rulesResult = new List<List<int>>();
                foreach (var rule in tweetTicket.Rules)
                {
                    List<int> usersWhoAccomplishTheRule = await TweetAction(rule, tweetId);

                    rulesResult.Add(usersWhoAccomplishTheRule.Distinct().ToList());
                }

                List<int> usersInTicket = GetUsersWithAllRules(rulesResult);
                userIDWhoOnTickets.AddRange(usersInTicket);
            }
            int winner =  _dependencies.GetRandomNumber(0, userIDWhoOnTickets.Count - 1);
            
            return userIDWhoOnTickets[winner];
        }


        private Task<List<int>> TweetAction(TwitterRule rule, int tweetId) => rule switch
        {
            TwitterRule.Follow => GetFollowers(tweetId),
            TwitterRule.Like => _dependencies.GetUserIdWhoLikedATweet(tweetId),
            TwitterRule.Comment => _dependencies.GetResponsesOfATweet(tweetId),
            TwitterRule.Retweet => _dependencies.GetUserIdWhoRetweetedATweet(tweetId),
            TwitterRule.CommentPlusQuote => throw new NotImplementedException(),
            TwitterRule.Hashtag => throw new NotImplementedException(),
            _ => throw new NotImplementedException(),
        };

        private async Task<List<int>> GetFollowers(int tweetId)
        {
            return await _dependencies.GetTwitterAccountFromTweetId(tweetId)
                .Bind(_dependencies.GetFollowersOfTweeterAccount).ThrowAsync();

        }

        //TODO: Improve this code.
        private List<int> GetUsersWithAllRules(List<List<int>> rulesResult)
        {
            List<int> userIdsInAllRules = new List<int>();

            foreach (int userInRule in rulesResult[0])
            {
                bool success = true;
                foreach (List<int> rule in rulesResult.Skip(1))
                {
                    if (!rule.Any(a => a == userInRule))
                        success = false;
                }
                if (success)
                    userIdsInAllRules.Add(userInRule);
            }
            return userIdsInAllRules;
        }



    }
}
