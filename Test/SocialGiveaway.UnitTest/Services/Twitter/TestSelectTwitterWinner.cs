using Moq;
using ROP;
using SocialGiveaway.Dto.Twitter;
using SocialGiveaway.Model.Twitter;
using SocialGiveaway.Services.Twitter;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SocialGiveaway.UnitTest.Services.Twitter
{
    public class TestSelectTwitterWinner
    {
        [Fact]
        public async Task GetWinner_whenFollowLikeAndRetweet()
        {
            long tweetId = 1;
            // long tweeterAccount = 123;
            List<long> followersOfAccount = new List<long>() {1, 2, 3, 5};

            TweetTicketDto ticket1 = new TweetTicketDto(new List<TwitterRuleDto>()
            {
                new()
                {
                    Type = TwitterRuleType.Follow
                },
                new()
                {
                    Type = TwitterRuleType.Like
                },
                new()
                {
                    Type = TwitterRuleType.Retweet
                }
            });

            Mock<ISelectTwitterWinnerDependencies> dependencies = new Mock<ISelectTwitterWinnerDependencies>();

            dependencies.Setup(a => a.GetUserIdWhoLikedATweet(tweetId))
                .ReturnsAsync(followersOfAccount.Success());
            dependencies.Setup(a => a.GetUserIdWhoRetweetedATweet(tweetId))
                .ReturnsAsync(followersOfAccount.Success());
            dependencies.Setup(a => a.GetRandomNumber(0, 3))
                .Returns(2);
            dependencies.Setup(a => a.GetUsername(3))
                .ReturnsAsync(("number3", "at3"));

            Mock<ITwitterCommentSubRuleValidationDependencies> commentDependencies =
                new Mock<ITwitterCommentSubRuleValidationDependencies>();
            Mock<ITwitterFollowSubRuleValidation> followDependencies = new Mock<ITwitterFollowSubRuleValidation>();
            followDependencies.Setup(a => a.Execute(It.IsAny<TwitterRuleDto>(), It.IsAny<long>()))
                .ReturnsAsync(followersOfAccount.Success());
            SelectTwitterWinner selectWinner = new SelectTwitterWinner(dependencies.Object,
                new TwitterCommentSubRuleValidation(commentDependencies.Object),
                followDependencies.Object);

            Result<TwitterUserDto> result = await selectWinner.Execute(tweetId, new List<TweetTicketDto>() {ticket1});
            Assert.True(result.Success);
            Assert.Equal("number3", result.Value.Name);
            Assert.Equal("at3", result.Value.At);
        }


        [Fact]
        public async Task GetWinner_whenFollowLikeAndRetweet_WithCommentAndFollow()
        {
            long tweetId = 1;
            // long tweeterAccount = 123;
            List<long> followersOfAccount = new List<long>() {1, 2, 3, 5};

            List<TweetInformation> tweets = new List<TweetInformation>()
            {
                new("xx", "3", null, null),
                new("xx", "4", null, null),
                new("xx", "5", null, null)
            };

            TweetTicketDto ticket1 = new TweetTicketDto(new List<TwitterRuleDto>()
            {
                new()
                {
                    Type = TwitterRuleType.Follow
                },
                new()
                {
                    Type = TwitterRuleType.Like
                },
                new()
                {
                    Type = TwitterRuleType.Retweet
                }
            });

            TweetTicketDto ticket2 = new TweetTicketDto(new List<TwitterRuleDto>()
            {
                new()
                {
                    Type = TwitterRuleType.Follow
                },
                new()
                {
                    Type = TwitterRuleType.Comment,
                }
            });

            Mock<ISelectTwitterWinnerDependencies> dependencies = new Mock<ISelectTwitterWinnerDependencies>();

            dependencies.Setup(a => a.GetUserIdWhoLikedATweet(tweetId))
                .ReturnsAsync(followersOfAccount.Success());
            dependencies.Setup(a => a.GetUserIdWhoRetweetedATweet(tweetId))
                .ReturnsAsync(followersOfAccount.Success());
            dependencies.Setup(a => a.GetRandomNumber(0, 5))
                .Returns(5);
            dependencies.Setup(a => a.GetUsername(5))
                .ReturnsAsync(("number5", "at5"));

            Mock<ITwitterCommentSubRuleValidationDependencies> commentDependencies =
                new Mock<ITwitterCommentSubRuleValidationDependencies>();
            commentDependencies.Setup(a => a.GetResponsesOfATweet(tweetId))
                .ReturnsAsync(tweets.Success());
            Mock<ITwitterFollowSubRuleValidation> followDependencies = new Mock<ITwitterFollowSubRuleValidation>();
            followDependencies.Setup(a => a.Execute(It.IsAny<TwitterRuleDto>(), It.IsAny<long>()))
                .ReturnsAsync(followersOfAccount.Success());

            SelectTwitterWinner selectWinner = new SelectTwitterWinner(dependencies.Object,
                new TwitterCommentSubRuleValidation(commentDependencies.Object),
                followDependencies.Object);

            Result<TwitterUserDto> result =
                await selectWinner.Execute(tweetId, new List<TweetTicketDto>() {ticket1, ticket2});
            Assert.True(result.Success);
            Assert.Equal("number5", result.Value.Name);
            Assert.Equal("at5", result.Value.At);
        }
    }
}