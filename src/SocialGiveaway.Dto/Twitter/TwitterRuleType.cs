using System.Text.Json.Serialization;

namespace SocialGiveaway.Dto.Twitter
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TwitterRuleType
    {
        Follow,
        Like,
        Comment,
        Retweet
    }
}