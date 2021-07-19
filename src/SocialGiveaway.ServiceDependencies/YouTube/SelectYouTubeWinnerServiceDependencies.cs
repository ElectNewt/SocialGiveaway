using ROP;
using SocialGiveaway.Dto.YouTube;
using SocialGiveaway.External.Twitter.Functionalities;
using SocialGiveaway.Services.YouTube;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialGiveaway.ServiceDependencies.YouTube
{
    public class SelectYouTubeWinnerServiceDependencies : ISelectYouTubeWinnerDependencies
    {

        private readonly Youtube _youtube;

        public SelectYouTubeWinnerServiceDependencies(Youtube youtube)
        {
            _youtube = youtube;
        }

        public async Task<Result<List<(string channelUrl, string username)>>> GetChannelIdsWoCommented(string videoId)
        {
            return await _youtube.GetCommentsAuthorsOnVideo(videoId);
        }

        public int GetRandomNumber(int start, int end)
        {
            Random random = new Random();
            return random.Next(start, end);
        }
    }
}
