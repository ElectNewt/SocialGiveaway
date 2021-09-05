using Moq;
using ROP;
using SocialGiveaway.Services.Twitter;
using System.Collections.Generic;
using System.Threading.Tasks;
using SocialGiveaway.Dto.Twitter;
using Xunit;
using SocialGiveaway.Model.Twitter;
using System.Collections.ObjectModel;
using System.Linq;

namespace SocialGiveaway.UnitTest.Services.Twitter
{
    public class TwitterWinnerWithHashtag
    {

        public class TestState
        {
            public  SelectTwitterWinner Subject;
            public Mock<ISelectTwitterWinnerDependencies> _dependencies;
            public Mock<ITwitterCommentSubRuleValidationDependencies> _commentDependencies;
            public readonly long TweetId;
            public readonly TweetTicketDto Ticket1;
            public TestState(string commentHashtag, string hashtag)
            {
                TweetId = 1;
                long tweeterAccount = 123;
                
                ReadOnlyCollection<TweetHashtag> hashtags = new List<TweetHashtag>()
                {
                    new TweetHashtag(commentHashtag)
                }.AsReadOnly();

                List<TweetInformation> tweets = new()
                {
                    new ("xx", "1", null, hashtags),
                };

                Ticket1 = new TweetTicketDto
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
                                    Condition = hashtag
                                }
                            }
                        }
                    }
                };
                
                _dependencies = new Mock<ISelectTwitterWinnerDependencies>();
                _dependencies.Setup(a => a.GetTwitterAccountFromTweetId(1))
                    .ReturnsAsync(tweeterAccount.Success());
                _dependencies.Setup(a => a.GetRandomNumber(0, 0))
                    .Returns(0);
                _dependencies.Setup(a => a.GetUsername(1))
                    .ReturnsAsync(("name1", "at1"));

                _commentDependencies = new Mock<ITwitterCommentSubRuleValidationDependencies>();
                _commentDependencies.Setup(a => a.GetResponsesOfATweet(TweetId))
                    .ReturnsAsync(tweets.Success());

                Subject = new SelectTwitterWinner(_dependencies.Object, new TwitterCommentSubRuleValidation(_commentDependencies.Object));

            }

        }
        
        [Fact]
        public async Task GetTwitterWinnerWithHashtag()
        {
            string hashtag = "ThisIsAtest";

            TestState test = new TestState(hashtag, hashtag);
            
            Result<TwitterUserDto> result =
                await test.Subject.Execute(test.TweetId, new List<TweetTicketDto>() { test.Ticket1 });
            Assert.True(result.Success);
            Assert.Equal("name1", result.Value.Name);
            Assert.Equal("at1", result.Value.At);

        }
        
        [Fact]
        public async Task GetTwitterWinnerWithHashtagCaseInsensitive()
        {
            string hashtag = "ThisIsAtest";

            TestState test = new TestState(hashtag.ToUpper(), hashtag);
            
            
            Result<TwitterUserDto> result =
                await test.Subject.Execute(test.TweetId, new List<TweetTicketDto>() { test.Ticket1 });
            Assert.True(result.Success);
            Assert.Equal("name1", result.Value.Name);
            Assert.Equal("at1", result.Value.At);

        }
    }
}
