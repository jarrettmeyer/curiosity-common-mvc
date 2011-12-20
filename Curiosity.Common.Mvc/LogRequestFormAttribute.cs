using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;
using Curiosity.Common.Messaging;

namespace Curiosity.Common.Mvc
{
    public class LogRequestFormAttribute : ActionFilterAttribute
    {
        private static readonly string[] defaultProtectedFields = new[] { "CreditCard", "CreditCardNumber", "NewPassword", "NewPasswordConfirmation", "Password", "PasswordConfirmation" };
        private static Func<string> getConfigProtectedFormFieldsMethod = () => ConfigurationManager.AppSettings["ProtectedFormFields"];
        private static readonly object lockObject = new object();
        private static volatile List<string> protectedFields;

        static LogRequestFormAttribute()
        {
            Bus.Instance.AddMessageHandlerType(typeof(DebugLogRequestFormMessageObserver));
        }

        /// <summary>
        /// Allows for testing the configuration endpoint.
        /// </summary>
        internal static Func<string> GetConfigProtectedFormFieldsMethod
        {
            get { return getConfigProtectedFormFieldsMethod; }
            set { getConfigProtectedFormFieldsMethod = value; }
        }

        /// <summary>
        /// Gets the list of protected form fields.
        /// </summary>
        public static IEnumerable<string> ProtectedFormFields
        {
            get
            {
                InitializeProtectedFieldsCollection();
                return protectedFields;
            }
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Bus.Instance.Send(new LogRequestFormMessage(filterContext));
        }

        private static void InitializeProtectedFieldsCollection()
        {
            if (protectedFields == null)
            {
                lock (lockObject)
                {
                    if (protectedFields == null)
                    {
                        var innerList = new List<string>();
                        innerList.AddRange(defaultProtectedFields);
                        innerList.AddRange(GetConfigProtectedFormFields());
                        protectedFields = innerList.Distinct().ToList();
                    }
                }
            }
        }

        private static IEnumerable<string> GetConfigProtectedFormFields()
        {
            string protectedFormFieldsString = getConfigProtectedFormFieldsMethod();
            Debug.WriteLine("Configured protected form fields: " + protectedFormFieldsString);

            if (string.IsNullOrEmpty(protectedFormFieldsString)) return new string[] { };
            
            var protectedFormFields = protectedFormFieldsString
                .Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim());
            return protectedFormFields;
        }
    }
}
