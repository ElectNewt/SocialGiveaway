using ROP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SocialGiveaway.Dto.Twitter;
using SocialGiveaway.Model.Twitter;

namespace SocialGiveaway.Services.Twitter
{
    public interface ITwitterCommentSubRuleValidationDependencies
    {
        Task<Result<List<TweetInformation>>> GetResponsesOfATweet(long tweetId);
    }

    public interface ITwitterCommentSubRuleValidation
    {
        Task<Result<List<long>>> Execute(TwitterRuleDto rule, long tweetId);
    }
    
    public class TwitterCommentSubRuleValidation : ITwitterCommentSubRuleValidation
    {

        private readonly ITwitterCommentSubRuleValidationDependencies _dependencies;

        public TwitterCommentSubRuleValidation(ITwitterCommentSubRuleValidationDependencies dependencies)
        {
            _dependencies = dependencies;
        }

        public async Task<Result<List<long>>> Execute(TwitterRuleDto rule, long tweetId)
        => await _dependencies.GetResponsesOfATweet(tweetId)
                .Bind(x => ValidateSubRule(x, rule.Conditions).Async());


        private Result<List<long>> ValidateSubRule(List<TweetInformation> tweets, List<TwitterConditionDto>? conditions)
            => tweets.Where(a => ValidateConditions(a, conditions)).Select(a => long.Parse(a.AuthorId))
                .ToList();


        //all conditions must be fulfil || or no conditions are specified 
        private bool ValidateConditions(TweetInformation tweet, List<TwitterConditionDto>? conditions)
            => conditions?.All(condition => ValidateCommentSubRule(condition, tweet)) ?? true;

        private bool ValidateCommentSubRule(TwitterConditionDto conditionDto, TweetInformation tweet) =>
            conditionDto.SubRule switch
            {
                TwitterSubRule.None => true,
                TwitterSubRule.Follow => throw new Exception(
                    "You should not validate the follow subRule using the comment"),
                TwitterSubRule.Mention => tweet.Mentions?
                    .Select(a => a.Username).Any(mention=>mention.Equals(conditionDto.Condition, StringComparison.OrdinalIgnoreCase)) ?? false,
                TwitterSubRule.Hashtag =>
                    tweet.Hashtags?.Select(a => a.Hashtag).Any(hashtag=>hashtag.Equals(conditionDto.Condition, StringComparison.OrdinalIgnoreCase)) ?? false,
                _ => throw new ArgumentOutOfRangeException()
            };
    }
}
