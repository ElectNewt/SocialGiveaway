using System.Text.Json.Serialization;

namespace SocialGiveaway.Dto.YouTube
{
    [JsonConverter(typeof(JsonStringEnumConverter))]

    public enum YouTubeRule
    {
        Comment
    }
}
