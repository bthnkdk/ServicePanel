using Core;
using Domain;
using Omu.AwesomeMvc;
using System.Linq;
using System.Web.Mvc;
using Web.UI.Helper;
using Web.UI.ViewModels;
using WebMarkupMin.AspNet4.Mvc;

namespace Web.UI.Controllers
{
    [MinifyHtml]
    public class AULogController : ROGenericController<AppUserLog>
    {
        public AULogController(IRepo<AppUserLog> repo)
            : base(repo)
        {
        }

        protected override object MapEntityToGridModel(AppUserLog model)
        {
            return new { model.Username, model.IpAddress, model.Browser, model.Os, Date = model.Date.ToString("dd.MM.yyyy HH:mm"), model.Status };
        }

        [HttpPost]
        public ActionResult GridGetItems(GridParams g, string parent)
        {
            WebUserManager.CheckIsAuthorized("SYS.S");
            parent = (parent ?? "").ToLower();
            var data = repo.Where(o => o.Username.ToLower().Contains(parent) || o.Browser.ToLower().Contains(parent) || o.Os.ToLower().Contains(parent) || o.Status.ToLower().Contains(parent) || o.IpAddress.ToLower().Contains(parent));
            var model = GetGridModelDto(g, data);
            return Json(model);
        }
        public ActionResult GetLoginErrors()
        {
            WebUserManager.CheckIsAuthorized("SYS.S");
            var logs = repo.Where(s => s.Status != "Başarılı").GroupBy(s => s.Username)
                .Select(s => new AppUserErrorViewModel()
                {
                    Count = s.Count(),
                    Username = s.Key,
                }).Take(5).OrderByDescending(s => s.Count);
            return PartialView("LoginErrors", logs);
        }
    }
}