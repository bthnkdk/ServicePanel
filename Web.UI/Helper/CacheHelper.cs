using System;

namespace Web.UI.Helper
{
    public class CacheHelper
    {
        public static void Add(string key, object data)
        {
            System.Web.HttpContext.Current.Cache.Add(key, data, null, DateTime.Now.AddHours(6), TimeSpan.Zero, System.Web.Caching.CacheItemPriority.Default, null);
        }

        public static object Get(string key)
        {
            return System.Web.HttpContext.Current.Cache[key];
        }

        public static void Remove(string key)
        {
            System.Web.HttpContext.Current.Cache.Remove(key);
        }
    }
}