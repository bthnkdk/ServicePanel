﻿using Core;
using Domain;
using Omu.AwesomeMvc;
using System;
using System.Web.Mvc;
using Web.UI.Controllers;
using Web.UI.Helper;
using Web.UI.Mappers;
using WebMarkupMin.AspNet4.Mvc;

namespace Web.UI.Areas.DEF.Controllers
{
    [JsonAllowGet]
    [MinifyHtml]
    public class CustomerTypeController : XGenericController<CustomerType, CustomerTypeInput, CustomerTypeInput>
    {
        public CustomerTypeController(IRepo<CustomerType> repo, IMapper mapper)
            : base(repo, mapper)
        {

        }
        protected override object MapEntityToGridModel(CustomerType model)
        {
            return new { model.Id, model.Name, model.IsDeleted, CustomerCount = model.Customers.Count };
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