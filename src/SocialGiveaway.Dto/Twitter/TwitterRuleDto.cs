using System.Collections.Generic;

namespace SocialGiveaway.Dto.Twitter
{
    public class TwitterRuleDto
    {
        public TwitterRuleType Type { get; set; }
        public List<TwitterConditionDto>? Conditions { get; set; }
        
    }
}