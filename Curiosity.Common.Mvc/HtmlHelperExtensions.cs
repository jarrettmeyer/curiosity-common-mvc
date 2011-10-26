using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace Curiosity.Common.Mvc
{
    /// <summary>
    /// HTML-based extenstion methods.
    /// </summary>
    public static class HtmlHelperExtensions
    {
        /// <summary>
        /// Replace newlines with breaks and paragraphs.
        /// </summary>
        public static IHtmlString SimpleFormat(this HtmlHelper htmlHelper, string source)
        {
            source = source ?? "";

            // Clean up Windows-style CRLF.
            source = Regex.Replace(source, "(\r\n){2,}", "<br/><br/>");
            source = Regex.Replace(source, "\r\n", "<br/>");

            // Clean up Unix-style LF.
            source = Regex.Replace(source, "\n{2,}", "<br/><br/>");            
            source = Regex.Replace(source, "\n", "<br/>");

            return source.ToHtmlString();
        }

        /// <summary>
        /// Returns a new <see cref="UrlHelper"/> for the source <see cref="HtmlHelper"/>.
        /// </summary>
        internal static UrlHelper UrlHelper(this HtmlHelper html)
        {
            var requestContext = html.ViewContext.RequestContext;
            return new UrlHelper(requestContext);
        }
    }
}
