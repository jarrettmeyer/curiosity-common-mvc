using System;

namespace Curiosity.Common.Mvc
{
    /// <summary>
    /// Form handler.
    /// </summary>
    public interface IFormHandler
    {
        /// <summary>
        /// Returns true if the <see cref="IFormHandler"/> instance can handle the given form.
        /// </summary>
        /// <param name="type">Type of form to be handled</param>        
        bool CanHandle(Type type);

        /// <summary>
        /// Handle the given form.
        /// </summary>
        /// <param name="form">Form to be handled</param>
        void Handle(object form);
    }

    /// <summary>
    /// Strongly typed form handler for forms of type FORM.
    /// </summary>
    /// <typeparam name="FORM">Type of form</typeparam>
    public interface IFormHandler<in FORM> : IFormHandler
    {
        /// <summary>
        /// Handle the given form.
        /// </summary>
        /// <param name="form">Form to be handled</param>
        void Handle(FORM form);
    }
}
