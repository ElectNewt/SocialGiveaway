using System.Text.Json.Serialization;

namespace SocialGiveaway.Dto.Twitter
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TwitterSubRule
    {
        None,
        Follow,
        Mention, 
        Hashtag
    }
}