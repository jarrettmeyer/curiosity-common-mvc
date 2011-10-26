using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Curiosity.Common.Mvc
{
    /// <summary>
    /// Methods to add links to pages.
    /// </summary>
    public static class LinkExtenstions
    {
        /// <summary>
        /// Returns a link if the given condition is method. Otherwise, returns text.
        /// </summary>
        public static IHtmlString ActionLinkIf(this HtmlHelper html, bool condition, string linkText, string actionName)
        {            
            return condition ? html.ActionLink(linkText, actionName) : linkText.ToHtmlString();
        }

        /// <summary>
        /// Returns a link if the given condition is method. Otherwise, returns text.
        /// </summary>
        public static IHtmlString ActionLinkIf(this HtmlHelper html, bool condition, string linkText, string actionName, object routeValues)
        {
            return condition ? html.ActionLink(linkText, actionName, routeValues) : linkText.ToHtmlString();
        }

        /// <summary>
        /// Returns a link if the given condition is method. Otherwise, returns text.
        /// </summary>
        public static IHtmlString ActionLinkIf(this HtmlHelper html, bool condition, string linkText, string actionName, object routeValues, object htmlAttributes)
        {
            return condition ? html.ActionLink(linkText, actionName, routeValues, htmlAttributes) : linkText.ToHtmlString();
        }

        /// <summary>
        /// Returns a link if the given condition is method. Otherwise, returns text.
        /// </summary>
        public static IHtmlString ActionLinkIf(this HtmlHelper html, bool condition, string linkText, string actionName, string controllerName)
        {
            return condition ? html.ActionLink(linkText, actionName, controllerName) : linkText.ToHtmlString();
        }

        /// <summary>
        /// Returns a link if the given condition is method. Otherwise, returns text.
        /// </summary>
        public static IHtmlString ActionLinkIf(this HtmlHelper html, bool condition, string linkText, string actionName, string controllerName, object routeValues, object htmlAttributes)
        {
            return condition ? html.ActionLink(linkText, actionName, controllerName, routeValues, htmlAttributes) : linkText.ToHtmlString();
        }

        public static IHtmlString ButtonTo(this HtmlHelper html, string buttonText, string actionName)
        {
            var urlHelper = html.UrlHelper();
            string url = urlHelper.Action(actionName);
            var button = BuildButtonHtmlTag(buttonText, url);
            return button.ToHtmlString();
        }

        public static IHtmlString ButtonTo(this HtmlHelper html, string buttonText, string actionName, object routeValues)
        {
            var urlHelper = html.UrlHelper();
            string url = urlHelper.Action(actionName, routeValues);
            var button = BuildButtonHtmlTag(buttonText, url);
            return button.ToHtmlString();
        }

        public static IHtmlString ButtonTo(this HtmlHelper html, string buttonText, string actionName, string controllerName)
        {
            var urlHelper = html.UrlHelper();
            string url = urlHelper.Action(actionName, controllerName);
            var button = BuildButtonHtmlTag(buttonText, url);
            return button.ToHtmlString();
        }

        public static IHtmlString ButtonTo(this HtmlHelper html, string buttonText, string actionName, string controllerName, object routeValues)
        {
            var urlHelper = html.UrlHelper();
            string url = urlHelper.Action(actionName, controllerName, routeValues);
            var button = BuildButtonHtmlTag(buttonText, url);
            return button.ToHtmlString();
        }

        private static string BuildButtonHtmlTag(string buttonText, string url)
        {
            var buttonTag = new TagBuilder("button");
            buttonTag.MergeAttribute("onclick", string.Format("javascript:window.location='{0}';", url));
            buttonTag.InnerHtml = buttonText;
            string button = buttonTag.ToString(TagRenderMode.Normal);
            return button;
        }
        
    }
}
