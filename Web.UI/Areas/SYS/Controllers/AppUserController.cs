using Core;
using Domain;
using Omu.AwesomeMvc;
using System;
using System.Linq;
using System.Web.Helpers;
using System.Web.Mvc;
using Web.UI.Controllers;
using Web.UI.Helper;
using Web.UI.Mappers;
using WebMarkupMin.AspNet4.Mvc;

namespace Web.UI.Areas.SYS.Controllers
{
    [JsonAllowGet]
    [MinifyHtml]
    public class AppUserController : XGenericController<AppUser, AppUserInput, AppUserInput>
    {
        IRepo<AppUserLog> appLogRepo;
        public AppUserController(IRepo<AppUser> repo,IRepo<AppUserLog> appLogRepo, IMapper mapper)
            : base(repo, mapper)
        {
            this.appLogRepo = appLogRepo;
        }

        protected override object MapEntityToGridModel(AppUser model)
        {
            return new { model.Id, model.Firstname, model.IsDeleted, model.Lastname, model.Username, TitleName = model.Title.Name };
        }

        [HttpPost]
        public ActionResult GridGetItems(GridParams g, int? title, string parent, bool? restore)
        {
            CheckIsAuthorized(EnumHelper.AuthorizeMethod.Select);
            parent = (parent ?? string.Empty).ToLower();
            var isAdmin = WebUserManager.GetUserInfo().IsAdmin;
            if (restore.HasValue && restore.Value && WebUserManager.IsUpdateAuthorize(GetCode()))
            {
                repo.Restore(repo.Get(Convert.ToInt32(g.Key)));
                repo.Save();
            }
            var data = repo.Where(o => o.Firstname.ToLower().Contains(parent) || o.Lastname.ToLower().Contains(parent) || o.Username.ToLower().Contains(parent), isAdmin);
            //if (title.HasValue) data = data.Where(o => o.TitleId == title.Value);
            var model = GetGridModelDto(g, data);
            return Json(model);
        }

        [HttpPost]
        public override ActionResult Create(AppUserInput input)
        {
            try
            {
                CheckIsAuthorized(EnumHelper.AuthorizeMethod.Insert);

                if (!ModelState.IsValid)
                    return PartialView(input);

                AppUser model = mapper.Map<AppUserInput, AppUser>(input);
                model.RowId = Guid.NewGuid();
                model.Pin = "0000";
                string password = Guid.NewGuid().ToString().ToLower().Substring(0, 4);
                model.Password = Crypto.HashPassword(password);
                model.TokenKey = Guid.NewGuid().ToString();

                if (input.CopyAppUserId.HasValue)
                {
                    var copyAuthCodes = repo.Get(input.CopyAppUserId.Value).AppUserAuthorities.Select(s => new AppUserAuthority
                    {
                        AuthCodeId = s.AuthCodeId,
                        Access = s.Access
                    }).ToList();
                    model.AppUserAuthorities = copyAuthCodes;
                }

                var entity = repo.Insert(model);
                repo.Save();

                string callbackUrl = Url.Action("Login", "Account", null, protocol: Request.Url.Scheme);
                string mailTemplate = System.IO.File.ReadAllText(Server.MapPath("~/App_Data/MailTemplate.html"));
                string body = $"<h3>Kullanıcı Bilgileri</h3><p>Merhaba {model.Firstname} {model.Lastname},</p><p>Hesabınız aktif edilmiş ve aşağıda yer alan bilgiler ile <a href='{callbackUrl}'>oturum</a> açabilirsiniz.</p><p><b>Kullanıcı Adı : </b>{model.Username}<br><b>Parola : </b>{password}</p>";
                var mailResult = MailHelper.SendMail(model.Username, "Hesap Bilgileri",
                        string.Format(mailTemplate, ApplicationSettingHelper.BrandName, body));

                return Json(MapEntityToGridModel(entity));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return PartialView(input);
            }
        }

