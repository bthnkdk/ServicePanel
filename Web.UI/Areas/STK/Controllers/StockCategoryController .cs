﻿using Core;
using Domain;
using Omu.AwesomeMvc;
using System;
using System.Web.Mvc;
using Web.UI.Controllers;
using Web.UI.Helper;
using Web.UI.Mappers;
using WebMarkupMin.AspNet4.Mvc;

namespace Web.UI.Areas.STK.Controllers
{
    [JsonAllowGet]
    [MinifyHtml]
    public class StockCategoryController : XGenericController<StockCategory, StockCategoryInput, StockCategoryInput>
    {
        public StockCategoryController(IRepo<StockCategory> repo, IMapper mapper)
            : base(repo, mapper)
        {

        }
        protected override object MapEntityToGridModel(StockCategory model)
        {
            return new { model.Id, model.Name, MainStockCategory = model.StockMainCategory.Name, model.IsDeleted };
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
    }
}