using Core;
using Domain;
using Omu.AwesomeMvc;
using System;
using System.Linq;
using System.Transactions;
using System.Web.Mvc;
using Web.UI.Controllers;
using Web.UI.Helper;
using Web.UI.Mappers;
using WebMarkupMin.AspNet4.Mvc;

namespace Web.UI.Areas.SVC.Controllers
{
    [JsonAllowGet]
    [MinifyHtml]
    public class ServiceController : GenericController<Service, ServiceInput, ServiceInput>
    {
        IRepo<ServicePrinter> servicePrinterRepo;
        IRepo<ServiceStock> serviceStockRepo;
        IRepo<ServiceVehicle> serviceVehicleRepo;
        IRepo<ServicePerson> servicePersonRepo;
        CounterHelper counterHelper;

        public ServiceController(IRepo<Service> repo, IRepo<ServicePrinter> servicePrinterRepo, IRepo<ServiceStock> serviceStockRepo, IRepo<ServicePerson> servicePersonRepo, IRepo<Counter> counterRepo, IRepo<TonerChange> tonerChangeRepo, IRepo<ServiceVehicle> serviceVehicleRepo, IMapper mapper)
            : base(repo, mapper)
        {
            this.servicePrinterRepo = servicePrinterRepo;
            this.serviceVehicleRepo = serviceVehicleRepo;
            this.servicePersonRepo = servicePersonRepo;
            this.serviceStockRepo = serviceStockRepo;

            counterHelper = new CounterHelper(counterRepo, tonerChangeRepo, mapper);
        }

        protected override object MapEntityToGridModel(Service model)
        {
            return new { CustomerName = model.Location.Customer.Name, LocationName = model.Location.Name, ServiceCategoryName = model.ServiceCategory.Name, model.Priority, model.Status, ServiceDate = model.ServiceDate.ToString("dd MMMM yyyy"), ServicePersons = String.Join(",", model.ServicePersons.Select(s => s.AppUser.FullName)), model.Id };
        }

        [HttpPost]
        public ActionResult GridGetItems(GridParams g, string parent, int? customerId, int? serviceCategoryId, int? servicePersonalId, int? serviceTownId, int? serviceStatusId)
        {
            CheckIsAuthorized(EnumHelper.AuthorizeMethod.Select);
            parent = (parent ?? string.Empty).ToLower();
            var isAdmin = WebUserManager.GetUserInfo().IsAdmin;
            var data = repo.Where(s => s.Location.Customer.Name.ToLower().Contains(parent), isAdmin);
            if (customerId.HasValue)
                data = data.Where(s => s.Location.CustomerId == customerId.Value);
            if (serviceStatusId.HasValue)
                data = data.Where(s => s.Status == serviceStatusId);
            if (serviceCategoryId.HasValue)
                data = data.Where(s => s.ServiceCategory.Id == serviceCategoryId);
            if (servicePersonalId.HasValue)
                data = data.Where(s => s.CreatedUser.Id == servicePersonalId);
            if (serviceTownId.HasValue)
                data = data.Where(s => s.Location.Town.Id == serviceTownId);
            var model = GetGridModelDto(g, data);
            return Json(model);
        }

        public override ActionResult Index()
        {
            var data = repo.GetAll();
            ServiceListViewModel svcModel = new ServiceListViewModel();
            svcModel.AllServiceCount = data.Count();
            svcModel.WaitApprovalCount = data.Where(s => s.Status == ConstHelper.ServiceStatus.URUNONAYIBEKLIYOR).Count();
            svcModel.CompletedServiceCount = data.Where(s => s.Status == ConstHelper.ServiceStatus.TAMAMLANDI).Count();
            svcModel.ServiceCategories = (from s in data
                                          group s by s.ServiceCategory into grp
                                          select new ServiceCategoryViewModel()
                                          {
                                              ServiceCategoryId = grp.Key.Id,
                                              Name = grp.Key.Name,
                                              Count = grp.Count()
                                          }).ToList();
            svcModel.ServicePersonals = (from s in data
                                         group s by s.CreatedUser into grp
                                         select new ServicePersonalViewModel()
                                         {
                                             ServicePersonalId = grp.Key.Id,
                                             Name = grp.Key.Firstname + " " + grp.Key.Lastname,
                                             Count = grp.Count()
                                         }).ToList();
            svcModel.ServiceTowns = (from s in data.Select(s => s.Location)
                                     group s by s.Town into grp
                                     select new ServiceTownViewModel()
                                     {
                                         ServiceTownId = grp.Key.Id,
                                         Name = grp.Key.Name,
                                         Count = grp.Count()
                                     }).ToList();
            return PartialView("Index", svcModel);
        }

