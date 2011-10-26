using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using NUnit.Framework;

namespace Curiosity.Common.Mvc
{
    [TestFixture]
    public class LowercaseRouteTests
    {
        private RequestContext requestContext;

        [SetUp]
        public void BeforeEachTest()
        {
            SetUpRequestContext();
        }

        private void SetUpRequestContext()
        {
            HttpTest.SetUpHttpContext();
            requestContext = new RequestContext();
            requestContext.HttpContext = new HttpContextWrapper(HttpContext.Current);
            requestContext.RouteData = new RouteData();
        }

        [Test]
        public void GetVirtualPath_WhenGivenUppercase_ReturnsLowercase()
        {
            var lowercaseRoute = new LowercaseRoute("{controller}/{action}", new MvcRouteHandler());

            var routeValues = new RouteValueDictionary(new { controller = "MyTest", action = "Index" });
            var path = lowercaseRoute.GetVirtualPath(requestContext, routeValues);

            Assert.IsNotNull(path);            
            Assert.AreEqual("mytest/index", path.VirtualPath);
        }
    }
}
