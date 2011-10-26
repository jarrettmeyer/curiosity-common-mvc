using System.Web.Mvc;
using Moq;
using NUnit.Framework;

namespace Curiosity.Common.Mvc
{
    [TestFixture]
    public class HtmlHelperExtensionsTests
    {
        const string UNIX_TEST_STRING = "This is\n\n\nmy test string. It has a triple newline,\n\na double newline,\nand a single newline.";
        const string WINDOWS_TEST_STRING = "This is\r\n\r\n\r\nmy test string. It has a triple newline,\r\n\r\na double newline,\r\nand a single newline.";

        private HtmlHelper htmlHelper;
        private Mock<IViewDataContainer> viewDataContainer;

        [SetUp]
        public void SetUp()
        {
            var viewContext = new ViewContext();
            viewDataContainer = new Mock<IViewDataContainer>();
            htmlHelper = new HtmlHelper(viewContext, viewDataContainer.Object);
        }

        [Test]
        public void SimpleFormat_ConvertsMultipleNewlinesToDoubleBreak()
        {            
            var result = htmlHelper.SimpleFormat(WINDOWS_TEST_STRING).ToString();
            StringAssert.Contains("<br/><br/>", result);            
        }

        [Test]
        public void SimpleFormat_HasNoNewlines()
        {
            var result = htmlHelper.SimpleFormat(WINDOWS_TEST_STRING).ToString();
            StringAssert.DoesNotContain("\r\n", result);
        }

        [Test]
        public void SimpleFormat_HasNoUnixStyleNewlines()
        {
            var result = htmlHelper.SimpleFormat(UNIX_TEST_STRING).ToString();
            StringAssert.DoesNotContain("\n", result);
        }
    }
}
