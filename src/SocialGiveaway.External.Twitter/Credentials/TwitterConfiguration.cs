using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialGiveaway.External.Twitter.Credentials
{
    public class TwitterConfiguration
    {
        public readonly TwitterModificableCredentials Credentials;
        public readonly TwitterSettings Settings;

        public TwitterConfiguration(TwitterModificableCredentials twitterCredentials, TwitterSettings twitterSetting)
        {
            Credentials = twitterCredentials;
            Settings = twitterSetting;
        }
    }

    public class TwitterSettings
    {
        public const string HttpFactoryName = "TwitterApi";
        public readonly string ApiUrl;

        public TwitterSettings(string twitterUrl)
        {
            ApiUrl = twitterUrl;
        }
    }
}
