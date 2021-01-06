using Core;
using Domain;
using Omu.AwesomeMvc;
using System.Web.Mvc;
using Web.UI.Controllers;
using Web.UI.Helper;
using Web.UI.Mappers;
using WebMarkupMin.AspNet4.Mvc;

namespace Web.UI.Areas.DEF.Controllers
{
    [JsonAllowGet]
    [MinifyHtml]
    public class PreApplicationTypeController : GenericController<PreApplicationType, PreApplicationTypeInput, PreApplicationTypeInput>
    {
        public PreApplicationTypeController(IRepo<PreApplicationType> repo, IMapper mapper)
            : base(repo, mapper)
        {

        }
        protected override object MapEntityToGridModel(PreApplicationType model)
        {
            return new { model.Id, model.Name, };
        }

        [HttpPost]
        public ActionResult GridGetItems(GridParams g, string parent)
        {
            CheckIsAuthorized(EnumHelper.AuthorizeMethod.Select);
            parent = (parent ?? string.Empty).ToLower();
            var isAdmin = WebUserManager.GetUserInfo().IsAdmin;
            var data = repo.Where(o => o.Name.ToLower().Contains(parent), isAdmin);
            var model = GetGridModelDto(g, data);
            return Json(model);
        }
    }
}