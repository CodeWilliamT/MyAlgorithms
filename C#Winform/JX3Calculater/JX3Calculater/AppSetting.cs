using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Reflection;

namespace AppSettingDll
{
    public class AppSetting
    {
        static Configuration config;
        static KeyValueConfigurationCollection settings;
        static void BeReady()
        {
            if (config == null)
            {
                config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                settings = config.AppSettings.Settings;
            }
        }

        public static void SaveOne(string key, string value)
        {
            BeReady();
            try
            {
                if (settings[key] == null)
                {
                    settings.Add(key, value);
                }
                else
                {
                    settings[key].Value = value;
                }
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Error writing app settings");
            }
        }
        public static string LoadOne(string key)
        {
            BeReady();
            return ConfigurationManager.AppSettings[key];
        }

        public static void SaveObj(object obj, string objKey)
        {
            FieldInfo[] infos = obj.GetType().GetFields();
            foreach (FieldInfo fi in infos)
            {
                if (fi.Attributes.HasFlag(FieldAttributes.Static))
                {
                    switch (fi.FieldType.Name)
                    {
                        case "Int32":
                            SaveOne(obj.GetType().Name + "." + fi.Name, fi.GetValue(obj).ToString());
                            break;
                        case "Double":
                            SaveOne(obj.GetType().Name + "." + fi.Name, ((double)fi.GetValue(obj)).ToString());
                            break;
                        case "Boolean":
                            SaveOne(obj.GetType().Name + "." + fi.Name, fi.GetValue(obj).ToString());
                            break;
                        case "String":
                            SaveOne(obj.GetType().Name + "." + fi.Name, (string)fi.GetValue(obj));
                            break;
                        default:
                            break;
                    }
                }
                else
                { 
                    switch (fi.FieldType.Name)
                    {
                        case "Int32":
                            SaveOne(objKey + "." + fi.Name, fi.GetValue(obj).ToString());
                            break;
                        case "Double":
                            SaveOne(objKey + "." + fi.Name, ((double)fi.GetValue(obj)).ToString());
                            break;
                        case "Boolean":
                            SaveOne(objKey + "." + fi.Name, fi.GetValue(obj).ToString());
                            break;
                        case "String":
                            SaveOne(objKey + "." + fi.Name, (string)fi.GetValue(obj));
                            break;
                        default:
                            break;
                    }
                }
            }
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection(config.AppSettings.SectionInformation.Name);
        }
        public static void LoadObj(object obj, string objKey)
        {
            FieldInfo[] infos = obj.GetType().GetFields();
            string tempString;
            foreach (FieldInfo fi in infos)
            {

                if (fi.Attributes.HasFlag(FieldAttributes.Static))
                {
                    switch (fi.FieldType.Name)
                    {
                        case "Int32":
                            tempString = ConfigurationManager.AppSettings[obj.GetType().Name + "." + fi.Name];
                            if (tempString != null)
                                fi.SetValue(obj, int.Parse(tempString));
                            break;
                        case "Double":
                            tempString = ConfigurationManager.AppSettings[obj.GetType().Name + "." + fi.Name];
                            if (tempString != null)
                                fi.SetValue(obj, double.Parse(tempString));
                            break;
                        case "Boolean":
                            tempString = ConfigurationManager.AppSettings[obj.GetType().Name + "." + fi.Name];
                            if (tempString != null)
                                fi.SetValue(obj, bool.Parse(tempString));
                            break;
                        case "String":
                            tempString = ConfigurationManager.AppSettings[obj.GetType().Name + "." + fi.Name];
                            if (tempString != null)
                                fi.SetValue(obj, tempString);
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    switch (fi.FieldType.Name)
                    {
                        case "Int32":
                            tempString = ConfigurationManager.AppSettings[objKey + "." + fi.Name];
                            if (tempString != null)
                                fi.SetValue(obj, int.Parse(tempString));
                            break;
                        case "Double":
                            tempString = ConfigurationManager.AppSettings[objKey + "." + fi.Name];
                            if (tempString != null)
                                fi.SetValue(obj, double.Parse(tempString));
                            break;
                        case "Boolean":
                            tempString = ConfigurationManager.AppSettings[objKey + "." + fi.Name];
                            if (tempString != null)
                                fi.SetValue(obj, bool.Parse(tempString));
                            break;
                        case "String":
                            tempString = ConfigurationManager.AppSettings[objKey + "." + fi.Name];
                            if (tempString != null)
                                fi.SetValue(obj, tempString);
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }
}
