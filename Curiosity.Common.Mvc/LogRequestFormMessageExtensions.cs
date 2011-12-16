using System.Xml.Linq;

namespace Curiosity.Common.Mvc
{
    public static class LogRequestFormMessageExtensions
    {
        /// <summary>
        /// Prints the form data message as XML.
        /// </summary>
        public static XElement ToXml(this LogRequestFormMessage message)
        {
            var root = new XElement("LogRequestFormMessage");
            root.Add(new XElement("Url", message.Url));
            root.Add(new XElement("Controller", message.ControllerName));
            root.Add(new XElement("Action", message.ActionName));
            var formData = new XElement("FormData");
            foreach (var key in message.FormData.AllKeys)
            {
                var value = message.GetFormValue(key);
                formData.Add(new XElement(key, value));
            }
            root.Add(formData);
            return root;
        }
    }
}
