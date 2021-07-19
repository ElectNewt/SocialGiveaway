using Moq;
using ROP;
using SocialGiveaway.Dto.YouTube;
using SocialGiveaway.Services.YouTube;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SocialGiveaway.UnitTest.Services.YouTube
{
    public class TestSelectYoutubeWinner
    {
        [Fact]
        public async Task Test()
        {
            string videoId = "video";
            Mock<ISelectYouTubeWinnerDependencies> dependencies = new Mock<ISelectYouTubeWinnerDependencies>();
            List<(string channelUrl, string username)> channelsWhoCommented
                = new List<(string channelUrl, string username)>()
                {
                    ("channel1", "username1"),
                    ("channel2", "username2"),
                    ("channel3", "username3"),
                    ("channel4", "username4"),
                    ("channel5", "username5")
                };
            dependencies.Setup(d => d.GetChannelIdsWoCommented(videoId))
                .ReturnsAsync(channelsWhoCommented);
            dependencies.Setup(a => a.GetRandomNumber(0, 4))
                .Returns(2);


            SelectYouTubeWinner subject = new SelectYouTubeWinner(dependencies.Object);

            Result<YouTubeUserDto> result = await subject.Execute(videoId, new List<YouTubeTicketDto>()
            { new YouTubeTicketDto()
                {
                    Rules = new List<YouTubeRule>(){ YouTubeRule.Comment }
                }
            });
            Assert.True(result.Success);
            Assert.Equal("channel3", result.Value.ChannelUrl);
            Assert.Equal("username3", result.Value.YouTubeName);

        }
    }
}
