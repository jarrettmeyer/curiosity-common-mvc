using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Curiosity.Common.Mvc
{
    /// <summary>
    /// Defines a container for storing flash messages inside a temp data dictionary.
    /// </summary>
    public class FlashStorage
    {
        private readonly TempDataDictionary backingStorage;
        private static readonly string key = typeof(FlashStorage).FullName;

        public FlashStorage(TempDataDictionary tempData)
        {
            backingStorage = tempData;
        }

        /// <summary>
        /// Get the current flash messages. Reading the flash messages
        /// will also clear the contents of the flash storage.
        /// </summary>
        public IEnumerable<KeyValuePair<string, string>> Messages
        {
            get
            {
                try
                {
                    object value;
                    return backingStorage.TryGetValue(key, out value) 
                        ? (IEnumerable<KeyValuePair<string, string>>)value
                        : new List<KeyValuePair<string, string>>();
                }
                finally
                {
                    backingStorage.Remove(key);
                }
            }
        }

        /// <summary>
        /// Add a flash message.
        /// </summary>
        /// <param name="type">Flash message type.</param>
        /// <param name="message">Flash message text.</param>
        public void Add(string type, string message)
        {
            // Don't add blank strings.
            if (string.IsNullOrWhiteSpace(message)) return;
            
            IList<KeyValuePair<string, string>> messages;
            
            object temp;
            if (!backingStorage.TryGetValue(key, out temp))
            {
                messages = new List<KeyValuePair<string, string>>();
                backingStorage.Add(key, messages);
            }
            else
            {
                messages = (IList<KeyValuePair<string, string>>)temp;
            }

            var item = messages.SingleOrDefault(p => p.Key.Equals(type, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(item.Value))
            {
                messages.Remove(item);
            }

            messages.Add(new KeyValuePair<string, string>(type, message));
            backingStorage.Keep(key);
        }
    }
}
