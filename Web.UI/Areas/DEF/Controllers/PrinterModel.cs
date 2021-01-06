using Core;
using Domain;
using Omu.AwesomeMvc;
using System.Linq;
using System.Web.Mvc;
using Web.UI.Controllers;
using Web.UI.Helper;
using Web.UI.Mappers;
using WebMarkupMin.AspNet4.Mvc;

namespace Web.UI.Areas.DEF.Controllers
{
    [JsonAllowGet]
    [MinifyHtml]
    public class PrinterModelController : GenericController<PrinterModel, PrinterModelInput, PrinterModelInput>
    {
        public PrinterModelController(IRepo<PrinterModel> repo, IMapper mapper)
            : base(repo, mapper)
        {

        }
        protected override object MapEntityToGridModel(PrinterModel model)
        {
            return new { model.Id, model.Name, PrinterBrand = model.PrinterBrand.Name, model.IsColor };
        }

        [HttpPost]
        public ActionResult GridGetItems(GridParams g, string parent, int? printerbrand)
        {
            CheckIsAuthorized(EnumHelper.AuthorizeMethod.Select);
            parent = (parent ?? string.Empty).ToLower();
            var isAdmin = WebUserManager.GetUserInfo().IsAdmin;
            var data = repo.Where(o => o.Name.ToLower().Contains(parent), isAdmin);
            if (printerbrand.HasValue)
                data = data.Where(q => q.PrinterBrandId == printerbrand.Value);
            var model = GetGridModelDto(g, data);
            return Json(model);
        }
    }
}