using Core;
using Domain;
using Omu.AwesomeMvc;
using System;
using System.Web.Mvc;
using Web.UI.Controllers;
using Web.UI.Helper;
using Web.UI.Mappers;
using WebMarkupMin.AspNet4.Mvc;

namespace Web.UI.Areas.CRM.Controllers
{
    [JsonAllowGet]
    [MinifyHtml]
    public class PreApplicationController : XGenericController<PreApplication, PreApplicationInput, PreApplicationInput>
    {
        public PreApplicationController(IRepo<PreApplication> repo, IMapper mapper)
            : base(repo, mapper)
        {

        }
        protected override object MapEntityToGridModel(PreApplication model)
        {
            return new
            {
                model.Id,
                model.CustomerName,
                model.CustomerAuthorized,
                PreApplicationTypeName = model.PreApplicationType.Name,
                PreApplicationStatusName = model.PreApplicationStatus.Name,
                CreatedDate = model.CreatedDate.ToString("dd.MM.yyyy"),
                CreatedUserName = model.CreatedUser.Firstname + " " + model.CreatedUser.Lastname,
                UpdatedDate = model.UpdatedDate.HasValue ? model.UpdatedDate.Value.ToString("dd.MM.yyyy") : "",
                UpdatedUserName = (model.UpdatedUser?.Firstname ?? "") + " " + (model.UpdatedUser?.Lastname ?? ""),
                model.IsDeleted
            };
        }

        public override ActionResult Create()
        {
            try
            {
                CheckIsAuthorized(EnumHelper.AuthorizeMethod.Select);
                PreApplicationInput input = new PreApplicationInput()
                {
                    CreatedDate = DateTime.Now,
                    CreatedUserId = WebUserManager.GetUserInfo().Id
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

        [HttpPost]
        public override ActionResult Edit(PreApplicationInput input)
        {
            try
            {
                CheckIsAuthorized(EnumHelper.AuthorizeMethod.Update);
                if (!ModelState.IsValid)
                    return PartialView(EditView, input);
                input.UpdatedDate = DateTime.Now;
                input.UpdatedUserId = WebUserManager.GetUserInfo().Id;
                var entity = mapper.Map<PreApplicationInput, PreApplication>(input, repo.Get(input.Id));
                repo.Save();
                return Json(MapEntityToGridModel(repo.Get(entity.Id)));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return PartialView("Create", input);
            }
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

            var data = repo.Where(o => o.CustomerName.ToLower().Contains(parent), isAdmin);
            var model = GetGridModelDto(g, data);
            return Json(model);
        }
    }
}