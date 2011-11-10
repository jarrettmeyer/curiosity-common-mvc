using System;
using System.Web.Mvc;

namespace Curiosity.Common.Mvc
{
    public class DependencyResolverFormHandlerFactory : DefaultFormHandlerFactory
    {
        public override IFormHandler CreateFormHandler(Type formHandlerType)
        {
            var formHandlerInstance = DependencyResolver.Current.GetService(formHandlerType);
            return formHandlerInstance != null
                       ? (IFormHandler)formHandlerInstance
                       : base.CreateFormHandler(formHandlerType);
        }
    }
}
