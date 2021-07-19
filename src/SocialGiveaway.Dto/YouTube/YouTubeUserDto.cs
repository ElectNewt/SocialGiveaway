namespace SocialGiveaway.Dto.YouTube
{
    public record YouTubeUserDto
    {
        public string ChannelUrl { get; init; }
        public string YouTubeName { get; init; }

        public YouTubeUserDto(string channelUrl, string youTubeName)
        {
            ChannelUrl = channelUrl;
            YouTubeName = youTubeName;
        }
    }
}
