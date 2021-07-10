using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialGiveAway.Dto
{
    public class TweetTicketDto
    {
        public List<TwitterRule> Rules { get; set; }

    }
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
