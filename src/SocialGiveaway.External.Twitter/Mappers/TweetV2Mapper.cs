using SocialGiveaway.Model.Twitter;
using System.Collections.ObjectModel;
using System.Linq;
using Tweetinvi.Models.V2;

namespace SocialGiveaway.ServiceDependencies.Twitter.Mappers
{
    public static class TweetV2Mapper
    {

        public static TweetInformation ToTweetInformation(this TweetV2 tweet)
        {
            ReadOnlyCollection<TweetMention>? mentions = tweet.Entities?.Mentions?.Select(a => new TweetMention(a.Username)).ToList().AsReadOnly();
            ReadOnlyCollection<TweetHashtag>? hastags = tweet.Entities?.Hashtags?.Select(a => new TweetHashtag(a.Tag)).ToList().AsReadOnly();
            return new TweetInformation(tweet.Id, tweet.AuthorId, mentions, hastags);
        }

    }
}
