using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Curiosity.Common.Mvc
{
    [TestFixture]
    public class CollectionExtensionsTests
    {
        [Test]
        public void ToSelectList_ConvertsItemsToSelectList()
        {
            var list = CreateSampleList();
            var selectList = list.ToSelectList(x => x.Key.ToString(), x => x.Value);
            Assert.AreEqual(3, selectList.Count());
            Assert.Contains("1", selectList.Select(x => x.Value).ToList());
            Assert.Contains("2", selectList.Select(x => x.Value).ToList());
            Assert.Contains("3", selectList.Select(x => x.Value).ToList());
        }

        [Test]
        [TestCase(1, "1")]
        [TestCase(2, "2")]
        [TestCase(3, "3")]
        public void ToSelectList_MarksItemsAsSelected(int intValue, string stringValue)
        {
            var list = CreateSampleList();
            var selectList = list.ToSelectList(x => x.Key, x => x.Value, intValue);
            var itemWithKey = selectList.First(x => x.Value == stringValue);
            Assert.IsTrue(itemWithKey.Selected);
        }

        private static IEnumerable<KeyValuePair<int, string>> CreateSampleList()
        {
            return new List<KeyValuePair<int, string>>
                       {
                           new KeyValuePair<int, string>(1, "First"),
                           new KeyValuePair<int, string>(2, "Second"),
                           new KeyValuePair<int, string>(3, "Third")
                       };
        }
    }
}

