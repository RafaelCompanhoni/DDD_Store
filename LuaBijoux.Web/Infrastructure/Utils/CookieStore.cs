using System;
using System.Web;

namespace LuaBijoux.Web.Infrastructure.Utils
{
    public class CookieStore
    {
        public static void SetCookie(string key, string value, int daysToExpire = 1)
        {
            TimeSpan expires = TimeSpan.FromDays(daysToExpire);
            HttpCookie cookie = new HttpCookie(key, value);
            cookie.Expires = DateTime.Now.Add(expires);
            HttpContext.Current.Response.Cookies.Set(cookie);
        }

        public static string GetCookie(string key)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[key];
            return cookie == null ? null : cookie.Value;
        }
    }
}