using System.Collections.Generic;

namespace SocialGiveaway.Dto.Twitter
{
    public record TweetTicketDto
    {
        public List<TwitterRuleDto> Rules { get; init; }
        public TweetTicketDto(List<TwitterRuleDto> rules)
        {
            Rules = rules;
        }
    }
}
