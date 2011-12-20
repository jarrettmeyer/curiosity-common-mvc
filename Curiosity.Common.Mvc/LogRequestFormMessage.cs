using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Curiosity.Common.Messaging;

namespace Curiosity.Common.Mvc
{
    public class LogRequestFormMessage : EventMessageBase, IEnumerable<KeyValuePair<string, string>>
    {
        private readonly ControllerContext context;
        
        public LogRequestFormMessage(ControllerContext context)
        {
            context.ThrowIfArgumentIsNull("context", "Unable to create log request form message. No controller context is given.");
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
        private NameValueCollection FormData
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

        public string[] Keys
        {
            get { return FormData.AllKeys; }
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

        public string this[string key]
        {
            get { return GetFormValue(key); }
        }

        /// <summary>
        /// Is the given field protected?
        /// </summary>
        public bool IsProtected(string key)
        {
            return ProtectedFormFields.Contains(key);
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            foreach (string key in Keys)
            {
                yield return new KeyValuePair<string, string>(key, this[key]);
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine(string.Format("Url: {0}, Controller: {1}, Action: {2}", Url, ControllerName, ActionName));
            string formDataString = BuildFormDataString();
            sb.AppendLine(formDataString);
            return sb.ToString();
        }

        private string BuildFormDataString()
        {
            var formDataStringParts = new List<string>();
            foreach (string key in Keys)
            {
                var value = this[key];
                var stringPart = string.Format("{0}: {1}", key, value);
                formDataStringParts.Add(stringPart);
            }            
            return string.Join(", ", formDataStringParts);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private string GetFormValue(string key)
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
