using System;

namespace Curiosity.Common.Mvc
{
    internal static class ObjectExtensions
    {
        internal static void ThrowIfArgumentIsNull(this object obj, string argumentName)
        {
            if (obj == null)
                throw new ArgumentNullException(argumentName);
        }

        internal static void ThrowIfArgumentIsNull(this object obj, string argumentName, string message)
        {
            if (obj == null)
                throw new ArgumentNullException(argumentName, message);
        }
    }
}
