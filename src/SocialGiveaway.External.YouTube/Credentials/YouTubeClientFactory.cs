using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Upload;
using Google.Apis.Util.Store;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;

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
