using Core;
using Domain;
using System;
using System.Globalization;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Web.UI.Helper;
using Web.UI.ViewModels;
using WebMarkupMin.AspNet4.Mvc;

namespace Web.UI.Controllers
{
    [Authorize]
    [MinifyHtml]
    public class AccountController : Controller
    {
        readonly IUserRepo<AppUser> userRepo;
        readonly IRepo<AppUserLog> userLogRepo;

        public AccountController(IUserRepo<AppUser> userRepo, IRepo<AppUserLog> userLogRepo)
        {
            this.userRepo = userRepo;
            this.userLogRepo = userLogRepo;
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            if (Request.IsAuthenticated)
                return Redirect("/");
            else
                return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginInput input, string returnUrl)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(input);

                string captchaKey = Session["CaptchaKey"]?.ToString() ?? "";
                if (Session == null || (input.Code != captchaKey))
                {
                    LogIt(input.Email, "Güvenlik kodu hatalı");
                    throw new Exception("Güvenlik kodunu hatalı girdiniz !");
                }

                var user = userRepo.GetActiveAppUserFromUsername(input.Email);
                if (user == null)
                {
                    LogIt(input.Email, "Kullanıcı yok");
                    throw new Exception("Girilen e-posta adresine ait kullanıcı bulunamadı !");
                }

                if (!user.VerifyHashedPassword(input.Password))
                {
                    LogIt(input.Email, "Hatalı parola");
                    throw new Exception("Girilen parola hatalı !");
                }

                LogIt(input.Email, "Başarılı");

                FormsAuthentication.Initialize();
                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                    1,
                    user.Username,
                    DateTime.Now,
                    DateTime.Now.AddYears(1),
                    true,
                    "",
                    FormsAuthentication.FormsCookiePath);

                string hash = FormsAuthentication.Encrypt(ticket);
                HttpCookie cookie = new HttpCookie(
                    FormsAuthentication.FormsCookieName,
                    hash);
                cookie.HttpOnly = true;

                //if (ticket.IsPersistent)
                cookie.Expires = ticket.Expiration;
                HttpContext.Response.Cookies.Add(cookie);

                WebUserManager.SetUserInfo(user.Username);
                return RedirectToLocal(returnUrl);
            }
            catch (Exception ex)
            {
                ModelState.SetModelValue("Code", new ValueProviderResult("", "", CultureInfo.InvariantCulture));
                ModelState.AddModelError("", ex.Message);
                return View(input);
            }
        }

        [AllowAnonymous]
        public ActionResult CreateImage()
        {
            string captchaKey = StringHelper.CreateCaptchaKey();
            base.Session["CaptchaKey"] = captchaKey;
            return base.File(GdiHelper.CreateImage(captchaKey), "image/png");
        }

        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(ForgotPasswordInput input)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(input);

                var user = userRepo.ForgetPassword(input.Email);

                string callbackUrl = Url.Action("ResetPassword", "Account", new { userid = EncryptHelper.EncryptString(user.Id.ToString(), ApplicationSettingHelper.EncryptKey), code = user.TokenKey }, protocol: Request.Url.Scheme);
                var filePath = Server.MapPath("~/App_Data/MailTemplate.html");
                string content = string.Format("<h3>Parola sıfırlama</h3><p>Sayın {0} {1},</p><p>Parola sıfırlama işlemini başlatmak için <a href='{2}'>tıklayınız.</a>",
                    user.Firstname,
                    user.Lastname,
                    callbackUrl);
                string body = string.Format(System.IO.File.ReadAllText(filePath, Encoding.GetEncoding(1254)), ApplicationSettingHelper.BrandName, content);
                MailHelper.SendMail(user.Username, "Parolamı Unuttum", body);
                return RedirectToAction("ForgotPasswordConfirmation");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(input);
            }
        }

        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult ResetPassword()
        {
            string userid = Request["userid"];
            string code = Request["code"];
            if (!string.IsNullOrEmpty(userid) && !string.IsNullOrEmpty(code))
            {
                var input = new ResetPasswordInput();
                input.Code = code;
                input.UserId = userid;
                return View(input);
            }
            else
                return RedirectToAction("ErrorPassword");
        }

        [AllowAnonymous]
        public ActionResult ErrorPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordInput input)
        {
            try
            {
                int id = Convert.ToInt32(EncryptHelper.DecryptString(input.UserId, ApplicationSettingHelper.EncryptKey));
                userRepo.ResetPassword(id, input.Email, input.Code, input.Password);
                return RedirectToAction("ResetPasswordConfirmation");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(input);
            }
        }

        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        public ActionResult LogOff()
        {
            WebUserManager.SignOut();
            return RedirectToAction("Login");
        }

        public ActionResult UserProfile()
        {
            var userInfo = WebUserManager.GetUserInfo();
            var entity = userRepo.Get(userInfo.Id);
            return PartialView(entity);
        }

        [HttpPost]
        public ActionResult UpdatePassword(UpdatePasswordInput input)
        {
            try
            {
                if (!ModelState.IsValid)
                    return PartialView(input);

                var userInfo = WebUserManager.GetUserInfo();
                userRepo.UpdatePassword(userInfo.Id, input.OldPassword, input.Password);

                return Json(new { Url = "/Account/LogOff" });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return PartialView(input);
            }
        }

        [AllowAnonymous]
        public ActionResult Lock()
        {
            return View();
        }

        public ActionResult Refresh()
        {
            WebUserManager.SetUserInfo();
            return Redirect("/");
        }

        void LogIt(string username, string status)
        {
            userLogRepo.Insert(
                new AppUserLog
                {
                    Username = username,
                    Status = status,
                    IpAddress = NetworkHelper.GetIPAddress(),
                    Browser = NetworkHelper.GetBrowserType(),
                    Os = NetworkHelper.GetOperatingSystemType(),
                    Date = DateTime.Now
                });
            userLogRepo.Save();
        }

        ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}