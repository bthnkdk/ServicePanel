using Core;
using Domain;
using Omu.AwesomeMvc;
using System;
using System.Web.Mvc;
using Web.UI.Controllers;
using Web.UI.Helper;
using Web.UI.Mappers;
using WebMarkupMin.AspNet4.Mvc;

namespace Web.UI.Areas.PRT.Controllers
{
    [JsonAllowGet]
    [MinifyHtml]
    public class PrinterMovementController : GenericController<PrinterMovement, PrinterMovementInput, PrinterMovementInput>
    {
        public PrinterMovementController(IRepo<PrinterMovement> repo, IMapper mapper)
            : base(repo, mapper)
        {

        }
        protected override object MapEntityToGridModel(PrinterMovement model)
        {
            return new { model.Id };
        }


        [HttpPost]
        public ActionResult GridGetItems(GridParams g, string parent)
        {
            CheckIsAuthorized(EnumHelper.AuthorizeMethod.Select);
            //parent = (parent ?? string.Empty).ToLower();
            var isAdmin = WebUserManager.GetUserInfo().IsAdmin;
            var data = repo.GetAll();
            var model = GetGridModelDto(g, data);
            return Json(model);
        }
    }
}