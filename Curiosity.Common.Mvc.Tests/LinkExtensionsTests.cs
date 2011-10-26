using System.Web.Mvc;
using Moq;
using NUnit.Framework;

namespace Curiosity.Common.Mvc
{
    [TestFixture]
    public class LinkExtensionsTests
    {
        private HtmlHelper htmlHelper;
        private ViewContext viewContext;
        private Mock<IViewDataContainer> viewDataContainer;

        [SetUp]
        public void BeforeEachTest()
        {
            viewContext = new ViewContext();
            viewDataContainer = new Mock<IViewDataContainer> { DefaultValue = DefaultValue.Mock };

            htmlHelper = new HtmlHelper(viewContext, viewDataContainer.Object);
        }

        [Test]
        public void ActionLinkIf_GivenAction_WhenFalse_ReturnsExpectedText()
        {
            var actionLink = htmlHelper.ActionLinkIf(false, "My Link", "Test").ToString();
            Assert.AreEqual("My Link", actionLink);
        }

        [Test]
        public void ActionLinkIf_GivenAction_WhenTrue_ReturnsExpectedLink()
        {
            var actionLink = htmlHelper.ActionLinkIf(true, "My Link", "Test").ToString();
            Assert.AreEqual("<a href=\"\">My Link</a>", actionLink);
        }
    }
}
