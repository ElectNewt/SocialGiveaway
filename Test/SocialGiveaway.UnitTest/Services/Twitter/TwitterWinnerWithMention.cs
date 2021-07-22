using Moq;
using ROP;
using SocialGiveaway.Services.Twitter;
using System.Collections.Generic;
using System.Threading.Tasks;
using SocialGiveaway.Dto.Twitter;
using Xunit;
using SocialGiveaway.Model.Twitter;
using System.Collections.ObjectModel;

namespace SocialGiveaway.UnitTest.Services.Twitter
{
    public class TwitterWinnerWithMention
    {
        [Fact]
        public async Task GetTwitterWinnerWithMention()
        {
            string mention = "atMe";

            long tweetId = 1;
            long tweeterAccount = 123;
            List<long> usersWhoCommented = new List<long>() { 1 };

            ReadOnlyCollection<TweetMention> mentions = new List<TweetMention>()
            {
                new TweetMention(mention)
            }.AsReadOnly();

            List<TweetInformation> tweets = new List<TweetInformation>()
            {
                new ("xx", "1", mentions, null),
            };

            TweetTicketDto ticket1 = new TweetTicketDto
            {
                Rules = new List<TwitterRuleDto>()
                {
                    new()
                    {
                        Type = TwitterRuleType.Comment,
                        Conditions = new List<TwitterConditionDto>()
                        {
                            new TwitterConditionDto()
                            {
                                SubRule = TwitterSubRule.Quote,
                                Condition= mention
                            }
                        }
                    }
                }
            };


            Mock<ISelectTwitterWinnerDependencies> dependencies = new Mock<ISelectTwitterWinnerDependencies>();
            dependencies.Setup(a => a.GetTwitterAccountFromTweetId(1))
                .ReturnsAsync(tweeterAccount.Success());
            dependencies.Setup(a => a.GetRandomNumber(0, 0))
                .Returns(0);
            dependencies.Setup(a => a.GetResponsesOfATweet(tweetId))
                .ReturnsAsync(tweets.Success());
            dependencies.Setup(a => a.GetUsername(1))
                .ReturnsAsync(("name1", "at1"));


            SelectTwitterWinner selectWinner = new SelectTwitterWinner(dependencies.Object);

            Result<TwitterUserDto> result =
                await selectWinner.Execute(tweetId, new List<TweetTicketDto>() { ticket1 });
            Assert.True(result.Success);
            Assert.Equal("name1", result.Value.Name);
            Assert.Equal("at1", result.Value.At);

        }
    }
}
