using System;
using System.Configuration;
using System.Linq;
using Rair.Utilities.Core.Extensions;

namespace Rair.Utilities.Windows.Extensions
{
    public static class SettingsExtensions
    {
        private static bool HasSetting(this SettingsBase settings, string settingName)
        {
            var foundSetting = settings?.Properties.Cast<SettingsProperty>().FirstOrDefault(prop => prop.Name.Equals(settingName, StringComparison.OrdinalIgnoreCase));
            return foundSetting != null;
        }

        private static SettingsProperty GetSetting(this SettingsBase settings, string settingName)
        {
            if (!settings.HasSetting(settingName)) { return null; }
            var foundSetting = settings?.Properties.Cast<SettingsProperty>().FirstOrDefault(prop => prop.Name.Equals(settingName, StringComparison.OrdinalIgnoreCase));
            return foundSetting;
        }

        private static SettingsProperty GetOrAddSetting(this ApplicationSettingsBase settings, string settingName, Type settingType, object defaultValue)
        {
            return settings.HasSetting(settingName) ? settings.GetSetting(settingName) : settings.AddUserSetting(settingName, settingType, defaultValue);
        }

        private static SettingsProperty AddUserSetting(this ApplicationSettingsBase settings, string settingName, Type settingType, object defaultValue)
        {
            var val = Convert.ChangeType(defaultValue, settingType);
            var provider = settings.Providers.Cast<SettingsProvider>().FirstOrDefault();
            var setting = new SettingsProperty(settingName) { PropertyType = settingType, DefaultValue = val, IsReadOnly = false, Provider = provider };
            setting.Attributes[setting.Attributes.Count] = new UserScopedSettingAttribute();
            settings.Properties.Add(setting);
            settings.Reload();
            return setting;
        }

        public static void SaveSetting(this ApplicationSettingsBase settings, string settingName, object value)
        {
            if (!settings.HasSetting(settingName)) { return; }

            var val = Convert.ChangeType(value, value.GetType());
            settings[settingName] = val;
        }

        public static T LoadSetting<T>(this ApplicationSettingsBase settings, string settingName)
        {
            if (!settings.HasSetting(settingName)) { return default(T); }

            var val = settings[settingName];
            return Convert.ChangeType(val, typeof(T)).To<T>();
        }
    }
}
