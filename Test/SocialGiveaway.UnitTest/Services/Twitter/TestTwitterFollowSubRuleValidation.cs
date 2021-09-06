using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using ROP;
using SocialGiveaway.Dto.Twitter;
using SocialGiveaway.Services.Twitter;
using Xunit;

namespace SocialGiveaway.UnitTest.Services.Twitter
{
    public class TestTwitterFollowSubRuleValidation
    {
        public class TestState
        {
            public Mock<ITwitterFollowSubRuleValidationDependencies> Dependencies;

            public TwitterFollowSubRuleValidation Subject;
            public readonly string AccountName;
            public readonly int TweetId;
            public readonly List<long> DefaultFollowers;
            public readonly long AccountId = 1;

            public TestState()
            {
                AccountName = "testAccountName";
                TweetId = 1;
                AccountId= 1;
                DefaultFollowers = new List<long>() {1, 2, 3};
                Dependencies = new Mock<ITwitterFollowSubRuleValidationDependencies>();
                Dependencies.Setup(a => a.GetAccountIdByAccountName(AccountName))
                    .ReturnsAsync(AccountId);
                Dependencies.Setup(a => a.GetFollowersOfTweeterAccount(AccountId))
                    .ReturnsAsync(DefaultFollowers);
                
                Subject = new TwitterFollowSubRuleValidation(Dependencies.Object);
            }

            public void VerifyMockCalls()
            {
                Dependencies.Verify(a => a.GetAccountIdByAccountName(AccountName));
                Dependencies.Verify(a => a.GetFollowersOfTweeterAccount(AccountId));
            }
        }
        
        [Fact]
        public async Task WhenAccountNameIsProvided_Then_GetFollowers()
        {
            TestState state = new TestState();
            
            Result<List<long>> result =await state.Subject.Execute(new TwitterRuleDto()
            {
                Type = TwitterRuleType.Follow,
                Conditions = new List<TwitterConditionDto>()
                {
                    new TwitterConditionDto()
                    {
                        SubRule = TwitterSubRule.Follow,
                        Condition = state.AccountName
                    }
                }
            }, state.TweetId);
            Assert.True(result.Success);
            Assert.Equal(state.DefaultFollowers.Count, result.Value.Count);
            Assert.True(state.DefaultFollowers.SequenceEqual(result.Value));
            state.VerifyMockCalls();
            

        }
        
        [Fact]
        public async Task WhenMultipleAccountNamesAreProvided_Then_GetCommonFollowers()
        {
            TestState state = new TestState();
            string additionalAccountName = "secondName";
            int additionalAccountId = 2;

            state.Dependencies.Setup(a => a.GetAccountIdByAccountName(additionalAccountName))
                .ReturnsAsync(additionalAccountId);
            state.Dependencies.Setup(a => a.GetFollowersOfTweeterAccount(additionalAccountId))
                .ReturnsAsync(new List<long>() { 1,2,3,4,5,6});
            
            
            Result<List<long>> result =await state.Subject.Execute(new TwitterRuleDto()
            {
                Type = TwitterRuleType.Follow,
                Conditions = new List<TwitterConditionDto>()
                {
                    new TwitterConditionDto()
                    {
                        SubRule = TwitterSubRule.Follow,
                        Condition = state.AccountName
                    },
                    new TwitterConditionDto()
                    {
                        SubRule = TwitterSubRule.Follow,
                        Condition = additionalAccountName
                    }
                }
            }, state.TweetId);
            
            
            
            Assert.True(result.Success);
            Assert.Equal(state.DefaultFollowers.Count, result.Value.Count);
            Assert.True(state.DefaultFollowers.SequenceEqual(result.Value));
            state.VerifyMockCalls();
            state.Dependencies.Setup(a => a.GetAccountIdByAccountName(additionalAccountName));
            state.Dependencies.Setup(a => a.GetFollowersOfTweeterAccount(additionalAccountId));
        }
        
        [Fact]
        public async Task WhenNoAccountNameIsProvided_Then_GetCommonFollowersFromTweetOwner()
        {
            TestState state = new TestState();
            int additionalAccountId = 2;

            state.Dependencies.Setup(a => a.GetTwitterAccountFromTweetId(state.TweetId))
                .ReturnsAsync(additionalAccountId);
            state.Dependencies.Setup(a => a.GetFollowersOfTweeterAccount(additionalAccountId))
                .ReturnsAsync(state.DefaultFollowers);
            
            
            Result<List<long>> result = await state.Subject.Execute(new TwitterRuleDto()
            {
                Type = TwitterRuleType.Follow,
                Conditions = new List<TwitterConditionDto>(){}
            }, state.TweetId);
            
            
            
            Assert.True(result.Success);
            Assert.Equal(state.DefaultFollowers.Count, result.Value.Count);
            Assert.True(state.DefaultFollowers.SequenceEqual(result.Value));
        }
    }
}