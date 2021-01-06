using Core;
using Domain;
using Omu.AwesomeMvc;
using System;
using System.Web.Mvc;
using Web.UI.Controllers;
using Web.UI.Helper;
using Web.UI.Mappers;
using Web.UI.ViewModels;
using WebMarkupMin.AspNet4.Mvc;

namespace Web.UI.Areas.CRM.Controllers
{
    [JsonAllowGet]
    [MinifyHtml]
    public class LocationController : XGenericController<Location, LocationInput, LocationInput>
    {
        IRepo<Customer> customerRepo;
        public LocationController(IRepo<Location> repo, IRepo<Customer> customerRepo, IMapper mapper)
            : base(repo, mapper)
        {
            this.customerRepo = customerRepo;
        }
        protected override object MapEntityToGridModel(Location model)
        {
            return new { model.Id, model.Name, model.ResponsibleName, model.IsDeleted };
        }
        public override ActionResult Create()
        {
            try
            {
                LocationInput input = new LocationInput();
                input.CreatedDate = DateTime.Now;
                input.CreatedUserId = WebUserManager.GetUserInfo().Id;
                input.RowId = Guid.NewGuid();
                if (!String.IsNullOrEmpty(Request["customerId"]))
                    input.CustomerId = Convert.ToInt32(Request["customerId"]);
                if (!customerRepo.Any(s => s.Id == input.CustomerId))
                    throw new Exception("Müşteri bulunamadı");

                CheckIsAuthorized(EnumHelper.AuthorizeMethod.Select);
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
                var input = mapper.Map<Location, LocationInput>(entity);
                if (entity.DefaultAppUserId.HasValue)
                    input.DefaultDepartmentId = entity.DefaultAppUser.DepartmentId;

                return PartialView(EditView, input);
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
        public override ActionResult Create(LocationInput input)
        {
            try
            {
                if (!ModelState.IsValid)
                    return PartialView(input);
                string url = String.Empty;

                Location entity;
                if (input.Id == 0)
                    entity = repo.Insert(mapper.Map<LocationInput, Location>(input));
                else
                {
                    input.UpdateDate = DateTime.Now;
                    input.UpdateUserId = WebUserManager.GetUserInfo().Id;
                    entity = mapper.Map<LocationInput, Location>(input, repo.Get(input.Id));
                }
                CheckIsAuthorized(EnumHelper.AuthorizeMethod.Insert);
                repo.Save();
                if (!string.IsNullOrEmpty(url))
                    return Json(new { Url = url });
                else
                    return Json(MapEntityToGridModel(repo.Get(entity.Id)));
            }
            catch (Exception ex)
            {
                return PartialView("_Error", ex.Message);
            }
        }
        public override ActionResult Delete(DeleteConfirmInput input)
        {
            CheckIsAuthorized(EnumHelper.AuthorizeMethod.Delete);
            var entity = repo.Get(input.Id);
            if (entity.Name == "Merkez")
                return PartialView("_Error", "Merkez lokasyonu silinemez.");

            repo.Delete(entity);
            repo.Save();
            return Json(new { input.Id });
        }
        public ActionResult CustomerLocations(int customerId)
        {
            CheckIsAuthorized(EnumHelper.AuthorizeMethod.Select);
            return PartialView("_CustomerLocations", customerId);
        }
        [HttpPost]
        public ActionResult GridGetItems(GridParams g, int customerId, string parent, bool? restore)
        {
            CheckIsAuthorized(EnumHelper.AuthorizeMethod.Select);
            parent = (parent ?? string.Empty).ToLower();
            var isDeleteAuthorize = WebUserManager.IsDeleteAuthorize(GetCode());
            if (restore.HasValue && restore.Value && WebUserManager.IsUpdateAuthorize(GetCode()))
            {
                repo.Restore(repo.Get(Convert.ToInt32(g.Key)));
                repo.Save();
            }
            var data = repo.Where(s => s.CustomerId == customerId && (s.ResponsibleName.ToLower().Contains(parent) || s.Name.ToLower().Contains(parent)), isDeleteAuthorize);
            var model = GetGridModelDto(g, data);
            return Json(model);
        }
    }
}