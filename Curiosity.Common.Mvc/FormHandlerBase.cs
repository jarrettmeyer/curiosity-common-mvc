using System;
using System.Diagnostics;

namespace Curiosity.Common.Mvc
{
    /// <summary>
    /// Base class implementation of an <see cref="IFormHandler{FORM}"/>
    /// </summary>
    /// <typeparam name="FORM">Type of form to handle</typeparam>
    public abstract class FormHandlerBase<FORM> : IFormHandler<FORM>
    {
        /// <summary>
        /// Handle the given form.
        /// </summary>
        public abstract void Handle(FORM form);

        [DebuggerStepThrough]
        public virtual bool CanHandle(Type type)
        {
            return type != null && type.Equals(typeof(FORM));
        }

        /// <summary>
        /// Handle the given form.
        /// </summary>
        public virtual void Handle(object form)
        {
            try
            {
                if (form == null)
                {
                    throw new ArgumentNullException("form", string.Format("Unable to handle null form. Was expecting form of type '{0}'.", typeof(FORM).Name));
                }
                Handle((FORM)form); // strongly-typed implementation should be in concrete classes.
            }
            catch (ArgumentNullException ex)
            {
                Debug.WriteLine(ex.Message);
                throw;
            }
            catch (InvalidCastException)
            {
                string type = (form != null) ? form.GetType().ToString() : "null";
                Debug.WriteLine(string.Format("Unable to cast form of type '{0}'. Expected a form of type '{1}'.", type, typeof(FORM)));
                throw;
            }
        }
    }
}
