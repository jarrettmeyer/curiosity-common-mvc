using System.Linq;
using System.Web.Mvc;
using NUnit.Framework;

namespace Curiosity.Common.Mvc
{
    [TestFixture]
    public class FlashStorageTests
    {
        private TempDataDictionary tempData;
        private FlashStorage flashStorage;

        [SetUp]
        public void SetUp()
        {
            tempData = new TempDataDictionary();
            flashStorage = new FlashStorage(tempData);
        }

        [Test]
        public void AfterReadingOnce_MessagesCollectionShouldBeCleared()
        {
            flashStorage.Add("notice", "This is a test message.");

            Assert.AreEqual(1, flashStorage.Messages.Count(), "On 1st read, flash should have a message.");
            Assert.AreEqual(0, flashStorage.Messages.Count(), "On second read, flash should be empty.");
        }

        [Test]
        public void WhenAdded_MessageIsInMessagesCollection()
        {
            const string type = "success";
            const string message = "This is a success message";

            flashStorage.Add(type, message);

            var messages = flashStorage.Messages;

            Assert.Contains(type, messages.Select(m => m.Key).ToList());
            Assert.Contains(message, messages.Select(m => m.Value).ToList());
        }
    }
}
