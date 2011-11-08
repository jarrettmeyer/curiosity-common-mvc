using System;
using System.Diagnostics;
using System.Web;

namespace Curiosity.Common.Mvc
{
    public static class HttpSessionStateBaseExtensions
    {
        public static T Get<T>(this HttpSessionStateBase session, string key)
        {
            T value;
            session.TryGetValue(key, out value);
            return value;
        }

        public static bool TryGetValue<T>(this HttpSessionStateBase session, string key, out T value)
        {
            if (session == null)
                throw new ArgumentNullException("session");

            try
            {
                object sessionValue = session[key];
                value = (T)sessionValue;
                return true;
            }
            catch (Exception)
            {
                Debug.WriteLine(string.Format("TryGetValue: Unable to get session value for key '{0}'.", key));
                value = default(T);
                return false;                
            }
        }
    }
}
