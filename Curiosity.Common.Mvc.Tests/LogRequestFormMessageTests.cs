using System.Collections.Specialized;
using System.Configuration;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Curiosity.Common.Messaging;
using Moq;
using NUnit.Framework;

namespace Curiosity.Common.Mvc
{
    [TestFixture]
    public class LogRequestFormMessageTests
    {
        private ControllerContext context;
        private NameValueCollection formData;
        private Mock<HttpContextBase> httpContext;
        private LogRequestFormMessage message;
        private Mock<HttpRequestBase> request;

        public class TypeTests
        {
            [Test]
            public void should_inherit_from_MessageBase()
            {
                var type = typeof(LogRequestFormMessage);
                var superType = typeof(MessageBase);
                bool result = type.IsSubclassOf(superType);
                Assert.IsTrue(result);
            }
        }

            
        [SetUp]
        public void before_each_test()
        {
            formData = new NameValueCollection();

            request = new Mock<HttpRequestBase> { DefaultValue = DefaultValue.Mock };
            request.SetupGet(x => x.RawUrl).Returns("http://www.example.com");
            request.SetupGet(x => x.Form).Returns(formData);

            httpContext = new Mock<HttpContextBase> { DefaultValue = DefaultValue.Mock};
            httpContext.SetupGet(x => x.Request).Returns(request.Object);

            var routeData = new RouteData();
            routeData.Values.Add("controller", "Home");
            routeData.Values.Add("action", "Index");

            context = new ControllerContext();
            context.HttpContext = httpContext.Object;
            context.RequestContext = new RequestContext(httpContext.Object, routeData);

            message = new LogRequestFormMessage(context);
        }

        [Test]
        public void can_configure_protected_fields()
        {
            LogRequestFormAttribute.GetConfigProtectedFormFieldsMethod = () => "MyProtectedField";
            bool result = message.IsProtected("MyProtectedField");
            Assert.IsTrue(result);
        }

        [Test]
        public void can_get_url_from_request()
        {            
            Assert.AreEqual("http://www.example.com", message.Url);
        }

        [Test]
        public void can_get_controller_name()
        {
            Assert.AreEqual("Home", message.ControllerName);
        }

        [Test]
        public void can_get_action_name()
        {
            Assert.AreEqual("Index", message.ActionName);
        }

        [Test]
        [TestCase("CreditCard", Result = true)]
        [TestCase("CreditCardNumber", Result = true)]
        [TestCase("NewPassword", Result = true)]
        [TestCase("NewPasswordConfirmation", Result = true)]
        [TestCase("Password", Result = true)]
        [TestCase("PasswordConfirmation", Result = true)]
        public bool default_fields_are_always_protected(string key)
        {
            return message.IsProtected(key);
        }

        [Test]
        public void HasFormData_returns_false_when_no_form_data_is_available()
        {
            Assert.IsFalse(message.HasFormData);
        }

        [Test]
        public void HasFormData_returns_true_when_form_data_is_present()
        {
            formData.Add("FirstName", "John");
            Assert.IsTrue(message.HasFormData);
        }

        [Test]
        public void protected_keys_should_be_protected()
        {
            Assert.IsTrue(message.IsProtected("Password"));
        }

        [Test]
        public void unprotected_keys_should_not_be_protected()
        {
            Assert.IsFalse(message.IsProtected("FirstName"));
        }
    }
}
