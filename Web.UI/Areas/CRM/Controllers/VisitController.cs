using Core;
using Domain;
using Omu.AwesomeMvc;
using System;
using System.Linq;
using System.Transactions;
using System.Web.Mvc;
using Web.UI.Controllers;
using Web.UI.Helper;
using Web.UI.Mappers;
using WebMarkupMin.AspNet4.Mvc;

namespace Web.UI.Areas.CRM.Controllers
{
    [JsonAllowGet]
    [MinifyHtml]
    public class VisitController : XGenericController<Visit, VisitInput, VisitInput>
    {
        IRepo<VisitLog> logRepo;
        public VisitController(IRepo<Visit> repo, IRepo<VisitLog> logRepo, IMapper mapper)
            : base(repo, mapper)
        {
            this.logRepo = logRepo;
        }
        protected override object MapEntityToGridModel(Visit model)
        {
            return new { model.Id, CustomerName = model.Customer.Name, VisitTypeName = model.VisitType.Name, VisitCategoryName = model.VisitCategory.Name, PersonalName = model.AppUser.FullName, CreatedUser = model.CreatedUser.FullName, model.Status, VisitDate = model.VisitDate.ToString("dd.MM.yyyy"), model.IsDeleted };
        }

        public override ActionResult Create()
        {
            try
            {
                CheckIsAuthorized(EnumHelper.AuthorizeMethod.Select);
                VisitInput input = new VisitInput()
                {
                    CreatedUserId = WebUserManager.GetUserInfo().Id,
                    CreatedDate = DateTime.Now
                };
                return PartialView(input);
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

        public override ActionResult Edit(int id)
        {
            try
            {
                CheckIsAuthorized(EnumHelper.AuthorizeMethod.Select);
                var entity = repo.Get(id);
                if (entity.VisitLogs.Count > 0)
                {
                    string lastAct = entity.VisitLogs.LastOrDefault().Description ?? entity.VisitLogs.LastOrDefault().Description2;
                    ViewBag.lastAct = lastAct;
                }
                return PartialView(EditView, mapper.Map<Visit, VisitInput>(entity));
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
        public override ActionResult Create(VisitInput input)
        {
            try
            {
                if (!ModelState.IsValid)
                    return PartialView("_Form", input);
                string url = String.Empty;
                Visit entity;
                if (input.Id == 0)
                {
                    entity = mapper.Map<VisitInput, Visit>(input);
                    url = "/#/Visit/Index";
                    repo.Insert(entity);
                }
                else
                {
                    entity = mapper.Map<VisitInput, Visit>(input, repo.Get(input.Id));
                    logRepo.Insert(new VisitLog
                    {
                        AppUserId = WebUserManager.GetUserInfo().Id,
                        Date = DateTime.Now,
                        VisitId = input.Id,
                        Status = input.Status,
                        Description = input.VisitLog.Description,
                        Description2 = input.VisitLog.Description2
                    });
                }
                CheckIsAuthorized(EnumHelper.AuthorizeMethod.Insert);
                using (var scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    repo.Save();
                    logRepo.Save();
                    scope.Complete();
                }
                if (!string.IsNullOrEmpty(url))
                    return Json(new { Url = url, Content = "Kayıt Oluşturuldu" });
                else
                    return Json(new { Content = "Kayıt Güncellendi !" });
            }
            catch (Exception ex)
            {
                return Json(new { Error = ex.Message });
            }
        }

        public ActionResult VisitLogs(int id)
        {
            var data = logRepo.Where(s => s.VisitId == id).OrderByDescending(s => s.Id).ToList();
            return PartialView("_VisitLogs", data);
        }

        [HttpPost]
        public ActionResult GridGetItems(GridParams g, string parent, bool? restore)
        {
            CheckIsAuthorized(EnumHelper.AuthorizeMethod.Select);
            parent = (parent ?? string.Empty).ToLower();
            var isAdmin = WebUserManager.GetUserInfo().IsAdmin;

            if (restore.HasValue && restore.Value && WebUserManager.IsUpdateAuthorize(GetCode()))
            {
                repo.Restore(repo.Get(Convert.ToInt32(g.Key)));
                repo.Save();
            }

            var data = repo.Where(o => o.Customer.Name.ToLower().Contains(parent), isAdmin);
            var model = GetGridModelDto(g, data);
            return Json(model);
        }
    }
}