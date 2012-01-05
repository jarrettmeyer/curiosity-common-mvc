using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Curiosity.Common.Collections;

namespace Curiosity.Common.Mvc
{
    public static class FlashExtensions
    {
        public static string flashCssClass = Config.FlashCssClass;

        public static void Flash(this TempDataDictionary tempData, object messages)
        {
            IDictionary<string, string> flashMessageDictionary = messages.ToDictionary();
            var flashStorage = new FlashStorage(tempData);
            flashMessageDictionary.ForEach(kvp => flashStorage.Add(kvp.Key, kvp.Value));
        }

        public static IHtmlString Flash(this HtmlHelper htmlHelper, string container = "div")
        {
            var messages = new FlashStorage(htmlHelper.ViewContext.TempData).Messages.ToList();
            var tags = messages.Select(kvp => BuildFlashHtmlTag(container, kvp.Key, kvp.Value));
            var combinedTags = string.Join(Environment.NewLine, tags);
            return htmlHelper.Raw(combinedTags);
        }

        private static string BuildFlashHtmlTag(string container, string type, string message)
        {
            var cssClas = BuildFlashCssClass(type);
            var tagBuilder = new TagBuilder(container);
            tagBuilder.MergeAttribute("class", cssClas);
            tagBuilder.InnerHtml = message;
            return tagBuilder.ToString(TagRenderMode.Normal);
        }

        private static string BuildFlashCssClass(string type)
        {
            var @class = new StringBuilder()
                .Append(flashCssClass)
                .Append(" ")
                .Append(type)
                .ToString();
            return @class;
        }
    }
}