        [HttpPost]
        public override ActionResult Edit(AppUserInput input)
        {
            HttpContext.Application.Lock();
            HttpContext.Application["LockedIds"] = null;
            HttpContext.Application.UnLock();
            try
            {
                CheckIsAuthorized(EnumHelper.AuthorizeMethod.Update);

                if (!ModelState.IsValid)
                    return PartialView(EditView, input);

                var entity = mapper.Map<AppUserInput, AppUser>(input, repo.Get(input.Id));

                if (input.CopyAppUserId.HasValue)
                {
                    var copyAuthCodes = repo.Get(input.CopyAppUserId.Value).AppUserAuthorities.Select(s => new AppUserAuthority
                    {
                        AuthCodeId = s.AuthCodeId,
                        Access = s.Access,
                        AppUserId = input.Id
                    }).ToList();
                    entity.AppUserAuthorities = copyAuthCodes;
                }
                repo.Save();
                return Json(MapEntityToGridModel(repo.Get(entity.Id)));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return PartialView("Create", input);
            }
        }

        public ActionResult AppUserAuthority(int id)
        {
            try
            {
                WebUserManager.CheckIsAuthorized("SYS.A");
                AppUserAuthorityInput input = new AppUserAuthorityInput();
                var appUser = repo.Get(id);
                input.AppUserName = string.Format("{0} {1}", appUser.Firstname, appUser.Lastname);
                input.AppUserId = id;
                input.Codes = appUser.AppUserAuthorities.Select(s => new AuthorityInput
                {
                    Access = s.Access,
                    AuthCodeId = s.AuthCodeId
                }).ToList();

                return PartialView("_AppUserAuthority", input);
            }
            catch (UnauthorizedAccessException)
            {
                return PartialView("_NoAccess");
            }
            catch (Exception ex)
            {
                return PartialView("_Error", ex.Message);
            }
        }

        [HttpPost]
        public ActionResult ResetPassword(int id)
        {
            try
            {
                CheckIsAuthorized(EnumHelper.AuthorizeMethod.Update);

                var appUser = repo.Get(id);
                string password = Guid.NewGuid().ToString().ToLower().Substring(0, 4);
                appUser.Password = Crypto.HashPassword(password);
                repo.Save();
                MailHelper.SendMail(appUser.Username, ApplicationSettingHelper.BrandName + " Parola Sıfırlama", "Parolanız başarıyla sıfırlanmıştır. <br><br>Yeni Parolanız : " + password);
                return Json(new { Content = "Şifre başarıyla sıfırlandı." });
            }
            catch (Exception ex)
            {
                return Json(new { Error = ex.Message });
            }

        }

        [HttpPost]
        public ActionResult AppUserAuthority(AppUserAuthorityInput input)
        {
            try
            {
                WebUserManager.CheckIsAuthorized("SYS.A");
                if (!ModelState.IsValid)
                    return PartialView(input);

                var appUser = repo.Get(input.AppUserId);
                appUser.AppUserAuthorities.Clear();
                if (input.Codes != null)
                {
                    foreach (var code in input.Codes.Where(s => s.AuthCodeId != 0))
                    {
                        appUser.AppUserAuthorities.Add(new AppUserAuthority
                        {
                            AppUserId = input.AppUserId,
                            AuthCodeId = code.AuthCodeId,
                            Access = code.Access
                        });
                    }
                }
                repo.Save();
                HttpContext.Application.Lock();
                HttpContext.Application["AuthCodes"] = null;
                HttpContext.Application.UnLock();
                return Json(new { Content = "Güncellendi" });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return PartialView(input);
            }
        }
        public ActionResult AppUserLog()
        {
            CheckIsAuthorized(EnumHelper.AuthorizeMethod.Select);
            var appUserLog = appLogRepo.GetAll().OrderByDescending(s=>s.Id).ToList();
            return PartialView(appUserLog);
        }
    }
}