using System;
using System.Collections.Generic;

namespace Curiosity.Common.Mvc
{
    public interface IFormHandlerBus : IEnumerable<Type>
    {
        /// <summary>
        /// Gets the number of supported types.
        /// </summary>
        int NumberOfSupportedTypes { get; }

        /// <summary>
        /// Add a form handler type to the form handler bus.
        /// </summary>
        /// <param name="formHandlerType">Type of form handler</param>
        void AddFormHandlerType(Type formHandlerType);

        /// <summary>
        /// Clear all form handlers from the bus.
        /// </summary>
        void ClearFormHandlerTypes();

        /// <summary>
        /// Returns true if the form handler bus instance contains the given form handler type.
        /// </summary>
        /// <param name="formHandlerType">Type of form handler</param>        
        bool ContainsFormHandler(Type formHandlerType);

        /// <summary>
        /// Send a form to the bus for processing.
        /// </summary>
        /// <param name="form">Form</param>
        void SendForm(object form);

        /// <summary>
        /// Set the form handler factory to be used for creating form handlers.
        /// </summary>
        /// <param name="factory">Form handler factory</param>
        void SetFormHandlerFactory(IFormHandlerFactory factory);
    }
}
