using System.Collections.ObjectModel;

namespace SocialGiveaway.Model.Twitter
{
    public record TweetInformation
    {
        public string TweetId { get; init; }
        public string AuthorId { get; init; }
        public ReadOnlyCollection<TweetMention>? Mentions { get; init; }

        public ReadOnlyCollection<TweetHashtag>? Hashtags { get; init; }

        public TweetInformation(string tweetId, string authorId, ReadOnlyCollection<TweetMention>? mentions, ReadOnlyCollection<TweetHashtag>? hashtags)
        {
            TweetId = tweetId;
            AuthorId = authorId;
            Mentions = mentions;
            Hashtags = hashtags;
        }
    }
}
