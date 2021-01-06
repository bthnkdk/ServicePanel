using System;
using System.Configuration;

namespace Web.UI.Helper
{

    public static class ApplicationSettingHelper
    {
        public static string BrandName
        {
            get { return ConfigurationManager.AppSettings["BrandName"]; }
        }

        public static int BaseLocationId
        {
            get { return Convert.ToInt32(ConfigurationManager.AppSettings["BaseLocationId"]); }
        }

        public static string Http
        {
            get { return ConfigurationManager.AppSettings["Http"]; }
        }

        public static string EncryptKey
        {
            get { return ConfigurationManager.AppSettings["EncryptKey"]; }
        }


        public static string GooglemMapAPIKey
        {
            get { return ConfigurationManager.AppSettings["GooglemMapAPIKey"]; }
        }

        public static int Package
        {
            get { return Convert.ToInt32(ConfigurationManager.AppSettings["Package"]); }
        }

        public static bool ShowQuickMessage
        {
            get { return Convert.ToBoolean(ConfigurationManager.AppSettings["ShowQuickMessage"]); }
        }
    }
}
