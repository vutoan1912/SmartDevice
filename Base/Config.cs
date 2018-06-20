using System;
using System.Configuration;
//using System.Collections.Specialized;
using System.Xml;
using System.IO;
using System.Data;
using System.Windows.Forms;
using System.Reflection;
using System.Collections.Generic;

namespace ERP.Base
{
    class Config
    {
        public const string LogPath = "Logs/";

        private static string _token;
        public static string Token
        {
            get { return _token; }
            set { _token = value; }
        }

        //private static string _API_URL = "http://10.2.9.141:9090/api/";
        private static string _API_URL = "http://10.15.12.148:9090/api/";
        public static string API_URL
        {
            get
            {
                return _API_URL;
                //return getConfig("API_URL");
            }
            set
            {
                _API_URL = value;
                //setConfig("API_URL", value);
            }
        }

        private static void setConfig(string Key, string Value)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            //make changes
            config.AppSettings.Settings[Key].Value = Value;

            //save to apply changes
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }

        private static string getConfig(string Key)
        {
            //string value = System.Configuration.ConfigurationManager.AppSettings[Key];
            //var customConfig = (System.Collections.Specialized.NameValueCollection)System.Configuration.ConfigurationManager.GetSection("appSettings");
            //var API_URL = customConfig["API_URL"];

            return "";
            //return readConfigXml(Key);
        }

        private static string readConfigXml(string Key)
        {
            System.Xml.XmlDocument xmldoc = new XmlDocument();
            //XmlDataDocument xmldoc = new XmlDataDocument();
            XmlNodeList xmlnode;
            string str = null;
            string path = GetExecutingDirectoryName() + "\\ERP.exe.config";
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            xmldoc.Load(fs);
            xmlnode = xmldoc.GetElementsByTagName("API_URL");

            //Util.Logs(xmlnode.Item(0).Value);

            return xmlnode.Item(0).Value;

            //for (i = 0; i <= xmlnode.Count - 1; i++)
            //{
            //    xmlnode[i].ChildNodes.Item(0).InnerText.Trim();
            //    str = xmlnode[i].ChildNodes.Item(0).InnerText.Trim() + "  " + xmlnode[i].ChildNodes.Item(1).InnerText.Trim() + "  " + xmlnode[i].ChildNodes.Item(2).InnerText.Trim();
            //}
            return str;
        }

        private static string GetExecutingDirectoryName()
        {
            //var location = new Uri(Assembly.GetExecutingAssembly().GetName().CodeBase);
            //var location = new Uri(Assembly.GetCallingAssembly().GetName().CodeBase);
            //return new FileInfo(location.AbsolutePath).Directory.FullName;

            String full_path = System.Reflection.Assembly.GetCallingAssembly().GetName().CodeBase;
            String directory_path = full_path.Substring(0, full_path.LastIndexOf("\\"));
            return directory_path;
        }
    }

    // ----------------------------------------------------------------------------------------------------------

    //public sealed class UsersConfigMapSection : ConfigurationSection
    //{
    //    private static UsersConfigMapSection config = ConfigurationManager.GetSection("Users") as UsersConfigMapSection;

    //    public static UsersConfigMapSection Config
    //    {
    //        get
    //        {
    //            return config;
    //        }
    //    }

    //    [ConfigurationProperty("", IsRequired = true, IsDefaultCollection = true)]
    //    private UsersConfigMapConfigElements Settings
    //    {
    //        get { return (UsersConfigMapConfigElements)this[""]; }
    //        set { this[""] = value; }
    //    }

    //    public IEnumerable<UsersConfigMapConfigElement> SettingsList
    //    {
    //        get { return this.Settings.Cast<UsersConfigMapConfigElement>(); }
    //    }
    //}

    //public sealed class UsersConfigMapConfigElements : ConfigurationElementCollection
    //{
    //    protected override ConfigurationElement CreateNewElement()
    //    {
    //        return new UsersConfigMapConfigElement();
    //    }
    //    protected override object GetElementKey(ConfigurationElement element)
    //    {
    //        return ((UsersConfigMapConfigElement)element).Username;
    //    }
    //}

    //public sealed class UsersConfigMapConfigElement : ConfigurationElement
    //{
    //    [ConfigurationProperty("username", IsKey = true, IsRequired = true)]
    //    public string Username
    //    {
    //        get { return (string)base["username"]; }
    //        set { base["username"] = value; }
    //    }

    //    [ConfigurationProperty("password", IsRequired = true)]
    //    public string Password
    //    {
    //        get { return (string)base["password"]; }
    //        set { base["password"] = value; }
    //    }

    //    [ConfigurationProperty("domain", IsRequired = true)]
    //    public string Domain
    //    {
    //        get { return (string)base["domain"]; }
    //        set { base["domain"] = value; }
    //    }
    //}

}