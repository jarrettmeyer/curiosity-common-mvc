using System;

namespace Curiosity.Common.Mvc
{
    public interface IFormHandlerFactory
    {
        /// <summary>
        /// Create a new instance of the requested form handler.
        /// </summary>
        /// <param name="formHandlerType">Type of form handler to create</param>        
        IFormHandler CreateFormHandler(Type formHandlerType);
    }
}
