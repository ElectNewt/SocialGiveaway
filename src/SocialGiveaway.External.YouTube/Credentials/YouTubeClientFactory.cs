using Google.Apis.Services;
using Google.Apis.YouTube.v3;

namespace SocialGiveaway.External.Twitter.Credentials
{
    public interface IYouTubeClientFactory
    {
        YouTubeService GetYouTubeClient();
    }

    public class YouTubeClientFactory : IYouTubeClientFactory
    {

        public readonly YouTubeConfiguration _configuration;

        public YouTubeClientFactory(YouTubeConfiguration configuration)
        {
            _configuration = configuration;
        }

        public YouTubeService GetYouTubeClient()
        {
            return new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = _configuration.Credentials.ApiKey,
                ApplicationName = _configuration.Settings.ApplicationName
            });
        }
    }
}
