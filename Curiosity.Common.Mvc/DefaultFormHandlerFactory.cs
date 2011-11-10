using System;
using System.Collections.Generic;
using System.Linq;
using Curiosity.Common.Reflection;

namespace Curiosity.Common.Mvc
{
    public class DefaultFormHandlerFactory : List<Type>, IFormHandlerFactory
    {
        public new void Add(Type type)
        {
            bool isFormHandler = type.IsImplementationOf(typeof(IFormHandler));
            if (!isFormHandler)
            {
                throw new ArgumentException(string.Format("Unable to add type '{0}' because it is not a form handler implementation.", type));
            }
            base.Add(type);
        }

        /// <summary>
        /// Create a new instance of the requested form handler.
        /// </summary>
        /// <param name="type">Type of form handler to create</param>
        public IFormHandler Create(Type type)
        {
            var typeToInstance = GetImplementationOf(type);
            return (IFormHandler)Activator.CreateInstance(typeToInstance);
        }

        private Type GetImplementationOf(Type type)
        {
            var handlerType = typeof(IFormHandler<>).MakeGenericType(type);
            var matchingType = this.Single(t => !t.IsInterface && !t.IsAbstract && t.IsImplementationOf(handlerType));
            return matchingType;
        }
    }
}
