using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using NUnit.Framework;

namespace Curiosity.Common.Mvc
{
    [TestFixture]
    public class FormHandlerResultTests
    {
        private ControllerContext context;
        private TestForm form;
        private FormHandlerResult<TestForm> formHandlerResult;
        private TestResult success;
        private TestResult failure;

        [SetUp]
        public virtual void BeforeEachTest()
        {
            form = new TestForm();
            success = new TestResult();
            failure = new TestResult();

            formHandlerResult = new FormHandlerResult<TestForm>(form, () => success, () => failure);

            var requestContext = new RequestContext();
            var controller = new TestController();
            context = new ControllerContext(requestContext, controller);

            FormHandlerBus.Instance.ClearFormHandlerTypes();
        }

        [Test]
        public void Throws_Exception_When_Creating_With_Null_Form()
        {
            formHandlerResult = new FormHandlerResult<TestForm>(form, () => success, () => failure);
        }

        [Test]
        public void Sets_Success_To_EmptyResult_When_Given_Null_Value()
        {
            formHandlerResult = new FormHandlerResult<TestForm>(form, null, () => failure);
            Assert.IsNotNull(formHandlerResult.Success);
            Assert.IsInstanceOf<EmptyResult>(formHandlerResult.Success.Invoke());
        }

        [Test]
        public void Sets_Failure_To_EmptyResult_When_Given_Null_Value()
        {
            formHandlerResult = new FormHandlerResult<TestForm>(form, () => success, null);
            Assert.IsNotNull(formHandlerResult.Failure);
            Assert.IsInstanceOf<EmptyResult>(formHandlerResult.Failure.Invoke());
        }

        [Test]
        public void Should_Execute_Failure_When_Model_State_Is_Invalid()
        {
            context.Controller.ViewData.ModelState.AddModelError("*", "Something is wrong!");
            formHandlerResult.ExecuteResult(context);
            Assert.IsTrue(failure.IsExecuted);
        }

        [Test]
        public void Should_Execute_Success_Result()
        {
            formHandlerResult.ExecuteResult(context);
            Assert.IsTrue(success.IsExecuted);
        }
        
        [Test]
        public void Should_Execute_Failure_Result_When_Form_Handler_Throws_Application_Exception()
        {
            FormHandlerBus.Instance.AddFormHandlerType(typeof(ApplicationExceptionThrowingFormHandler));
            formHandlerResult.ExecuteResult(context);
            Assert.IsTrue(failure.IsExecuted);
        }

        [Test]
        public void Should_Execute_Failure_Result_When_Form_Handler_Throws_Exception()
        {
            FormHandlerBus.Instance.AddFormHandlerType(typeof(ExceptionThrowingFormHandler));
            formHandlerResult.ExecuteResult(context);
            Assert.IsTrue(failure.IsExecuted);
        }

        [Test]
        public void Should_Add_Error_Message_To_Flash_When_Form_Handler_Throws_Exception()
        {
            FormHandlerBus.Instance.AddFormHandlerType(typeof(ExceptionThrowingFormHandler));
            formHandlerResult.ExecuteResult(context);
            var flashTempData = context.Controller.TempData[typeof(FlashStorage).FullName];
            var flash = (IEnumerable<KeyValuePair<string, string>>)flashTempData;
            Assert.IsTrue(flash.Any(m => m.Key == "error"));
        }

        [Test]
        public void Should_Add_Warning_Message_To_Flash_When_Form_Handler_Throws_Application_Exception()
        {
            FormHandlerBus.Instance.AddFormHandlerType(typeof(ApplicationExceptionThrowingFormHandler));
            formHandlerResult.ExecuteResult(context);
            var flashTempData = context.Controller.TempData[typeof(FlashStorage).FullName];
            var flash = (IEnumerable<KeyValuePair<string, string>>)flashTempData;
            Assert.IsTrue(flash.Any(m => m.Key == "warning"));
        }

        [Test]
        public void Should_Add_Warning_Message_To_Flash_When_Form_Is_Invalid()
        {
            context.Controller.ViewData.ModelState.AddModelError("*", "Something bad happened!");
            formHandlerResult.ExecuteResult(context);
            var flashTempData = context.Controller.TempData[typeof(FlashStorage).FullName];
            var flash = (IEnumerable<KeyValuePair<string, string>>)flashTempData;
            Assert.IsTrue(flash.Any(m => m.Key == "warning"));
        }

        public class TestForm { }

        public class TestFormHandler : FormHandlerBase<TestForm>
        {
            public override void Handle(TestForm form)
            {
            }
        }

        public class ApplicationExceptionThrowingFormHandler : FormHandlerBase<TestForm>
        {
            public override void Handle(TestForm form)
            {
                throw new ApplicationException();
            }
        }

        public class ExceptionThrowingFormHandler : FormHandlerBase<TestForm>
        {
            public override void Handle(TestForm form)
            {
                throw new Exception();
            }
        }

        public class TestResult : ActionResult
        {
            public bool IsExecuted { get; private set; }

            public override void ExecuteResult(ControllerContext context)
            {
                IsExecuted = true;
            }
        }

        public class TestController : Controller { }
    }
}
