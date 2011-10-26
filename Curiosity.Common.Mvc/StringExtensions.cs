using System.Web;

namespace Curiosity.Common.Mvc
{
    public static class StringExtensions
    {
        /// <summary>
        /// Cast the given string to an <see cref="IHtmlString"/>. Returns an empty
        /// string if the source string is null.
        /// </summary>
        public static IHtmlString ToHtmlString(this string source)
        {            
            return new HtmlString(source ?? "");
        }
    }
}
