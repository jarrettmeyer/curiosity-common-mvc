using System;

namespace Curiosity.Common.Mvc
{
    public class DefaultFormHandlerFactory : IFormHandlerFactory
    {
        /// <summary>
        /// Create a new instance of the requested form handler.
        /// </summary>
        /// <param name="formHandlerType">Type of form handler to create</param>
        public virtual IFormHandler CreateFormHandler(Type formHandlerType)
        {
            if (formHandlerType == null)
            {
                throw new ArgumentNullException("formHandlerType");
            }
            if (formHandlerType.GetInterface(typeof(IFormHandler).Name) == null)
            {
                throw new ArgumentException(string.Format("Requested form handler type {0} does not implement IFormHandler.", formHandlerType.Name), "formHandlerType");
            }
            var instance = Activator.CreateInstance(formHandlerType);
            return (IFormHandler)instance;
        }
    }
}
