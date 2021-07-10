using Moq;
using ROP;
using SocialGiveaway.Services.Twitter;
using System.Collections.Generic;
using System.Threading.Tasks;
using SocialGiveaway.Dto.Twitter;
using Xunit;

namespace SocialGiveaway.UnitTest.Services.Twitter
{
    public class TestSelectWinner
    {
        [Fact]
        public async Task GetWinner_whenFollowLikeAndRetweet()
        {
            int tweetId = 1;
            int tweeterAccount = 123;
            List<int> followersOfAccount = new List<int>() {1, 2, 3, 5};

            TweetTicketDto ticket1 = new TweetTicketDto();
            ticket1.Rules = new List<TwitterRule>()
            {
                TwitterRule.Follow,
                TwitterRule.Like,
                TwitterRule.Retweet
            };

            Mock<ISelectWinnerDependencies> dependencies = new Mock<ISelectWinnerDependencies>();
            dependencies.Setup(a => a.GetTwitterAccountFromTweetId(1))
                .ReturnsAsync(tweeterAccount.Success());
            dependencies.Setup(a => a.GetFollowersOfTweeterAccount(tweeterAccount))
                .ReturnsAsync(followersOfAccount.Success());
            dependencies.Setup(a => a.GetUserIdWhoLikedATweet(tweetId))
                .ReturnsAsync(followersOfAccount.Success());
            dependencies.Setup(a => a.GetUserIdWhoRetweetedATweet(tweetId))
                .ReturnsAsync(followersOfAccount.Success());
            dependencies.Setup(a => a.GetRandomNumber(0, 3))
                .Returns(2);

            SelectWinner selectWinner = new SelectWinner(dependencies.Object);

            Result<TwitterUser> result = await selectWinner.Execute(tweetId, new List<TweetTicketDto>() {ticket1});
            Assert.True(result.Success);
            Assert.Equal(3, result.Value.Id);
        }


        [Fact]
        public async Task GetWinner_whenFollowLikeAndRetweet_WithCommentAndFollow()
        {
            int tweetId = 1;
            int tweeterAccount = 123;
            List<int> followersOfAccount = new List<int>() {1, 2, 3, 5};
            List<int> usersWhoCommented = new List<int>() {3, 4, 5};

            TweetTicketDto ticket1 = new TweetTicketDto
            {
                Rules = new List<TwitterRule>() {TwitterRule.Follow, TwitterRule.Like, TwitterRule.Retweet}
            };

            TweetTicketDto ticket2 = new TweetTicketDto();
            ticket2.Rules = new List<TwitterRule>()
            {
                TwitterRule.Follow,
                TwitterRule.Comment
            };

            Mock<ISelectWinnerDependencies> dependencies = new Mock<ISelectWinnerDependencies>();
            dependencies.Setup(a => a.GetTwitterAccountFromTweetId(1))
                .ReturnsAsync(tweeterAccount.Success());
            dependencies.Setup(a => a.GetFollowersOfTweeterAccount(tweeterAccount))
                .ReturnsAsync(followersOfAccount.Success());
            dependencies.Setup(a => a.GetUserIdWhoLikedATweet(tweetId))
                .ReturnsAsync(followersOfAccount.Success());
            dependencies.Setup(a => a.GetUserIdWhoRetweetedATweet(tweetId))
                .ReturnsAsync(followersOfAccount.Success());
            dependencies.Setup(a => a.GetRandomNumber(0, 5))
                .Returns(5);
            dependencies.Setup(a => a.GetResponsesOfATweet(tweetId))
                .ReturnsAsync(usersWhoCommented.Success());

            SelectWinner selectWinner = new SelectWinner(dependencies.Object);

            Result<TwitterUser> result =
                await selectWinner.Execute(tweetId, new List<TweetTicketDto>() {ticket1, ticket2});
            Assert.True(result.Success);
            Assert.Equal(5, result.Value.Id);
        }
    }
}