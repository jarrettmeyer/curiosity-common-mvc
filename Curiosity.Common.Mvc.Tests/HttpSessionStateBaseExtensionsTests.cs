using System.Web;
using NUnit.Framework;

namespace Curiosity.Common.Mvc
{
    [TestFixture]
    public class HttpSessionStateBaseExtensionsTests
    {
        private HttpSessionStateBase session;

        [SetUp]
        public void BeforeEachTest()
        {
            HttpTest.SetUpHttpContext();
            var context = HttpContext.Current;            
            session = new HttpSessionStateWrapper(context.Session);
        }

        [TearDown]
        public void AfterEachTest()
        {
            HttpTest.DestroyHttpContext();
        }

        [Test]
        public void Get_WhenKeyIsNotStored_ReturnsDefault()
        {
            var result = session.Get<int>("key not stored");
            Assert.AreEqual(default(int), result);
        }

        [Test]
        public void Get_WhenKeyIsStored_ReturnsExpectedValue()
        {
            session["test"] = 456;
            var result = session.Get<int>("test");
            Assert.AreEqual(456, result);
        }

        [Test]
        public void TryGetValue_WhenKeyIsNotStored_ReturnsFalse()
        {
            int value;
            var result = session.TryGetValue("key not stored", out value);
            Assert.IsFalse(result);
        }

        [Test]
        public void TryGetValue_WhenKeyIsStored_ReturnsTrue()
        {
            session["test"] = 123;
            int value;
            var result = session.TryGetValue("test", out value);
            Assert.IsTrue(result);
        }

        [Test]
        public void TryGetValue_WhenKeyIsStored_ReturnsValueThroughParameter()
        {
            session["test"] = 123;
            int value;
            session.TryGetValue("test", out value);
            Assert.AreEqual(123, value);
        }
    }
}
