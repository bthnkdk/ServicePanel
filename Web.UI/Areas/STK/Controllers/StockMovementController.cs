using Core;
using Domain;
using Omu.AwesomeMvc;
using System.Web.Mvc;
using Web.UI.Controllers;
using Web.UI.Helper;
using Web.UI.Mappers;
using WebMarkupMin.AspNet4.Mvc;

namespace Web.UI.Areas.STK.Controllers
{
    [JsonAllowGet]
    [MinifyHtml]
    public class StockMovementController : ROGenericController<StockMovement>
    {
        public StockMovementController(IRepo<StockMovement> repo)
            : base(repo)
        {
            
        }
        protected override object MapEntityToGridModel(StockMovement model)
        {
            return new { model.Id, LocationName = model.Location.Name, StockName = model.Stock.Name, model.Count,model.Price, CreatedUserName=model.CreateAppUser.FullName,AppUserName=model.AppUser.FullName, CreatedDate = model.Date.ToString("dd.MM.yyyy"), model.Action };
        }

        [HttpPost]
        public ActionResult GridGetItems(GridParams g, string parent)
        {
            
            var data = repo.GetAll();
            var model = GetGridModelDto(g, data);
            return Json(model);
        }
        public ActionResult Details(int id)
        {
            var data = repo.Get(id);
            return PartialView("_Details",data);
        }
    }
}