using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using Curiosity.Common.Messaging;

namespace Curiosity.Common.Mvc
{
    public class LogRequestFormAttribute : ActionFilterAttribute
    {
        private static readonly string[] defaultProtectedFields = new[] { "CreditCard", "CreditCardNumber", "NewPassword", "NewPasswordConfirmation", "Password", "PasswordConfirmation" };
        private static string[] protectedFields;

        static LogRequestFormAttribute()
        {
            Bus.Instance.AddMessageHandlerType(typeof(DebugLogRequestFormMessageObserver));
        }

        /// <summary>
        /// Gets the list of protected form fields.
        /// </summary>
        public static IEnumerable<string> ProtectedFormFields
        {
            get { return protectedFields ?? (protectedFields = ConfigProtectedFormFields.Union(defaultProtectedFields).Distinct().ToArray()); }
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Bus.Instance.Send(new LogRequestFormMessage(filterContext));
        }

        private static IEnumerable<string> ConfigProtectedFormFields
        {
            get
            {
                string protectedFormFieldsString = ConfigurationManager.AppSettings["ProtectedFormFields"];
                if (string.IsNullOrEmpty(protectedFormFieldsString)) return new string[] { };
                var protectedFormFields = protectedFormFieldsString
                    .Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)                    
                    .Select(s => s.Trim());
                return protectedFormFields;
            }
        }
    }
}
