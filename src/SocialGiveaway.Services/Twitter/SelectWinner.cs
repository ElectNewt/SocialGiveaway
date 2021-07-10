using ROP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SocialGiveaway.Dto.Twitter;

namespace SocialGiveaway.Services.Twitter
{
    public interface ISelectWinnerDependencies
    {
        //https://developer.twitter.com/en/docs/twitter-api/tweets/likes/api-reference/get-tweets-id-liking_users
        Task<Result<List<int>>> GetUserIdWhoLikedATweet(int tweetId);

        Task<Result<List<int>>> GetUserIdWhoRetweetedATweet(int tweetId);

        //https://developer.twitter.com/en/docs/twitter-api/conversation-id
        Task<Result<List<int>>> GetResponsesOfATweet(int tweetId);

        //https://developer.twitter.com/en/docs/twitter-api/v1/accounts-and-users/follow-search-get-users/api-reference/get-followers-ids
        Task<Result<List<int>>> GetFollowersOfTweeterAccount(int twitterAccount);
        Task<Result<int>> GetTwitterAccountFromTweetId(int tweetId);
        int GetRandomNumber(int start, int end);
    }

    public class SelectWinner
    {
        private readonly ISelectWinnerDependencies _dependencies;

        public SelectWinner(ISelectWinnerDependencies dependencies)
        {
            _dependencies = dependencies;
        }

        public async Task<Result<TwitterUser>> Execute(int tweetId, List<TweetTicketDto> tweetTickets)
        {
            List<Result<TwitterTicketResult>> userIdWhoOnTickets = new();
            tweetTickets
                .ForEach(async x => userIdWhoOnTickets.Add(await GetTweetTicketContenders(tweetId, x)));

            return await userIdWhoOnTickets
                .Traverse()
                .Bind(SelectTwitterUsersWithOptions)
                .Async()
                .Bind(GetWinner);
        }

        private Result<List<TwitterUser>> SelectTwitterUsersWithOptions(List<TwitterTicketResult> ticketResults)
        {
            return ticketResults
                .SelectMany(a=>a.Users)
                .ToList();
        }

        private async Task<Result<TwitterTicketResult>> GetTweetTicketContenders(int tweetId, TweetTicketDto tweetTicket)
        {
            List<Result<List<TwitterUser>>> userContenders = new();
            foreach (var rule in tweetTicket.Rules)
            {
                Result<List<TwitterUser>> usersWhoAccomplishTheRule =
                    await TweetAction(rule, tweetId).Map(ToTwitterUser);
                userContenders.Add(usersWhoAccomplishTheRule);
            }

            return userContenders
                .Traverse()
                .Bind(GetUsersWhoFulfillAllRules);

            Task<List<TwitterUser>> ToTwitterUser(List<int> userIds)
            {
                return Task.FromResult(userIds.Distinct()
                    .Select(a => new TwitterUser(a)).ToList());
            }
        }


        private Task<Result<List<int>>> TweetAction(TwitterRule rule, int tweetId) => rule switch
        {
            TwitterRule.Follow => GetFollowers(tweetId),
            TwitterRule.Like => _dependencies.GetUserIdWhoLikedATweet(tweetId),
            TwitterRule.Comment => _dependencies.GetResponsesOfATweet(tweetId),
            TwitterRule.Retweet => _dependencies.GetUserIdWhoRetweetedATweet(tweetId),
            TwitterRule.CommentPlusQuote => throw new NotImplementedException("Not implemented yet"),
            TwitterRule.Hashtag => throw new NotImplementedException("Not implemented yet"),
            _ => throw new NotImplementedException("seems like you're using a rule that is not configured."),
        };

        private Task<Result<TwitterUser>> GetWinner(List<TwitterUser> userIdWhoOnTickets)
        {
            int winner = _dependencies.GetRandomNumber(0, userIdWhoOnTickets.Count - 1);
            return userIdWhoOnTickets[winner].Success().Async();
        }


        private async Task<Result<List<int>>> GetFollowers(int tweetId)
        {
            return await _dependencies.GetTwitterAccountFromTweetId(tweetId)
                .Bind(_dependencies.GetFollowersOfTweeterAccount);
        }

        private Result<TwitterTicketResult> GetUsersWhoFulfillAllRules(List<List<TwitterUser>> rulesResult)
        {
            List<TwitterUser> usersInRule = new();

            foreach (TwitterUser userInRule in rulesResult[0])
            {
                bool success = true;
                foreach (List<TwitterUser> rule in rulesResult.Skip(1))
                {
                    if (rule.All(a => a != userInRule))
                        success = false;
                }

                if (success)
                    usersInRule.Add(userInRule);
            }

            return new TwitterTicketResult(usersInRule);
        }
        
        private record TwitterTicketResult
        {
            public readonly IReadOnlyCollection<TwitterUser> Users;

            public TwitterTicketResult(List<TwitterUser> users)
            {
                Users = users;
            }
        }
    }
}