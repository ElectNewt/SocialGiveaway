using System.Collections.Generic;
using System.Linq;
using SocialGiveaway.Shared.Extensions;
using Xunit;

namespace SocialGiveaway.UnitTest.Shared.Extensions
{
    public class TestListExtensions
    {
        [Fact]
        public void When_MultipleList_then_GetCommonItems()
        {
            List<int> expectedItems = new List<int>() {7, 10, 44};

            List<int> firstList = new List<int>() {4, 29, 39, 10, 55, 44, 7, 77};
            List<int> secondList = new List<int>() {9, 10, 30, 29, 11, 7, 4, 44};
            List<int> thirdList = new List<int>() {6, 7, 8, 10, 38, 44, 77};
            List<List<int>> subject = new List<List<int>>()
            {
                firstList, secondList, thirdList
            };

            List<int> result = subject.GetCommonItems().OrderBy(a=>a).ToList();
            Assert.Equal(3, result.Count);
            Assert.Equal(expectedItems.First(),result.First() );
            Assert.Equal(expectedItems.Skip(1).First(),result.Skip(1).First() );
            Assert.Equal(expectedItems.Skip(2).First(),result.Skip(2).First() );
        }
    }
}