using Core;
using Domain;
using Omu.AwesomeMvc;
using System;
using System.Globalization;
using System.Linq;
using System.Transactions;
using System.Web.Mvc;
using Web.UI.Controllers;
using Web.UI.Helper;
using Web.UI.Mappers;
using WebMarkupMin.AspNet4.Mvc;

namespace Web.UI.Areas.STK.Controllers
{
    [JsonAllowGet]
    [MinifyHtml]
    public class StockController : XGenericController<Stock, StockInput, StockInput>
    {
        IRepo<StockUserInventory> stockUserInventoryRepo;
        IRepo<StockMovement> stockMovementRepo;
        public StockController(IRepo<Stock> repo, IRepo<StockUserInventory> stockUserInventoryRepo, IRepo<StockMovement> stockMovementRepo, IMapper mapper)
            : base(repo, mapper)
        {
            this.stockUserInventoryRepo = stockUserInventoryRepo;
            this.stockMovementRepo = stockMovementRepo;
        }

        protected override object MapEntityToGridModel(Stock model)
        {
            return new { model.Id, model.Name, StockCategoryName = model.StockCategory.Name, model.Count, model.Barcode, model.IsDeleted };
        }

        public override ActionResult Create()
        {
            try
            {
                CheckIsAuthorized(EnumHelper.AuthorizeMethod.Select);
                StockInput input = new StockInput()
                {
                    RowId = GuidHelper.NewSequentialGuid()
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
            var data = repo.Where(q => q.Name.ToLower().Contains(parent), isAdmin);
            var model = GetGridModelDto(g, data);
            return Json(model);
        }

        public ActionResult AddProduct()
        {
            try
            {
                CheckIsAuthorized(EnumHelper.AuthorizeMethod.Update);
                return PartialView();
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
        public ActionResult AddProduct(ProductInput input)
        {
            try
            {
                CheckIsAuthorized(EnumHelper.AuthorizeMethod.Update);
                var userInfo = WebUserManager.GetUserInfo();
                if (!ModelState.IsValid)
                    return PartialView(input);

                var stock = repo.Get(input.StockId);
                if (stock != null)
                {
                    try
                    {
                        stock.Count += input.Count;
                        stock.StockMovements.Add(new StockMovement
                        {
                            Count = input.Count,
                            LocationId = 6,
                            AppUserId = userInfo.Id,
                            Date = DateTime.Now,
                            Price = stock.PriceBuy,
                            Action=ConstHelper.StockMovementAction.GIRIS,
                            CreateAppUserId = userInfo.Id
                        });
                    }
                    catch (Exception ex)
                    {
                        return PartialView("_Error", ex.Message);
                    }
                }

                repo.Save();

                return Json(MapEntityToGridModel(repo.Get(stock.Id)));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return PartialView("AddProduct", input);
            }
        }

        public ActionResult ExitProduct()
        {
            try
            {
                CheckIsAuthorized(EnumHelper.AuthorizeMethod.Update);
                return PartialView(new ExitProductInput());
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
        public ActionResult ExitProduct(ExitProductInput input)
        {
            try
            {
                CheckIsAuthorized(EnumHelper.AuthorizeMethod.Update);
                if (input.Products != null)
                    input.Products.RemoveAll(s => s.IsDeleted == 1);

                if (!ModelState.IsValid)
                    return PartialView(input);

                foreach (var product in input.Products)
                {
                    var productEntity = repo.Get(product.StockId);
                    if (productEntity.Count < product.Count)
                    {
                        ModelState.AddModelError("", productEntity.Name + " için stokta yeterli sayıda ürün yok. Stok : " + productEntity.Count);
                        return PartialView(input);
                    }
                    productEntity.Count -= product.Count;
                    productEntity.StockMovements.Add(new StockMovement
                    {
                        AppUserId = WebUserManager.GetUserInfo().Id,
                        Count = product.Count,
                        Date = DateTime.Now,
                        LocationId = input.LocationId,
                        Action = ConstHelper.StockMovementAction.CIKIS,
                        CreateAppUserId = WebUserManager.GetUserInfo().Id
                    });
                }
                repo.Save();

                return Json(new { Content = "Çıkış işlemi başarılı !" });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return PartialView("AddProduct", input);
            }
        }

        public ActionResult DebitProduct()
        {
            try
            {
                CheckIsAuthorized(EnumHelper.AuthorizeMethod.Update);
                return PartialView(new DebitProductInput());
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
        public ActionResult DebitProduct(DebitProductInput input)
        {

            CheckIsAuthorized(EnumHelper.AuthorizeMethod.Update);

            if (input.StockList != null)
                input.StockList.RemoveAll(s => s.IsDeleted == 1);

            if (!ModelState.IsValid || input.StockList == null || input.StockList.Count < 1 || input.AppUserId < 1)
            {
                if (input.StockList == null || input.StockList.Where(s => s.IsDeleted == 0).Count() < 1)
                    ModelState.AddModelError("", "Lütfen ürün ekleyiniz");

                if (input.AppUserId < 1)
                    ModelState.AddModelError("", "Lütfen personel seçiniz");

                return PartialView(input);
            }

            try
            {
                var userId = WebUserManager.GetUserInfo().Id;

                var personInventory = stockUserInventoryRepo.Where(s => s.AppUserId == input.AppUserId).ToList();


                foreach (var stock in input.StockList.Where(s => s.IsDeleted == 0))
                {
                    var stockEntity = repo.Where(s => s.Id == stock.Id).FirstOrDefault();

                    if (stockEntity.Count <= stock.Count)
                    {
                        ModelState.AddModelError("", stockEntity.Name + " için stokta yeterli sayıda ürün yok. Stok : " + stockEntity.Count);
                        return PartialView(input);
                    }

                    stockEntity.Count -= stock.Count;

                    var inInventoryStock = personInventory.SingleOrDefault(s => s.StockId == stock.Id);

                    if (inInventoryStock != null)
                        inInventoryStock.Count += stock.Count;
                    else
                        stockUserInventoryRepo.Insert(new StockUserInventory
                        {
                            AppUserId = input.AppUserId,
                            Count = stock.Count,
                            StockId = stock.Id
                        });

                    stockMovementRepo.Insert(new StockMovement
                    {
                        AppUserId = input.AppUserId,
                        CreateAppUserId = userId,
                        Count = stock.Count,
                        StockId = stock.Id,
                        LocationId = ApplicationSettingHelper.BaseLocationId,
                        Date = DateTime.Now,
                        Price = 0,
                        Action = ConstHelper.StockMovementAction.ZIMMET
                    });
                }
                using (TransactionScope scope = new TransactionScope())
                {
                    repo.Save();
                    stockMovementRepo.Save();
                    stockUserInventoryRepo.Save();
                    scope.Complete();
                }

                return Json(new { Content = "Zimmetleme işlemi başarılı !" });
            }
            catch (Exception ex)
            {
                return PartialView("_Error", ex.Message);
            }
        }

        public ActionResult DebitList()
        {
            CheckIsAuthorized(EnumHelper.AuthorizeMethod.Select);

            var debitList = stockUserInventoryRepo.GetAll(true).GroupBy(s => new { s.AppUserId, FullName = s.AppUser.Firstname + " " + s.AppUser.Lastname })
                .Select(s => new StockDebitListModel { FullName = s.Key.FullName, AppUserId = s.Key.AppUserId, Count = s.Count() }).ToList();

            return PartialView(debitList);
        }


        public ActionResult ExitProductFromPerson(int appUserId)
        {
            try
            {
                CheckIsAuthorized(EnumHelper.AuthorizeMethod.Update);
                var model = stockUserInventoryRepo.Where(s => s.AppUserId == appUserId).ToList().Select(s => new StockUserInventoryViewModel
                {
                    AppUserId = appUserId,
                    FullName = s.AppUser.FullName,
                    Products = s.AppUser.StockUserInventories.Select(c => new ProductInput
                    {
                        Barkod = c.Stock.Barcode,
                        Name = c.Stock.Name,
                        StockId = c.StockId,
                        Count = c.Count
                    }).ToList()
                }).FirstOrDefault();
                return PartialView(model);
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
        public ActionResult ExitProductFromPerson(ExitProductInput input)
        {
            try
            {

                CheckIsAuthorized(EnumHelper.AuthorizeMethod.Update);

                if (!ModelState.IsValid)
                    return PartialView(input);

                var entity = stockUserInventoryRepo.Where(s => s.AppUserId == input.AppUserId).ToList();

                foreach (var product in input.Products)
                {
                    var inventoryEntity = entity.FirstOrDefault(s => s.StockId == product.StockId);
                    var stocks = input.Products.Select(c => c.StockId).ToList();
                    var stockInfo = repo.Where(s => stocks.Contains(s.Id)).Select(x => new { x.Currency.Code, x.PriceSell, x.Id }).ToList();

                    if (input.IsExit == 0)
                    {
                        if (inventoryEntity.Count == product.Count)
                            entity.Remove(inventoryEntity);
                        else
                        {
                            inventoryEntity.Count -= product.Count;
                            stockMovementRepo.Insert(new StockMovement
                            {
                                Action = ConstHelper.StockMovementAction.IADE,
                                AppUserId = input.AppUserId,
                                CreateAppUserId = WebUserManager.GetUserInfo().Id,
                                Count = product.Count,
                                Date = DateTime.Now,
                                LocationId = ApplicationSettingHelper.BaseLocationId,
                                StockId = product.StockId,
                                Price = 0,
                            });
                            var baseStock = repo.Get(product.StockId);
                            baseStock.Count += product.Count;
                        }
                    }
                    else
                    {
                        if (inventoryEntity.Count == product.Count)
                            entity.Remove(inventoryEntity);
                        else
                        {
                            var firstInfo = stockInfo.FirstOrDefault(c => c.Id == product.StockId);
                            inventoryEntity.Count -= product.Count;
                            var movement = new StockMovement
                            {
                                Action = ConstHelper.StockMovementAction.IADE,
                                AppUserId = input.AppUserId,
                                CreateAppUserId = WebUserManager.GetUserInfo().Id,
                                Count = product.Count,
                                Date = DateTime.Now,
                                LocationId = input.LocationId,
                                StockId = product.StockId,
                                Price = CalculatePrice(product.Count, firstInfo.PriceSell, firstInfo.Code)
                            };
                            stockMovementRepo.Insert(movement);
                        }
                    }

                }
                repo.Save();

                return Json(new { Content = "İşlem başarılı !" });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return PartialView("AddProduct", input);
            }
        }
        
        double CalculatePrice(int count, double price,string currencyCode)
        {
            return count * (ConstHelper.GetCurrency(currencyCode) * price);
        }
    }
}