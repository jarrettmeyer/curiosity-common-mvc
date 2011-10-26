using System.Web.Mvc;
using Moq;
using NUnit.Framework;

namespace Curiosity.Common.Mvc
{
    [TestFixture]
    public class FlashExtensionsTests
    {
        private FlashStorage flashStorage;
        private HtmlHelper htmlHelper;
        private TempDataDictionary tempData;
        private ViewContext viewContext;
        private Mock<IViewDataContainer> viewDataContainer;
        

        [SetUp]
        public void SetUp()
        {
            tempData = new TempDataDictionary();
            viewContext = new ViewContext();
            viewContext.TempData = tempData;
            viewDataContainer = new Mock<IViewDataContainer>();            
            htmlHelper = new HtmlHelper(viewContext, viewDataContainer.Object);
            flashStorage = new FlashStorage(tempData);
            
        }

        [Test]
        public void CanContainUrl()
        {
            flashStorage.Add("success", @"This contains a <a href=""http://www.google.com"">link</a>");
            var htmlString = htmlHelper.Flash().ToHtmlString();

            StringAssert.Contains(@"<a href=""http://www.google.com"">link</a>", htmlString);
        }

        [Test]
        public void FlashToHtml_HasClassApplied()
        {
            flashStorage.Add("success", "This is a test message.");
            var htmlString = htmlHelper.Flash().ToHtmlString();

            StringAssert.Contains("flash success", htmlString);            
        }

        [Test]
        public void FlashToHtml_WrapsWithContainerObject()
        {
            flashStorage.Add("success", "This is a yet another test message");
            var htmlString = htmlHelper.Flash("p").ToHtmlString();
            
            StringAssert.StartsWith("<p", htmlString);
            StringAssert.EndsWith("</p>", htmlString);
        }
    }
}
