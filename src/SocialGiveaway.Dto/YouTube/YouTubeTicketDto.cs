using System.Collections.Generic;

namespace SocialGiveaway.Dto.YouTube
{
    public record YouTubeTicketDto
    {
        public List<YouTubeRule> Rules { get; init; }

        public YouTubeTicketDto(List<YouTubeRule> rules)
        {
            Rules = rules;
        }
    }
}
