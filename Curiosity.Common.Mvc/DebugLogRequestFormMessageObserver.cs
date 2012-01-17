using System.Diagnostics;
using Curiosity.Common.Messaging;

namespace Curiosity.Common.Mvc
{
#if DEBUG
    /// <summary>
    /// As an observer, this will make no changes to the message. It will simply log
    /// the form data to the debug window. Useful for debugging.
    /// </summary>
    public class DebugLogRequestFormMessageObserver : MessageHandlerBase<LogRequestFormMessage>
    {
        /// <summary>
        /// Handle the log request form message.
        /// </summary>        
        public override void Handle(LogRequestFormMessage message)
        {
            if (message.HasFormData)
            {
                Debug.WriteLine(message);
            }
        }
    }    
#endif
}
