using System.Web.Routing;
using NUnit.Framework;

namespace Curiosity.Common.Mvc
{
    [TestFixture]
    public class RouteCollectionExtensionsTests
    {
        [Test]
        public void MapLowercaseRoute_AddsExpectedRoutes()
        {
            RouteCollection routeCollection = new RouteCollection();
            routeCollection.MapLowercaseRoute("TestRoute", "{controller}/{action}", new { controller = "Home", action = "Index|About|Contact" });

            // assert
            Assert.AreEqual(1, routeCollection.Count);
            Assert.IsInstanceOf<LowercaseRoute>(routeCollection[0]);
        }
    }
}
