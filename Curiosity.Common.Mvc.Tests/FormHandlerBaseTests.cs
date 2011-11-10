using NUnit.Framework;

namespace Curiosity.Common.Mvc
{
    public class FormHandlerBaseTests
    {
        [TestFixture]
        public class When_Checking_CanHandle
        {
            [Test]
            public void Returns_True_When_Type_Is_Valid()
            {
                var formHandler = new TestFormHandler();
                var result = formHandler.CanHandle(typeof(TestForm));
                Assert.IsTrue(result);
            }

            [Test]
            public void Returns_False_When_Type_Is_Not_Valid()
            {
                var formHandler = new TestFormHandler();
                var result = formHandler.CanHandle(typeof(OtherTestForm));
                Assert.IsFalse(result);
            }
        }

        [TestFixture]
        public class When_Calling_Handle_With_Object
        {
            private object form;
            private IFormHandler<TestForm> formHandler;

            [SetUp]
            public void BeforeEachTest()
            {
                form = new TestForm();
                formHandler = new TestFormHandler();
            }

            [Test]
            public void Can_Call_With_Correct_Form()
            {                
                Assert.DoesNotThrow(() => formHandler.Handle(form));
            }

            [Test]
            public void Does_Not_Throw_Exception_When_Form_Is_Null()
            {
                form = null;
                Assert.DoesNotThrow(() => formHandler.Handle(form));
            }

            [Test]
            public void Does_Not_Throw_Exception_When_Form_Is_Wrong_Type()
            {
                form = new OtherTestForm();
                Assert.DoesNotThrow(() => formHandler.Handle(form));
            }
        }

        internal class TestForm  { }

        internal class OtherTestForm { }

        internal class TestFormHandler : FormHandlerBase<TestForm>
        {
            public bool IsHandled { get; private set; }

            public override void Handle(TestForm form)
            {
                IsHandled = true;
            }
        }
    }
}
