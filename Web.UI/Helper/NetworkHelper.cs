using System.Web;

namespace Web.UI.Helper
{
    public static class NetworkHelper
    {
        public static string GetIPAddress()
        {
            string IPAddress = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (!string.IsNullOrEmpty(IPAddress))
            {
                string[] ipRange = IPAddress.Split(new char[] { ',' });
                int le = ipRange.Length - 1;
                string text1 = ipRange[le];
                return IPAddress;
            }
            return HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString() == "::1" ? "127.0.0.1" : HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
        }

        public static string GetOperatingSystemType()
        {
            var ua = HttpContext.Current.Request.UserAgent;
            if (ua.Contains("Android"))
                return "Android";

            else if (ua.Contains("iPad"))
                return "iPad";

            else if (ua.Contains("iPhone"))
                return "iPhone";

            else if (ua.Contains("Linux") && ua.Contains("KFAPWI"))
                return "Kindle";

            else if (ua.Contains("RIM Tablet") || (ua.Contains("BB") && ua.Contains("Mobile")))
                return "Black Berry";

            else if (ua.Contains("Windows Phone"))
                return "Windows Phone";

            else if (ua.Contains("Mac OS"))
                return "Mac OS";

            else if (ua.Contains("Windows NT 5.1") || ua.Contains("Windows NT 5.2"))
                return "Windows XP";

            else if (ua.Contains("Windows NT 6.0"))
                return "Windows Vista";

            else if (ua.Contains("Windows NT 6.1"))
                return "Windows 7";

            else if (ua.Contains("Windows NT 6.2"))
                return "Windows 8";

            else if (ua.Contains("Windows NT 6.3"))
                return "Windows 8.1";

            else if (ua.Contains("Windows NT 10"))
                return "Windows 10";
            else
                return "UnKnown Operating System";
        }

        public static string GetBrowserType()
        {
            HttpBrowserCapabilities bc = HttpContext.Current.Request.Browser;
            var browserfullname = string.Format("{0}-V{1}", bc.Browser, bc.Version);
            return browserfullname;
        }
    }
}