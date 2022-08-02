using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace Utils
{
    public class AppConfigHelper
    {

        public static void UpdateKey(string key, string value)
        {
            try
            {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;
                if (settings[key] == null)
                {
                    settings.Add(key, value);
                }
                else
                {
                    settings[key].Value = value;
                }
                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Error writing app settings");
            }
        }

        public static string LoadKey(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
    }
}
