using System;
using System.Collections.Generic;

namespace Curiosity.Common.Mvc
{
    public interface IFormHandlerFactory : IList<Type>
    {
        /// <summary>
        /// Create a new instance of the requested form handler.
        /// </summary>
        /// <param name="type">Type of form handler to create</param>        
        IFormHandler Create(Type type);
    }
}
