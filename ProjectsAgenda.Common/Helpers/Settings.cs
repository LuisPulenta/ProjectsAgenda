using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace MyLeasing.Common.Helpers
{
    public static class Settings
    {
        private const string _token = "token";
        private const string _partner = "partner";
        private const string _isRemembered = "IsRemembered";


        
        private const string _project = "project";
        private static readonly string _settingsDefault = string.Empty;
        private static readonly bool _boolDefault = false;

        private static ISettings AppSettings => CrossSettings.Current;

  
        public static string Project
        {
            get => AppSettings.GetValueOrDefault(_project, _settingsDefault);
            set => AppSettings.AddOrUpdateValue(_project, value);
        }

        public static string Token
        {
            get => AppSettings.GetValueOrDefault(_token, _settingsDefault);
            set => AppSettings.AddOrUpdateValue(_token, value);
        }

        public static string Partner
        {
            get => AppSettings.GetValueOrDefault(_partner, _settingsDefault);
            set => AppSettings.AddOrUpdateValue(_partner, value);
        }

        public static bool IsRemembered
        {
            get => AppSettings.GetValueOrDefault(_isRemembered, _boolDefault);
            set => AppSettings.AddOrUpdateValue(_isRemembered, value);
        }


    }
}