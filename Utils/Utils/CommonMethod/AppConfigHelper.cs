using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Linq;

namespace Utils
{
    public class AppConfigHelper
    {

        const string spliter = @"; ";

        public static void SaveKey(string key, string value)
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
            string val = ConfigurationManager.AppSettings[key];
            return val != null ? val : "";
        }

        /// <summary>
        /// 保存List<string>
        /// </summary>
        /// <param name="key"></param>
        /// <param name="listStr"></param>
        /// <returns></returns>
        public static bool SaveStringList(string key, List<string> listStr)
        {
            string value = "";
            foreach (string ls in listStr)
            {
                value += ls + spliter;
            }
            SaveKey(key, value);
            return true;
        }
        /// <summary>
        /// 读取List<string>
        /// </summary>
        /// <param name="key"></param>
        /// <param name="listStr"></param>
        /// <returns></returns>
        public static List<string> SaveStringList(string key)
        {
            List<string> listStr = new List<string>();
            string value = LoadKey(key);
            listStr = value.Split(new[] { spliter }, StringSplitOptions.RemoveEmptyEntries).ToList();

            return listStr;
        }

        /// <summary>
        /// 保存类内所有int,double,bool,string public变量
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool SaveObj(object obj)
        {
            if (obj == null) return false;
            List<string> types = new List<string>() { "Int32", "Double", "long", "float", "Boolean", "String" };
            System.Reflection.FieldInfo[] infos = obj.GetType().GetFields();
            foreach (System.Reflection.FieldInfo fi in infos)
            {
                if (types.Contains(fi.FieldType.Name))
                    SaveKey(obj.GetType().ToString() + "-" + fi.Name, fi.GetValue(obj).ToString());
            }
            return true;
        }

        /// <summary>
        /// 加载对象内所有int,double,bool,string public变量
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool LoadObj(object obj)
        {
            System.Reflection.FieldInfo[] infos = obj.GetType().GetFields();

            foreach (System.Reflection.FieldInfo fi in infos)
            {
                switch (fi.FieldType.Name)
                {
                    case "Int32":
                        int tempInt = LoadInt(obj.GetType().ToString() + "-" + fi.Name);
                        fi.SetValue(obj, tempInt);
                        break;
                    case "long":
                        long templong = LoadLong(obj.GetType().ToString() + "-" + fi.Name);
                        fi.SetValue(obj, templong);
                        break;
                    case "float":
                        float tempFloat = LoadFloat(obj.GetType().ToString() + "-" + fi.Name);
                        fi.SetValue(obj, tempFloat);
                        break;
                    case "Double":
                        double tempDouble = LoadDouble(obj.GetType().ToString() + "-" + fi.Name);
                        fi.SetValue(obj, tempDouble);
                        break;
                    case "Boolean":
                        bool tempBool = LoadBool(obj.GetType().ToString() + "-" + fi.Name);
                        fi.SetValue(obj, tempBool);
                        break;
                    case "String":
                        string tempString = LoadKey(obj.GetType().ToString() + "-" + fi.Name);
                        fi.SetValue(obj, tempString);
                        break;
                    default:
                        break;
                }
            }
            return true;
        }
        public static int LoadInt(string key)
        {
            try
            {
                return int.Parse(ConfigurationManager.AppSettings[key]);
            }
            catch
            {
                return 0;
            }
        }
        public static long LoadLong(string key)
        {
            try
            {
                return long.Parse(ConfigurationManager.AppSettings[key]);
            }
            catch
            {
                return 0;
            }
        }
        public static float LoadFloat(string key)
        {
            try
            {
                return float.Parse(ConfigurationManager.AppSettings[key]);
            }
            catch
            {
                return 0;
            }
        }
        public static double LoadDouble(string key)
        {
            try
            {
                return double.Parse(ConfigurationManager.AppSettings[key]);
            }
            catch
            {
                return 0.0;
            }
        }

        public static bool LoadBool(string key)
        {
            try
            {
                return bool.Parse(ConfigurationManager.AppSettings[key]);
            }
            catch
            {
                return false;
            }
        }
    }
}
