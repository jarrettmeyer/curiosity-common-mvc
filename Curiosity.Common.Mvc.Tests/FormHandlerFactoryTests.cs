using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Curiosity.Common.Mvc
{
    public class FormHandlerFactoryTests
    {
        [TestFixture]
        public class When_Singleton_Instance_Has_Not_Been_Set
        {
            [Test]
            public void Returns_Default_Result()
            {
                var result = FormHandlerFactory.Instance;
                Assert.IsNotNull(result);
                Assert.IsInstanceOf<IFormHandlerFactory>(result);
            }
        }

        [TestFixture]
        public class When_Singleton_Instance_Has_Been_Set
        {
            [SetUp]
            public void BeforeEachTest()
            {
                FormHandlerFactory.Instance = new TestFormHandlerFactory();
            }

            [TearDown]
            public void AfterEachTest()
            {
                FormHandlerFactory.Instance = null;
            }

            [Test]
            public void Returns_Set_Result()
            {
                var result = FormHandlerFactory.Instance;
                Assert.IsNotNull(result);
                Assert.IsInstanceOf<TestFormHandlerFactory>(result);
            }
        }

        internal class TestFormHandlerFactory : List<Type>, IFormHandlerFactory
        {
            public IFormHandler Create(Type type)
            {
                throw new NotImplementedException();
            }
        }
    }
}
