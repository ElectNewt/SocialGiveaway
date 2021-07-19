using System.Text.Json.Serialization;

namespace SocialGiveaway.Dto.Twitter
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TwitterRule
    {
        Follow,
        Like,
        Comment,
        Retweet,
        CommentPlusQuote,
        Hashtag
    }
}