        public override ActionResult Create()
        {
            try
            {
                int userId = WebUserManager.GetUserInfo().Id;
                CheckIsAuthorized(EnumHelper.AuthorizeMethod.Select);
                ServiceInput input = new ServiceInput()
                {
                    CreatedDate = DateTime.Now,
                    CreatedUserId = userId,
                    RowId = Guid.NewGuid(),
                    ServiceDate = DateTime.Now,
                    Priority = ConstHelper.ServicePriority.NORMAL,
                    Status = ConstHelper.ServiceStatus.YENI
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
        public override ActionResult Create(ServiceInput input)
        {
            try
            {
                CheckIsAuthorized(EnumHelper.AuthorizeMethod.Insert);

                if (!ModelState.IsValid)
                    return Json(new { ErrorList = ExceptionHelper.ValidationErrors(ModelState) });

                var userId = WebUserManager.GetUserInfo().Id;

                Service entity;
                if (input.Id == 0)
                {
                    entity = mapper.Map<ServiceInput, Service>(input);
                    repo.Insert(entity);
                }
                else
                {
                    input.UpdateDate = DateTime.Now;
                    input.UpdateUserId = WebUserManager.GetUserInfo().Id;
                    entity = mapper.Map<ServiceInput, Service>(input, repo.Get(input.Id));

                    var removedVehicles = entity.ServiceVehicles.ToList();
                    if (input.ServiceVehicles != null && input.ServiceVehicles.Length > 0)
                    {
                        removedVehicles = removedVehicles.Where(s => !input.ServiceVehicles.Contains(s.VehicleId)).ToList();
                        var addedVehicles = input.ServiceVehicles.Where(s => !entity.ServiceVehicles.Select(p => p.VehicleId).Contains(s)).ToArray();

                        foreach (var addedVehicle in addedVehicles)
                            entity.ServiceVehicles.Add(new ServiceVehicle { VehicleId = addedVehicle });
                    }

                    foreach (var removedVehicle in removedVehicles)
                    {
                        var removedVehicleEntity = entity.ServiceVehicles.SingleOrDefault(s => s.VehicleId == removedVehicle.VehicleId);

                        if (removedVehicleEntity != null)
                            serviceVehicleRepo.Delete(removedVehicleEntity);
                    }


                    var removedPersons = entity.ServicePersons.ToList();

                    if (input.ServicePersons != null && input.ServicePersons.Length > 0)
                    {
                        removedPersons = entity.ServicePersons.Where(s => !input.ServicePersons.Contains(s.AppUserId)).ToList();
                        var addedPersons = input.ServicePersons.Where(s => !entity.ServicePersons.Select(p => p.AppUserId).Contains(s)).ToArray();

                        foreach (var addedPerson in addedPersons)
                            entity.ServicePersons.Add(new ServicePerson { AppUserId = addedPerson });
                    }

                    foreach (var removedPerson in removedPersons)
                    {
                        var removedPersonEntity = entity.ServicePersons.SingleOrDefault(s => s.AppUserId == removedPerson.AppUserId);

                        if (removedPersonEntity != null)
                            servicePersonRepo.Delete(removedPersonEntity);
                    }

                    foreach (var servicePrinter in input.ServicePrinters.Where(s => s.IsDeleted != 1))
                    {
                        var printerEntity = entity.ServicePrinters.SingleOrDefault(s => s.Id == servicePrinter.Id);

                        if (printerEntity != null)
                        {
                            printerEntity.Color = servicePrinter.Color;
                            printerEntity.Description = servicePrinter.Description;
                            printerEntity.IsMaintenanceOk = servicePrinter.IsMaintenanceOk;
                            printerEntity.Mono = servicePrinter.Mono;
                            printerEntity.PrinterId = servicePrinter.PrinterId;
                            printerEntity.PrinterServiceTypeId = servicePrinter.PrinterServiceTypeId;
                            printerEntity.Process = servicePrinter.Process;
                            printerEntity.Status = servicePrinter.Status;
                            if (printerEntity.CounterId.HasValue)
                                counterHelper.DeleteCounter(printerEntity.CounterId.Value);
                            if (servicePrinter.Color.HasValue || servicePrinter.Mono.HasValue)
                                printerEntity.CounterId = counterHelper.SetCounter(new PRT.CounterInput
                                {
                                    Mono = servicePrinter.Mono,
                                    Color = servicePrinter.Color,
                                    PrinterId = servicePrinter.PrinterId
                                });
                        }
                        else
                        {
                            entity.ServicePrinters.Add(mapper.Map<ServicePrinterInput, ServicePrinter>(servicePrinter));

                            if (servicePrinter.Color.HasValue || servicePrinter.Mono.HasValue)
                                printerEntity.CounterId = counterHelper.SetCounter(new PRT.CounterInput
                                {
                                    Mono = servicePrinter.Mono,
                                    Color = servicePrinter.Color,
                                    PrinterId = servicePrinter.Id
                                });
                        }
                    }

                    foreach (var removedPrinter in input.ServicePrinters.Where(s => s.IsDeleted == 1))
                    {
                        var removedPrinterEntity = entity.ServicePrinters.SingleOrDefault(s => s.PrinterId == removedPrinter.PrinterId);

                        if (removedPrinterEntity != null)
                        {
                            servicePrinterRepo.Delete(removedPrinterEntity);

                            if (removedPrinterEntity.CounterId.HasValue)
                                counterHelper.DeleteCounter(removedPrinterEntity.CounterId.Value);
                        }
                    }

                    input.ServicePrinters.RemoveAll(s => s.IsDeleted == -1);

                    //silinmemişse ve durumu depo çıkışı bekliyorsa veya (durumu zimmetlenmiş ve o servise ait bir personel ise) düzenleyebilir

                    foreach (var serviceStock in input.ServiceStocks.Where(s => s.IsDeleted != 1 &&
                    (s.Status == ConstHelper.ServiceStockStatus.DEPOCIKISIBEKLENIYOR ||
                    (s.Status == ConstHelper.ServiceStockStatus.ZIMMETLENDI && entity.ServicePersons.Any(p => p.AppUserId == userId)))))
                    {
                        var stockEntity = entity.ServiceStocks.SingleOrDefault(s => s.Id == serviceStock.Id);

                        if (stockEntity != null)
                        {
                            stockEntity.Count = serviceStock.Count;
                            stockEntity.IsDelivered = serviceStock.IsDelivered;
                            stockEntity.PrinterId = serviceStock.PrinterId;
                            if (serviceStock.Status == ConstHelper.ServiceStockStatus.ZIMMETLENDI)
                                stockEntity.Status = serviceStock.IsDelivered ? ConstHelper.ServiceStockStatus.TESLIMEDILDI : stockEntity.Status;
                        }
                        else
                            entity.ServiceStocks.Add(mapper.Map<ServiceStockInput, ServiceStock>(serviceStock));
                    }

                    foreach (var removedStock in input.ServiceStocks.Where(s => s.IsDeleted == 1 && s.Status != ConstHelper.ServiceStockStatus.TESLIMEDILDI))
                    {
                        var removedStockEntity = entity.ServiceStocks.SingleOrDefault(s => s.StockId == removedStock.StockId);

                        if (removedStockEntity != null)
                            serviceStockRepo.Delete(removedStockEntity);
                    }

                    input.ServicePrinters.RemoveAll(s => s.IsDeleted == -1);
                }

                using (var scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    repo.Save();
                    servicePrinterRepo.Save();
                    scope.Complete();
                }
                if (input.Id == 0)
                    return Json(new { Content = "Kayıt Oluşturuldu", Id = entity.Id });
                else
                    return Json(new { Url = "/#/Service/Index", Content = "Kayıt Güncellendi !" });
            }
            catch (Exception ex)
            {
                return Json(new { Error = ex.Message });
            }
        }

        public override ActionResult Edit(int id)
        {
            try
            {
                CheckIsAuthorized(EnumHelper.AuthorizeMethod.Select);

                var entity = repo.Get(id);
                var input = mapper.Map<Service, ServiceInput>(entity);
                input.CustomerName = entity.Location.Customer.Name;
                input.CustomerId = entity.Location.CustomerId;
                input.LocationName = entity.Location.Name;
                input.ServicePrinters = entity.ServicePrinters.Select(s => mapper.Map<ServicePrinter, ServicePrinterInput>(s)).ToList();
                input.ServicePersons = entity.ServicePersons.Select(s => s.AppUserId).ToArray();
                input.ServiceVehicles = entity.ServiceVehicles.Select(s => s.VehicleId).ToArray();
                input.ServiceStocks = entity.ServiceStocks.Select(s =>
                new ServiceStockInput
                {
                    Count = s.Count,
                    Id = s.Id,
                    IsDelivered = s.IsDelivered,
                    PrinterId = s.PrinterId,
                    Status = s.Status,
                    StatusName = ConstHelper.ServiceStockStatusName(s.Status),
                    StockId = s.StockId,
                    StockMovementId = s.StockMovementId,
                    StockName = s.Stock.Name,
                }).ToList();

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

        public ActionResult CustomerServices(int customerId)
        {
            CheckIsAuthorized(EnumHelper.AuthorizeMethod.Select);
            return PartialView("_CustomerServices", customerId);
        }

    }
}