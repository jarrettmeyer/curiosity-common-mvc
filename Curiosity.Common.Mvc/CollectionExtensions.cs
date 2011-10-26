using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Curiosity.Common.Mvc
{
    public static class CollectionExtensions
    {
        public static IEnumerable<SelectListItem> ToSelectList<T>(this IEnumerable<T> collection, Func<T, string> value, Func<T, string> text)
        {
            return collection == null 
                ? new List<SelectListItem>() 
                : collection.Select(item => new SelectListItem { Value = value(item), Text = text(item) });
        }

        public static IEnumerable<SelectListItem> ToSelectList<T>(this IEnumerable<T> collection, Func<T, object> value, Func<T, object> text)
        {
            return collection == null
                       ? new List<SelectListItem>()
                       : collection.Select(item => new SelectListItem { Value = value(item).ToString(), Text = text(item).ToString() });
        }

        public static IEnumerable<SelectListItem> ToSelectList<T>(this IEnumerable<T> collection, Func<T, object> value, Func<T, object> text, object selectedValue)
        {
            return collection == null
                       ? new List<SelectListItem>()
                       : collection.Select(item => new SelectListItem { Value = value(item).ToString(), Text = text(item).ToString(), Selected = (value(item).Equals(selectedValue)) });
        }
    }
}
