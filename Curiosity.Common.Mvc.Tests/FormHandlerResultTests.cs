using System;
using System.Web.Mvc;
using System.Web.Routing;
using NUnit.Framework;

namespace Curiosity.Common.Mvc
{
    public class FormHandlerResultTests
    {
        [TestFixture]
        public class When_Passing_Null_Form_To_FormHandlerResult
        {
            [Test]
            public void Should_Throw_Exception()
            {
                TestForm form = null;
                Assert.Throws<ArgumentNullException>(() => new FormHandlerResult<TestForm>(form, () => new EmptyResult(), () => new EmptyResult()));
            }
        }

        [TestFixture]
        public class When_Executing_FormHandlerResult : FormHandlerResult_Test
        {
            [TestFixtureSetUp]
            public void BeforeAllTests()
            {
                FormHandlerFactory.Instance.Add(typeof(TestFormHandler));
            }

            [SetUp]
            public override void BeforeEachTest()
            {
                base.BeforeEachTest();
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
        }

        [TestFixture]
        public class When_Form_Handler_Throws_Application_Exception : FormHandlerResult_Test
        {
            [TestFixtureSetUp]
            public void BeforeAllTests()
            {
                FormHandlerFactory.Instance.Add(typeof(ApplicationExceptionThrowingFormHandler));
            }

            [SetUp]
            public override void BeforeEachTest()
            {
                base.BeforeEachTest();
            }

            [Test]
            public void Should_Execute_Failure_Result()
            {
                formHandlerResult.ExecuteResult(context);
                Assert.IsTrue(failure.IsExecuted);
            }
        }

        [TestFixture]
        public class When_Form_Handler_Throws_Exception : FormHandlerResult_Test
        {
            [TestFixtureSetUp]
            public void BeforeAllTests()
            {
                FormHandlerFactory.Instance.Add(typeof(ExceptionThrowingFormHandler));
            }

            [SetUp]
            public override void BeforeEachTest()
            {
                base.BeforeEachTest();
            }

            [Test]
            public void Should_Execute_Failure_Result()
            {
                formHandlerResult.ExecuteResult(context);
                Assert.IsTrue(failure.IsExecuted);
            }
        }

        public class FormHandlerResult_Test
        {
            protected ControllerContext context;
            protected TestForm form;
            protected FormHandlerResult<TestForm> formHandlerResult;
            protected TestResult success;
            protected TestResult failure;

            public virtual void BeforeEachTest()
            {
                form = new TestForm();
                success = new TestResult();
                failure = new TestResult();

                formHandlerResult = new FormHandlerResult<TestForm>(form, () => success, () => failure);

                var requestContext = new RequestContext();
                var controller = new TestController();
                context = new ControllerContext(requestContext, controller);                
            }
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
