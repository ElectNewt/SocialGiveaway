namespace SocialGiveaway.External.Twitter.Credentials
{
    public class YouTubeConfiguration
    {
        public readonly YouTubeCredentials Credentials;
        public readonly YouTubeSettings Settings;

        public YouTubeConfiguration(YouTubeCredentials credentials, YouTubeSettings settings)
        {
            Credentials = credentials;
            Settings = settings;
        }
    }

    public class YouTubeCredentials
    {
        public readonly string ApiKey;

        public YouTubeCredentials(string apiKey)
        {
            ApiKey = apiKey;
        }
    }

    public class YouTubeSettings
    {
        public readonly string ApplicationName;

        public YouTubeSettings(string applicationName)
        {
            ApplicationName = applicationName;
        }
    }
}
