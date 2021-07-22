namespace SocialGiveaway.Model.Twitter
{
    public record TweetHashtag
    {
        public string Hashtag { get; init; }

        public TweetHashtag(string hashtag)
        {
            Hashtag = hashtag;
        }
    }

}
