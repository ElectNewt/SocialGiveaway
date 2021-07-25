namespace SocialGiveaway.External.Twitter.Credentials
{
    public class TwitterModificableCredentials
    {
        public readonly string ConsumerKey;
        public readonly string ConsumerSecret;
        public string Token => GetToken();
        private string? _token { get; set; }

        public TwitterModificableCredentials(string consumerKey, string consumerSecret)
        {
            ConsumerKey = consumerKey;
            ConsumerSecret = consumerSecret;
        }

        private string GetToken()
        {
            return _token ?? throw new System.Exception("The application did not set the token but you are trying to access to it.");
        }

        internal void UpdateToken(string bearerToken)
        {
            _token = bearerToken;
        }
    }
}
