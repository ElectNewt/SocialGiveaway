namespace SocialGiveaway.External.Twitter.Credentials
{
    public class TwitterCredentials
    {
        public readonly string ConsumerKey;
        public readonly string ConsumerSecret;
        public readonly string Token;

        public TwitterCredentials(string consumerKey, string consumerSecret, string token)
        {
            ConsumerKey = consumerKey;
            ConsumerSecret = consumerSecret;
            Token = token;
        }
    }
}
