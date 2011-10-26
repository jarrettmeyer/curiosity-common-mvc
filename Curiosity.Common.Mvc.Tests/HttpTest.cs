using System;
using System.Collections.Specialized;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.SessionState;
using Moq;

namespace Curiosity.Common.Mvc
{
    internal class HttpTest
    {
        private static HttpContext httpContext;
        private static HttpWorkerRequest httpWorkerRequest;
        private static StringBuilder outputStringBuilder;
        private static TextWriter output;
        private static Mock<HttpRequestBase> mockHttpRequest;
        private static Mock<HttpResponseBase> mockHttpResponse;
        private static Mock<HttpContextBase> mockHttpContext;
        private static NameValueCollection httpHeaders;

        public static Mock<HttpContextBase> MockHttpContext
        {
            get
            {
                if (mockHttpContext == null)
                {
                    mockHttpContext = new Mock<HttpContextBase>();
                    mockHttpContext.Setup(x => x.Request).Returns(MockHttpRequest.Object);
                    mockHttpContext.Setup(x => x.Response).Returns(MockHttpResponse.Object);
                }
                return mockHttpContext;
            }
        }

        public static Mock<HttpRequestBase> MockHttpRequest
        {
            get
            {
                if (mockHttpRequest == null)
                {
                    mockHttpRequest = new Mock<HttpRequestBase>();
                    mockHttpRequest.Setup(x => x.Headers).Returns(httpHeaders ?? (httpHeaders = new NameValueCollection()));
                }
                return mockHttpRequest;
            }
        }

        public static Mock<HttpResponseBase> MockHttpResponse
        {
            get { return mockHttpResponse ?? (mockHttpResponse = new Mock<HttpResponseBase>()); }
        }

        public static string Output
        {
            get { return outputStringBuilder.ToString(); }
        }

        public static void AddAjaxRequestHeader()
        {
            AddHttpHeader("X-Requested-With", "XMLHttpRequest");
        }

        public static void AddHttpHeader(string name, string value)
        {
            if (httpHeaders == null)
            {
                httpHeaders = new NameValueCollection();
            }
            httpHeaders.Add(name, value);
        }

        public static void DestroyHttpContext()
        {
            HttpContext.Current = null;
            mockHttpContext = null;
            mockHttpRequest = null;
            mockHttpResponse = null;
            httpHeaders = null;
        }

        public static void SetUpHttpContext()
        {
            BuildResponseOutput();
            BuildHttpWorkerRequest();
            BuildHttpContext();
            BuildHttpSession();
        }

        public static void SetUpControllerContext(Controller controller)
        {
            RequestContext requestContext = new RequestContext(MockHttpContext.Object, new RouteData());
            ControllerContext controllerContext = new ControllerContext(requestContext, controller);
            controller.ControllerContext = controllerContext;
            controller.Url = new UrlHelper(requestContext);
        }

        private static void BuildHttpContext()
        {
            httpContext = new HttpContext(httpWorkerRequest);
            HttpContext.Current = httpContext;
        }

        private static void BuildHttpSession()
        {
            HttpContext.Current.SetSessionStateBehavior(SessionStateBehavior.Required);
            var sessionStateContainer = new HttpSessionStateContainer(Guid.NewGuid().ToString(), new SessionStateItemCollection(), new HttpStaticObjectsCollection(), 20000, true, HttpCookieMode.UseCookies, SessionStateMode.Off, false);
            SessionStateUtility.AddHttpSessionStateToContext(httpContext, sessionStateContainer);
        }

        private static void BuildHttpWorkerRequest()
        {
            const string appVirtualDir = "/";
            const string appPhysicalDir = @"c:\";
            const string page = "/default.aspx";
            string query = string.Empty;
            httpWorkerRequest = new SimpleWorkerRequest(appVirtualDir, appPhysicalDir, page, query, output);
        }

        private static void BuildResponseOutput()
        {
            outputStringBuilder = new StringBuilder();
            output = new StringWriter(outputStringBuilder);
        }
    }
}
