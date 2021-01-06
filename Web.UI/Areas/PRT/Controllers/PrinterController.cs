using Core;
using Domain;
using Omu.AwesomeMvc;
using System;
using System.Linq;
using System.Web.Mvc;
using Web.UI.Controllers;
using Web.UI.Helper;
using Web.UI.Mappers;
using WebMarkupMin.AspNet4.Mvc;

namespace Web.UI.Areas.PRT.Controllers
{
    [JsonAllowGet]
    [MinifyHtml]
    public class PrinterController : XGenericController<Printer, PrinterInput, PrinterInput>
    {
        IRepo<Customer> customerRepo;
        public PrinterController(IRepo<Printer> repo, IRepo<Customer> customerRepo, IMapper mapper)
            : base(repo, mapper)
        {
            this.customerRepo = customerRepo;
        }
        protected override object MapEntityToGridModel(Printer model)
        {
            return new { LocationName = model.Location.Name, model.Name, model.SerialNumber, model.PrinterNumber, Model = model.PrinterModel.Name, Brand = model.PrinterModel.PrinterBrand.Name, CustomerName = model.Location.Customer.Name, CustomerCode = model.Location.Customer.CustomerCode, CustomerTypeName = model.Location.Customer.CustomerType.Name, model.Id, model.IsDeleted };
        }
        public ActionResult CustomerPrinters(int customerId)
        {
            CheckIsAuthorized(EnumHelper.AuthorizeMethod.Select);
            return PartialView("_CustomerPrinters", customerId);
        }
        public override ActionResult Create()
        {
            try
            {
                //int customerId = 0;
                //if (!String.IsNullOrEmpty(Request["customerId"]))
                //    customerId = Convert.ToInt32(Request["customerId"]);
                //if (customerId < 1)
                //    throw new Exception("Firma bilgisi olmadan yazıcı oluşturulamaz.");

                CheckIsAuthorized(EnumHelper.AuthorizeMethod.Select);

                PrinterInput input = new PrinterInput
                {
                    RowId = Guid.NewGuid(),
                    CreatedDate = DateTime.Now,
                    CreatedUserId = WebUserManager.GetUserInfo().Id,
                    //CustomerId = customerId,
                    //CustomerName = customerRepo.Get(customerId).Name,
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
        public override ActionResult Edit(int id)
        {
            try
            {
                CheckIsAuthorized(EnumHelper.AuthorizeMethod.Select);
                var entity = repo.Get(id);
                var input = mapper.Map<Printer, PrinterInput>(entity);
                input.CustomerName = entity.Location.Customer.Name;
                input.CustomerId = entity.Location.CustomerId;
                input.BrandId = entity.PrinterModel.PrinterBrandId;

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
        public override ActionResult Create(PrinterInput input)
        {
            try
            {
                CheckIsAuthorized(EnumHelper.AuthorizeMethod.Insert);

                if (!ModelState.IsValid)
                    return PartialView(input);

                var entity = repo.Insert(mapper.Map<PrinterInput, Printer>(input));
                var movementEntity = mapper.Map<PrinterInput, PrinterMovement>(input);
                movementEntity.MoveDate = DateTime.Now;
                entity.PrinterMovements.Add(movementEntity);

                repo.Save();

                return Json(MapEntityToGridModel(repo.Get(entity.Id)));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return PartialView(input);
            }
        }
        public override ActionResult Edit(PrinterInput input)
        {
            try
            {
                CheckIsAuthorized(EnumHelper.AuthorizeMethod.Update);
                if (!ModelState.IsValid)
                    return PartialView(EditView, input);

                var originalEntity = repo.Get(input.Id);
                int originalLocationId = originalEntity.LocationId;

                var entity = mapper.Map<PrinterInput, Printer>(input, originalEntity);

                if (originalLocationId != input.LocationId) // Lokasyon değişirse
                {
                    var movementEntity = mapper.Map<PrinterInput, PrinterMovement>(input);
                    movementEntity.MoveDate = DateTime.Now;
                    entity.PrinterMovements.Add(movementEntity);
                }

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
        public ActionResult GridGetItems(GridParams g, string parent, bool? restore, int? customerId)
        {
            CheckIsAuthorized(EnumHelper.AuthorizeMethod.Select);
            parent = (parent ?? string.Empty).ToLower();
            var isAdmin = WebUserManager.GetUserInfo().IsAdmin;
            if (restore.HasValue && restore.Value && WebUserManager.IsUpdateAuthorize(GetCode()))
            {
                repo.Restore(repo.Get(Convert.ToInt32(g.Key)));
                repo.Save();
            }
            var data = repo.Where(q => q.PrinterModel.Name.ToLower().Contains(parent), isAdmin);
            if (customerId.HasValue)
                data = data.Where(s => s.Location.CustomerId == customerId.Value);

            var model = GetGridModelDto(g, data);
            return Json(model);
        }
    }
}