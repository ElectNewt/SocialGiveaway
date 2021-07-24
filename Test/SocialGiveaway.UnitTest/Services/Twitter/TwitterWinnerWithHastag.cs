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
    public class TwitterWinnerWithHastag
    {
        [Fact]
        public async Task GetTwitterWinnerWithHashtag()
        {
            string hashtag = "ThisIsAtest";

            long tweetId = 1;
            long tweeterAccount = 123;
            List<long> usersWhoCommented = new List<long>() { 1 };

            ReadOnlyCollection<TweetHashtag> hashtags = new List<TweetHashtag>()
            {
                new TweetHashtag(hashtag)
            }.AsReadOnly();

            List<TweetInformation> tweets = new List<TweetInformation>()
            {
                new ("xx", "1", null, hashtags),
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
                                SubRule = TwitterSubRule.Hashtag,
                                Condition= hashtag
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
            dependencies.Setup(a => a.GetUsername(1))
                .ReturnsAsync(("name1", "at1"));

            Mock<ITwitterCommentSubRuleValidationDependencies> commentDependencies = new Mock<ITwitterCommentSubRuleValidationDependencies>();
            commentDependencies.Setup(a => a.GetResponsesOfATweet(tweetId))
                .ReturnsAsync(tweets.Success());


            SelectTwitterWinner selectWinner = new SelectTwitterWinner(dependencies.Object, new TwitterCommentSubRuleValidation(commentDependencies.Object));

            Result<TwitterUserDto> result =
                await selectWinner.Execute(tweetId, new List<TweetTicketDto>() { ticket1 });
            Assert.True(result.Success);
            Assert.Equal("name1", result.Value.Name);
            Assert.Equal("at1", result.Value.At);

        }
    }
}
