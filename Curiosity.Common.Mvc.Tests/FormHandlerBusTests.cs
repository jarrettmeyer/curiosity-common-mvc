using System;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework;

namespace Curiosity.Common.Mvc
{
    [TestFixture]
    public class FormHandlerBusTests
    {
        [SetUp]
        public void BeforeEachTest()
        {
            FormHandlerBus.Instance = null;
        }

        [Test]
        public void AddFormHandlerType_Will_Not_Duplicate_Additions()
        {
            Assert.IsTrue(FormHandlerBus.Instance.ContainsFormHandler(typeof(SampleFormHandler)));
            int initialCount = FormHandlerBus.Instance.NumberOfSupportedTypes;
            FormHandlerBus.Instance.AddFormHandlerType(typeof(SampleFormHandler));
            Assert.AreEqual(initialCount, FormHandlerBus.Instance.NumberOfSupportedTypes);
        }

        [Test]
        public void Bus_Should_Have_Expected_Instance()
        {
            var result = FormHandlerBus.Instance.ContainsFormHandler(typeof(SampleFormHandler));
            Assert.IsTrue(result);
        }

        [Test]
        public void Instance_Can_Be_Set()
        {
            var factory = new DefaultFormHandlerFactory();
            var bus = new FormHandlerBus(factory);
            FormHandlerBus.Instance = bus;

            Assert.AreSame(bus, FormHandlerBus.Instance);
        }

        [Test]
        public void Instance_Should_Be_A_Bus()
        {
            var instance = FormHandlerBus.Instance;
            Assert.IsNotNull(instance);
            Assert.IsInstanceOf<IFormHandlerBus>(instance);
        }

        [Test]
        public void NumberOfSupportedTypes_Returns_Count()
        {
            int count = FormHandlerBus.Instance.NumberOfSupportedTypes;
            Debug.WriteLine("Number of supported types: " + count);
            Assert.Greater(count, 0);
        }

        [Test]
        public void Set_Instance_Contains_No_Types()
        {
            var factory = new DefaultFormHandlerFactory();
            var bus = new FormHandlerBus(factory);
            FormHandlerBus.Instance = bus;

            Assert.AreEqual(0, FormHandlerBus.Instance.NumberOfSupportedTypes);
        }

        [Test]
        public void Should_Throw_Exception_When_Adding_Non_Form_Handler_Type()
        {
            Assert.Throws<ArgumentException>(() => FormHandlerBus.Instance.AddFormHandlerType(typeof(object)));
        }

        [Test]
        public void Should_Throw_Exception_When_Adding_Null_Form_Handler_Type()
        {
            Assert.Throws<ArgumentNullException>(() => FormHandlerBus.Instance.AddFormHandlerType(null));
        }

        public class SampleForm { }

        public class SampleFormHandler : FormHandlerBase<SampleForm>
        {
            public override void Handle(SampleForm form)
            {                
            }
        }
    }
}
