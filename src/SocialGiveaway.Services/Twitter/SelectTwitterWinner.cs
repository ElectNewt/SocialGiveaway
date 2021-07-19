using ROP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SocialGiveaway.Dto.Twitter;

namespace SocialGiveaway.Services.Twitter
{
    public interface ISelectTwitterWinnerDependencies
    {
        //https://developer.twitter.com/en/docs/twitter-api/tweets/likes/api-reference/get-tweets-id-liking_users
        Task<Result<List<long>>> GetUserIdWhoLikedATweet(long tweetId);

        Task<Result<List<long>>> GetUserIdWhoRetweetedATweet(long tweetId);

        //https://developer.twitter.com/en/docs/twitter-api/conversation-id
        Task<Result<List<long>>> GetResponsesOfATweet(long tweetId);

        //https://developer.twitter.com/en/docs/twitter-api/v1/accounts-and-users/follow-search-get-users/api-reference/get-followers-ids
        Task<Result<List<long>>> GetFollowersOfTweeterAccount(long twitterAccount);
        Task<Result<long>> GetTwitterAccountFromTweetId(long tweetId);
        int GetRandomNumber(int start, int end);
        Task<Result<(string name, string at)>> GetUsername(long twitterAccountId);
    }

    public class SelectTwitterWinner
    {
        private readonly ISelectTwitterWinnerDependencies _dependencies;

        public SelectTwitterWinner(ISelectTwitterWinnerDependencies dependencies)
        {
            _dependencies = dependencies;
        }

        public async Task<Result<TwitterUserDto>> Execute(long tweetId, List<TweetTicketDto> tweetTickets)
        {
            List<Result<TwitterTicketResult>> userIdWhoOnTickets = new();
            foreach (TweetTicketDto ticket in tweetTickets)
            {
                Result<TwitterTicketResult> contenders = await GetTweetTicketContenders(tweetId, ticket);
                userIdWhoOnTickets.Add(contenders);
            }

            return await userIdWhoOnTickets
                .Traverse()
                .Bind(SelectTwitterUsersWithOptions)
                .Bind(GetWinner)
                .Async()
                .Bind(GetUsername);
        }

        private Result<List<TwitterUser>> SelectTwitterUsersWithOptions(List<TwitterTicketResult> ticketResults)
        {
            return ticketResults
                .SelectMany(a => a.Users)
                .ToList();
        }

        private async Task<Result<TwitterTicketResult>> GetTweetTicketContenders(long tweetId, TweetTicketDto tweetTicket)
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

            Task<List<TwitterUser>> ToTwitterUser(List<long> userIds)
            {
                return Task.FromResult(userIds.Distinct()
                    .Select(a => new TwitterUser(a)).ToList());
            }
        }


        private Task<Result<List<long>>> TweetAction(TwitterRule rule, long tweetId) => rule switch
        {
            TwitterRule.Follow => GetFollowers(tweetId),
            TwitterRule.Like => _dependencies.GetUserIdWhoLikedATweet(tweetId),
            TwitterRule.Comment => _dependencies.GetResponsesOfATweet(tweetId),
            TwitterRule.Retweet => _dependencies.GetUserIdWhoRetweetedATweet(tweetId),
            TwitterRule.CommentPlusQuote => throw new NotImplementedException("Not implemented yet"),
            TwitterRule.Hashtag => throw new NotImplementedException("Not implemented yet"),
            _ => throw new NotImplementedException("seems like you're using a rule that is not configured."),
        };

        private Result<TwitterUser> GetWinner(List<TwitterUser> userIdWhoOnTickets)
        {
            int winner = _dependencies.GetRandomNumber(0, userIdWhoOnTickets.Count - 1);
            return userIdWhoOnTickets[winner];
        }

        private async Task<Result<TwitterUserDto>> GetUsername(TwitterUser user)
        {
            return await _dependencies.GetUsername(user.Id).Map(ToTwitterUser);

            Task<TwitterUserDto> ToTwitterUser((string name, string at) args)
            {
                return Task.FromResult(new TwitterUserDto(args.name, args.at));
            }
        }


        private async Task<Result<List<long>>> GetFollowers(long tweetId)
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
        private record TwitterUser
        {
            public readonly long Id;

            public TwitterUser(long id)
            {
                Id = id;
            }
        }
    }
}