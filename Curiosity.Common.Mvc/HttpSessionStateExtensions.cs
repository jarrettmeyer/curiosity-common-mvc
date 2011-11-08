using System;
using System.Diagnostics;
using System.Web.SessionState;

namespace Curiosity.Common.Mvc
{
    public static class HttpSessionStateExtensions
    {
        public static T Get<T>(this HttpSessionState session, string key)
        {
            T value;
            session.TryGetValue(key, out value);
            return value;
        }

        public static bool TryGetValue<T>(this HttpSessionState session, string key, out T value)
        {
            if (session == null)
                throw new ArgumentNullException("session", "Unable to get value from session. The session is null.");

            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("No value given for key.", "key");

            try
            {
                var sessionValue = session[key];
                value = (T)sessionValue;
                return true;
            }
            catch (Exception)
            {
                Debug.WriteLine(string.Format("TryGetValue: Session has no value for '{0}'.", key));
                value = default(T);
                return false;
            }
        }
    }
}
