using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Curiosity.Common.Messaging;

namespace Curiosity.Common.Mvc
{
    public class LogRequestFormMessage : EventMessageBase
    {
        private readonly ControllerContext context;
        
        public LogRequestFormMessage(ControllerContext context)
        {
            context.ThrowIfArgumentIsNull("context");
            this.context = context;            
        }

        /// <summary>
        /// Gets the action name for the request context.
        /// </summary>
        public string ActionName
        {
            get { return GetRouteValue("action"); }
        }

        /// <summary>
        /// Gets the controller name for the request context.
        /// </summary>
        public string ControllerName
        {
            get { return GetRouteValue("controller"); }
        }

        /// <summary>
        /// Gets the form data submitted with the request.
        /// </summary>
        public NameValueCollection FormData
        {
            get { return context.HttpContext.Request.Form; }
        }

        /// <summary>
        /// Returns true if there is form data submitted with the request.
        /// </summary>
        public bool HasFormData
        {
            get { return FormData != null && FormData.Count > 0; }
        }

        /// <summary>
        /// Gets a collection of form fields that should not be logged.
        /// </summary>
        public IEnumerable<string> ProtectedFormFields
        {
            get { return LogRequestFormAttribute.ProtectedFormFields; }
        }

        /// <summary>
        /// Gets the url submitted.
        /// </summary>
        public string Url
        {
            get { return context.HttpContext.Request.RawUrl; }
        }

        /// <summary>
        /// Is the given field protected?
        /// </summary>
        public bool IsProtected(string key)
        {
            return ProtectedFormFields.Contains(key);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine(string.Format("Url: {0}, Controller: {1}, Action: {2}", Url, ControllerName, ActionName));
            var keyStrings = new List<string>();
            foreach (string key in FormData.AllKeys)
            {
                var value = GetFormValue(key);
                var keyString = string.Format("{0}: {1}", key, value);
                keyStrings.Add(keyString);
            }
            sb.AppendLine(string.Join(", ", keyStrings));
            return sb.ToString();
        }

        internal string GetFormValue(string key)
        {
            string value;
            if (IsProtected(key))
            {
                var valueLength = (FormData[key] ?? "").Length;
                value = new string('*', valueLength);                
            }
            else
            {
                value = FormData[key] ?? "";
            }            
            return value;                        
        }

        private string GetRouteValue(string key)
        {
            return context.RequestContext.RouteData.Values[key].ToString();
        }        
    }
}
