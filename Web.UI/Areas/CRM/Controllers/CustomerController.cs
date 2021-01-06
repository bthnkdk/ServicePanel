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
    public class CustomerController : XGenericController<Customer, CustomerInput, CustomerInput>
    {
        public CustomerController(IRepo<Customer> repo, IMapper mapper)
            : base(repo, mapper)
        {

        }
        protected override object MapEntityToGridModel(Customer model)
        {
            return new { model.Id, model.Name, CustomerTypeName = model.CustomerType.Name, model.CustomerCode, model.IsDeleted };
        }
        public override ActionResult Create()
        {
            try
            {
                int userId = WebUserManager.GetUserInfo().Id;
                CheckIsAuthorized(EnumHelper.AuthorizeMethod.Select);
                CustomerInput input = new CustomerInput()
                {
                    CreatedDate = DateTime.Now,
                    CreatedUserId = userId,
                    RowId = Guid.NewGuid(),
                    Location = new LocationInput
                    {
                        CreatedDate = DateTime.Now,
                        CreatedUserId = userId,
                        RowId = Guid.NewGuid()

                    }
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
        public override ActionResult Create(CustomerInput input)
        {
            try
            {
                if (!ModelState.IsValid)
                    return PartialView("_Form", input);
                string url = String.Empty;
                Customer entity;
                if (input.Id == 0)
                {
                    if (repo.Any(s=>s.VerNo==input.VerNo))
                        throw new Exception("Bu vergi numarasına ait müşteri mevcut");
                    entity = mapper.Map<CustomerInput, Customer>(input);
                    Location location = mapper.Map<LocationInput, Location>(input.Location);
                    entity.Locations.Add(location);
                    url = "/#/Customer/Index";
                    repo.Insert(entity);
                }
                else
                {
                    input.UpdateDate = DateTime.Now;
                    input.UpdateUserId = WebUserManager.GetUserInfo().Id;
                    entity = mapper.Map<CustomerInput, Customer>(input, repo.Get(input.Id));
                }
                CheckIsAuthorized(EnumHelper.AuthorizeMethod.Insert);
                repo.Save();
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
        [HttpPost]
        public ActionResult GridGetItems(GridParams g)
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