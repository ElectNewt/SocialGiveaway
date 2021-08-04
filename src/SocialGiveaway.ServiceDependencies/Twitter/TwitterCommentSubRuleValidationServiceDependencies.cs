using ROP;
using SocialGiveaway.External.Twitter.Functionalities;
using SocialGiveaway.Model.Twitter;
using SocialGiveaway.Services.Twitter;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialGiveaway.ServiceDependencies.Twitter
{
    public class TwitterCommentSubRuleValidationServiceDependencies : ITwitterCommentSubRuleValidationDependencies
    {

        private readonly TweetFunctionalities _tweets;
        public TwitterCommentSubRuleValidationServiceDependencies(TweetFunctionalities tweets)
        {
            _tweets = tweets;
        }

        public async Task<Result<List<TweetInformation>>> GetResponsesOfATweet(long tweetId)
            => await _tweets.Comments.GetUsersWhoCommented(tweetId);
    }
}
