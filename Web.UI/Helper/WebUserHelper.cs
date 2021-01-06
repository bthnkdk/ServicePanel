using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using Web.UI.ViewModels;

namespace Web.UI.Helper
{
    public class UserInfo
    {
        public int Id { get; set; }
        public int TitleId { get; set; }
        public string TitleName { get; set; }
        public string DepartmentName { get; set; }
        public int DepartmentId { get; set; }
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName
        {
            get
            {
                return $"{FirstName} {LastName}";
            }
        }
        public bool IsAdmin { get; set; }
    }

    public class WebUserManager
    {
        public static void SignOut()
        {
            FormsAuthentication.SignOut();
            HttpContext.Current.Session.Abandon();
            HttpCookie aCookie;
            string cookieName;
            int count = HttpContext.Current.Request.Cookies.Count;
            for (int i = 0; i < count; i++)
            {
                cookieName = HttpContext.Current.Request.Cookies[i].Name;
                aCookie = new HttpCookie(cookieName);
                aCookie.Expires = DateTime.Now.AddDays(-1);
                HttpContext.Current.Response.Cookies.Add(aCookie);
            }
        }

        public static bool IsAuthorized(string authorityCode)
        {
            var userInfo = GetUserInfo();
            if (GetLockedIds().Contains(userInfo.Id))
            {
                FormsAuthentication.SignOut();
                return false;
            }
            if (userInfo.IsAdmin)
                return true;

            return GetAuthCodes().Any(p => p.Key == userInfo.Id && p.Value == authorityCode);
        }

        public static bool IsUpdateAuthorize(string authorityCode)
        {
            return IsAuthorized(authorityCode + EnumHelper.AuthorizeMethod.Update);
        }
        public static bool IsDeleteAuthorize(string authorityCode)
        {
            return IsAuthorized(authorityCode + EnumHelper.AuthorizeMethod.Delete);
        }
        public static List<KeyValue> GetAuthCodes()
        {
            if (HttpContext.Current.Application["AuthCodes"] == null)
            {
                var lockedIds = GetLockedIds().ToStringInt();
                List<KeyValue> list = new List<KeyValue>();
                var rd = DbHelper.ExecuteReader("select AppUserId,Code = AC.Code + '.' + AU.Access from AppUserAuthority AU JOIN AuthCode AC ON AU.AuthCodeId = AC.Id where AppUserId not in(" + lockedIds + ")");
                while (rd.Read())
                    list.Add(new KeyValue
                    {
                        Key = rd.GetInt32(0),
                        Value = rd[1].ToString()
                    });
                rd.Close();
                HttpContext.Current.Application.Lock();
                HttpContext.Current.Application["AuthCodes"] = list;
                HttpContext.Current.Application.UnLock();
            }
            return (List<KeyValue>)HttpContext.Current.Application["AuthCodes"];
        }

        static List<int> GetLockedIds()
        {
            if (HttpContext.Current.Application["LockedIds"] == null)
            {
                List<int> list = new List<int>();
                list.Add(0);
                var rd = DbHelper.ExecuteReader("select Id from AppUser Where IsLock = 1");
                while (rd.Read())
                    list.Add(rd.GetInt32(0));
                rd.Close();
                HttpContext.Current.Application.Lock();
                HttpContext.Current.Application["LockedIds"] = list;
                HttpContext.Current.Application.UnLock();
            }
            return (List<int>)HttpContext.Current.Application["LockedIds"];
        }
        public static void CheckIsAuthorized(string authorityCode)
        {
            var userInfo = GetUserInfo();
            if (!userInfo.IsAdmin)
            {
                if (!IsAuthorized(authorityCode))
                    throw new UnauthorizedAccessException("Yetkisiz Erişim !");

                if (GetLockedIds().Contains(userInfo.Id))
                {
                    SignOut();
                    throw new UnauthorizedAccessException("Hesap Kilitli !");
                }
            }
        }

        public static void Authorized(string authorityCode)
        {
            if (!IsAuthorized(authorityCode))
                throw new UnauthorizedAccessException("Yetkisiz Erişim !");
        }

        public static string GetUsername()
        {
            return HttpContext.Current.User.Identity.Name;
        }

        public static void SetUserInfo(string username = "")
        {
            username = string.IsNullOrEmpty(username) ? GetUsername() : username;
            var reader = DbHelper.ExecuteReader("select TBL.Id,TBL.TitleId,TBL.Username,TBL.Firstname,TBL.Lastname,TBL.DepartmentId,TBL.IsAdmin,T.Name TitleName,D.Name DepartmentName  from AppUser TBL join Title T on TBL.TitleId = T.Id join Department D on TBL.DepartmentId = TBL.DepartmentId where TBL.Username = @Username and T.IsDeleted = 0 and IsLock = 0", username);
            if (reader.Read())
            {
                UserInfo userInfo = new UserInfo();
                userInfo.Id = reader.GetInt32(0);
                userInfo.TitleId = reader.GetInt32(1);
                userInfo.Name = reader[2].ToString();
                userInfo.FirstName = reader[3].ToString();
                userInfo.LastName = reader[4].ToString();
                userInfo.DepartmentId = reader.GetInt32(5);
                userInfo.IsAdmin = reader.GetBoolean(6);
                userInfo.TitleName = reader[7].ToString();
                userInfo.DepartmentName = reader[8].ToString();
                HttpContext.Current.Session["UserInfo"] = userInfo;
            }
            else
            {
                SignOut();
                throw new Exception("Oturum sistem tarafından sonlandırıldı !");
            }
        }

        public static UserInfo GetUserInfo()
        {
            if (HttpContext.Current.Session["UserInfo"] == null)
                SetUserInfo();
            return (UserInfo)HttpContext.Current.Session["UserInfo"];
        }
    }
}