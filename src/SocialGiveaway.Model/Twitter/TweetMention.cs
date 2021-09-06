namespace SocialGiveaway.Model.Twitter
{

    public record TweetMention
    {
        public string Username { get; init; }

        public TweetMention(string username)
        {
            Username = username;
        }
    }

}
