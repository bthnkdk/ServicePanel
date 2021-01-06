using Core;
using Domain;
using Omu.AwesomeMvc;
using System;
using System.Web.Mvc;
using Web.UI.Controllers;
using Web.UI.Helper;
using Web.UI.Mappers;
using WebMarkupMin.AspNet4.Mvc;

namespace Web.UI.Areas.SYS.Controllers
{
    [JsonAllowGet]
    [MinifyHtml]
    public class MenuController : XGenericController<Menu, MenuInput, MenuInput>
    {
        public MenuController(IRepo<Menu> repo, IMapper mapper)
            : base(repo, mapper)
        {
        }
        protected override object MapEntityToGridModel(Menu model)
        {
            return new { model.Id, model.Name, model.IsDeleted, AuthCode = model.AuthCode.Name, model.Url, model.Icon, model.ParentId, ParentName = model.Parent?.Name ?? "" };
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

            var data = repo.Where(o => o.Name.ToLower().Contains(parent), isAdmin);
            var model = GetGridModelDto(g, data);
            return Json(model);
        }

        [HttpPost]
        public override ActionResult Create(MenuInput input)
        {
            try
            {
                CheckIsAuthorized(EnumHelper.AuthorizeMethod.Insert);
                if (!ModelState.IsValid)
                    return PartialView(input);

                Menu model = mapper.Map<MenuInput, Menu>(input);
                var entity = repo.Insert(model);
                repo.Save();
                return Json(MapEntityToGridModel(entity));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return PartialView(input);
            }
        }

        [HttpPost]
        public override ActionResult Edit(MenuInput input)
        {
            try
            {
                CheckIsAuthorized(EnumHelper.AuthorizeMethod.Update);
                if (!ModelState.IsValid)
                    return PartialView(EditView, input);
                var entity = mapper.Map<MenuInput, Menu>(input, repo.Get(input.Id));
                repo.Save();
                return Json(MapEntityToGridModel(repo.Get(entity.Id)));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return PartialView("Create", input);
            }
        }
    }
}