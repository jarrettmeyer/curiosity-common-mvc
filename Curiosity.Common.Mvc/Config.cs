using System.Configuration;

namespace Curiosity.Common.Mvc
{
    /// <summary>
    /// Internal class for holding values that can be configured as part of your app.config file.
    /// </summary>
    internal static class Config
    {
        /// <summary>
        /// CSS class to be applied to all flash responses. 
        /// The key for this value is "FlashCssClass". 
        /// The default value is "flash".
        /// </summary>
        public static string FlashCssClass
        {
            get 
            { 
                var setting = GetSetting("FlashCssClass", "flash");
                return setting;
            }
        }

        private static string GetSetting(string key, string defaultValue)
        {
            var settingValue = ConfigurationManager.AppSettings[key];
            return !string.IsNullOrEmpty(settingValue) ? settingValue : defaultValue;
        }
    }
}
