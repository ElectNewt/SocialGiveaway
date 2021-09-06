using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ROP;
using SocialGiveaway.Dto.Twitter;
using SocialGiveaway.Shared.Extensions;

namespace SocialGiveaway.Services.Twitter
{
    public interface ITwitterFollowSubRuleValidationDependencies
    {
        Task<Result<long>> GetAccountIdByAccountName(string accountName);
        Task<Result<List<long>>> GetFollowersOfTweeterAccount(long twitterAccount);
        Task<Result<long>> GetTwitterAccountFromTweetId(long tweetId);
    }

    public interface ITwitterFollowSubRuleValidation
    {
        Task<Result<List<long>>> Execute(TwitterRuleDto rule, long tweetId);
    }
    
    public class TwitterFollowSubRuleValidation : ITwitterFollowSubRuleValidation
    {
        private readonly ITwitterFollowSubRuleValidationDependencies _dependencies;

        public TwitterFollowSubRuleValidation(ITwitterFollowSubRuleValidationDependencies dependencies)
        {
            _dependencies = dependencies;
        }

        public async Task<Result<List<long>>> Execute(TwitterRuleDto rule, long tweetId)
            => await GetWhoToFollow(rule, tweetId)
                .Bind(GetAllFollowersForAccounts)
                .Bind(GetFollowersWhoFollowAllTheAccounts);

        private async Task<Result<List<long>>> GetWhoToFollow(TwitterRuleDto rule, long tweetId)
        {
            var followConditions = (rule.Conditions ?? new List<TwitterConditionDto>())
                .Where(a => a.SubRule == TwitterSubRule.Follow).ToList();
            if (followConditions.Any())
            {
                List<Result<long>> accountsToFollow = new List<Result<long>>();
                
                foreach (var condition in followConditions)
                {
                    if (string.IsNullOrWhiteSpace(condition.Condition))
                        throw new ArgumentException(
                            "you should specify a twitter account when specifying the follow subRule");

                    Result<long> accountId = await _dependencies.GetAccountIdByAccountName(condition.Condition);
                    accountsToFollow.Add(accountId);
                }

                return accountsToFollow.Traverse();
            }
            else
            {
                return await _dependencies.GetTwitterAccountFromTweetId(tweetId)
                    .Map(accountId => new List<long>() {accountId});
            }
        }

        private async Task<Result<List<List<long>>>> GetAllFollowersForAccounts(List<long> accountIds)
        {
            List<Result<List<long>>> followersForAccounts = new List<Result<List<long>>>();
            foreach (long accountId in accountIds)
            {
                Result<List<long>> followers = await _dependencies.GetFollowersOfTweeterAccount(accountId);
                followersForAccounts.Add(followers);
            }

            return followersForAccounts.Traverse();
        }

        private Task<Result<List<long>>> GetFollowersWhoFollowAllTheAccounts(List<List<long>> followers)
        {
            return followers.GetCommonItems().Success().Async();            
        }
    }
}