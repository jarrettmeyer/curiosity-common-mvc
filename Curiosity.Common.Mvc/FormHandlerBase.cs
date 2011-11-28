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
        public abstract void Handle(FORM form);

        [DebuggerStepThrough]
        public virtual bool CanHandle(Type type)
        {
            return type != null && type.Equals(typeof(FORM));
        }

        public virtual void Handle(object form)
        {
            try
            {
                if (form == null)
                {
                    throw new ArgumentNullException("form", string.Format("Unable to handle null form. Was expecting form of type '{0}'.", typeof(FORM).Name));
                }
                Handle((FORM)form);
            }
            catch (ArgumentNullException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            catch (InvalidCastException)
            {
                Debug.WriteLine(string.Format("Unable to cast form of type '{0}'. Expected a form of type '{1}'.", form.GetType(), typeof(FORM)));
            }
        }
    }
}
