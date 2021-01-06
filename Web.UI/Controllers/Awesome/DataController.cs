using Core;
using Domain;
using Omu.AwesomeMvc;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Web.UI.Helper;

namespace Web.UI.Controllers
{
    public class Select2Model
    {
        public Select2Model(object id = null, string text = null)
        {
            if (id != null)
                this.id = id;
            if (text != null)
                this.text = text;
        }
        public object id { get; set; }
        public string text { get; set; }
    }
    [JsonAllowGet]
    public class DataController : Controller
    {
        readonly IUniRepo repo;

        public DataController(IUniRepo repo)
        {
            this.repo = repo;
        }

        public ActionResult CustomerAutoComplete(string v)
        {
            v = (v ?? string.Empty).ToLower();
            var items = repo.All<Customer>().ToList()
                .Where(o => o.Name.ToLower().Contains(v))
                .Select(o => new KeyContent(o.Id, o.Name));
            return Json(items);
        }
        public ActionResult StockAutoComplete(string v)
        {
            v = (v ?? string.Empty).ToLower();
            var items = repo.All<Stock>().ToList()
                .Where(o => o.Barcode.ToLower().Contains(v) || o.Name.ToLower().Contains(v))
                .Select(o => new KeyContent(o.Id, o.Name + " - " + o.Barcode));
            return Json(items);
        }
        public ActionResult GetLocationByCustomer(int? parent)
        {
            var items = new List<KeyContent> { new KeyContent(string.Empty, "Lokasyon") };
            if (parent.HasValue)
                items.AddRange(repo.GetAll<Location>().Where(s => s.CustomerId == parent).OrderBy(p => p.Name).ToArray().Select(o => new KeyContent(o.Id, o.Name)));
            return Json(items);
        }
        public ActionResult GetAllLocationsWithCustomerNameForSelect2()
        {
            var items = new List<Select2Model> { new Select2Model(string.Empty, "Ürün") };
            items.AddRange(repo.GetAll<Location>(true).Where(p => !p.IsDeleted).OrderBy(p => p.Name).ToArray()
                .Select(o => new Select2Model(o.Id, o.Customer.Name + " - " + o.Name)));
            return Json(items);
        }


        public ActionResult GetAuthCodes()
        {
            var items = new List<KeyContent>();
            items.AddRange(repo.All<AuthCode>().ToList().Select(o => new KeyContent(o.Id, o.Name)));
            return Json(items);
        }
        public ActionResult GetCustomerTypes()
        {
            var items = new List<KeyContent>() { new KeyContent("", "Seçin") };
            items.AddRange(repo.All<CustomerType>().ToList().Select(o => new KeyContent(o.Id, o.Name)));
            return Json(items);
        }
        public ActionResult GetCustomer()
        {
            var items = new List<KeyContent>() { new KeyContent("", "Seçin") };
            items.AddRange(repo.All<Customer>().ToList().Select(o => new KeyContent(o.Id, o.Name)));
            return Json(items);
        }
        public ActionResult GetResponsibleByLocation(int? parent)
        {
            if (parent.HasValue)
            {
                var result = repo.GetAll<Location>().Where(s => s.Id == parent).Select(s => new { s.ResponsibleName, s.ResponsiblePhone }).FirstOrDefault();
                return Json(result);
            }
            return null;
        }

        public ActionResult GetPrinterByLocation(int? parent)
        {
            var items = new List<KeyContent>() { new KeyContent("", "Yazıcı") };
            if (parent.HasValue)
                items.AddRange(repo.GetAll<Printer>().Where(s => s.LocationId == parent.Value).ToArray().Select(s => new KeyContent(s.Id, s.Name)));
            return Json(items);
        }
        public ActionResult GetPrinterByLocationForSelect2(int? parent)
        {
            var items = new List<Select2Model>() { new Select2Model("", "Yazıcı") };
            if (parent.HasValue)
                items.AddRange(repo.GetAll<Printer>().Where(s => s.LocationId == parent.Value).ToArray().Select(s => new Select2Model(s.Id, s.Name)));
            return Json(items);
        }

        public ActionResult GetPreApplicationTypes()
        {
            var items = new List<KeyContent>() { new KeyContent("", "Seçin") };
            items.AddRange(repo.All<PreApplicationType>().ToList().Select(o => new KeyContent(o.Id, o.Name)));
            return Json(items);
        }
        public ActionResult GetPreApplicationStatus()
        {
            var items = new List<KeyContent>() { new KeyContent("", "Seçin") };
            items.AddRange(repo.All<PreApplicationStatus>().ToList().Select(o => new KeyContent(o.Id, o.Name)));
            return Json(items);
        }


        public ActionResult GetParentMenuList()
        {
            var items = new List<KeyContent>();
            items.AddRange(repo.All<Menu>(true).Where(s => (s.Parent.Parent.Parent.Parent ?? null) == null).ToArray()
               .Select(o => new KeyContent(o.Id, o.Name)));
            return Json(items);
        }
        public ActionResult GetMenuList(string v)
        {
            v = (v ?? "").ToLower().Trim();
            var items = new List<KeyContent>();
            items.AddRange(repo.All<Menu>(true).Where(s => s.Name.Contains(v)).OrderBy(p => p.Name).ToArray()
                .Select(o => new KeyContent(o.Url, o.Name)));
            return Json(items);
        }
        public ActionResult GetTitles()
        {
            var items = new List<KeyContent> { new KeyContent(string.Empty, "Görev") };
            items.AddRange(repo.GetAll<Title>().Where(p => !p.IsDeleted).OrderBy(p => p.Name).ToArray()
                .Select(o => new KeyContent(o.Id, o.Name)));
            return Json(items);
        }
        public ActionResult GetVisitTypes()
        {
            var items = new List<KeyContent> { new KeyContent(string.Empty, "Seçin") };
            items.AddRange(repo.GetAll<VisitType>().Where(p => !p.IsDeleted).OrderBy(p => p.Name).ToArray()
                .Select(o => new KeyContent(o.Id, o.Name)));
            return Json(items);
        }
        public ActionResult GetVisitCategories()
        {
            var items = new List<KeyContent> { new KeyContent(string.Empty, "Seçin") };
            items.AddRange(repo.GetAll<VisitCategory>().Where(p => !p.IsDeleted).OrderBy(p => p.Name).ToArray()
                .Select(o => new KeyContent(o.Id, o.Name)));
            return Json(items);
        }
        public ActionResult GetDepartments()
        {
            var items = new List<KeyContent> { new KeyContent(string.Empty, "Departman") };
            items.AddRange(repo.GetAll<Department>().Where(p => !p.IsDeleted).OrderBy(p => p.Name).ToArray()
                .Select(o => new KeyContent(o.Id, o.Name)));
            return Json(items);
        }
        public ActionResult GetAppUsers()
        {
            var items = new List<KeyContent> { new KeyContent(string.Empty, "Kullanıcılar") };
            items.AddRange(repo.All<AppUser>(true).Where(p => !p.IsDeleted).OrderBy(p => p.Firstname).ToArray()
                .Select(o => new KeyContent(o.Id, o.Firstname + " " + o.Lastname)));
            return Json(items);
        }
        public ActionResult GetAppUserForSelect2()
        {
            var items = new List<Select2Model> { new Select2Model(string.Empty, "Personel") };
            items.AddRange(repo.GetAll<AppUser>().Where(p => !p.IsDeleted).OrderBy(p => p.Firstname).ToArray()
                .Select(o => new Select2Model(o.Id, o.FullName)));
            return Json(items);
        }
        public ActionResult GetAppUsersAutoComplete(string v)
        {
            v = (v ?? string.Empty).ToLower();
            var items = repo.All<AppUser>().ToList()
                .Where(o => o.Firstname.ToLower().Contains(v) || o.Lastname.ToLower().Contains(v))
                .Select(o => new KeyContent(o.Id, o.FullName));
            return Json(items);
        }
        public ActionResult GetAppUsersByDepartment(int? id)
        {
            var items = new List<KeyContent> { new KeyContent(string.Empty, "Kullanıcılar") };
            if (id.HasValue)
                items.AddRange(repo.All<AppUser>(true).Where(p => !p.IsDeleted && p.DepartmentId == id).OrderBy(p => p.Firstname).ToArray()
                    .Select(o => new KeyContent(o.Id, o.Firstname + " " + o.Lastname)));
            return Json(items);
        }
        public ActionResult GetTechnicAppUserMulti()
        {
            var items = new List<KeyContent>();
            items.AddRange(repo.All<AppUser>(true).Where(p => !p.IsDeleted && (p.TitleId == ConstHelper.Title.TEKNIK_PERSONEL || p.TitleId == ConstHelper.Title.TEKNIK_YONETICI)).OrderBy(p => p.Firstname).ToArray()
                .Select(o => new KeyContent(o.Id, o.Firstname + " " + o.Lastname)));

            return Json(items);
        }

        public ActionResult GetVehicleMulti()
        {
            var items = new List<KeyContent>();
            items.AddRange(repo.All<Vehicle>(true).Where(p => !p.IsDeleted).Select(s => new { s.Id, Plate = s.Plate + " " + s.Name }).OrderBy(p => p.Plate).ToArray()
                .Select(o => new KeyContent(o.Id, o.Plate)));

            return Json(items);
        }

        public ActionResult GetPrinterBrands()
        {
            var items = new List<KeyContent> { new KeyContent(string.Empty, "Marka") };
            items.AddRange(repo.All<PrinterBrand>(true).OrderBy(p => p.Name).ToArray()
                .Select(o => new KeyContent(o.Id, o.Name)));
            return Json(items);
        }
        public ActionResult GetMainStockCategory()
        {
            var items = new List<KeyContent> { new KeyContent(string.Empty, "Seçin") };
            items.AddRange(repo.All<StockMainCategory>(true).OrderBy(p => p.Name).ToArray()
                .Select(o => new KeyContent(o.Id, o.Name)));
            return Json(items);
        }

        public ActionResult GetStockCategories()
        {
            var items = new List<KeyContent> { new KeyContent(string.Empty, "Seçin") };
            items.AddRange(repo.All<StockCategory>(true).OrderBy(p => p.Name).ToArray()
                .Select(o => new KeyContent(o.Id, o.Name)));
            return Json(items);
        }
        public ActionResult GetProductForSelect2()
        {
            var items = new List<Select2Model> { new Select2Model(string.Empty, "Ürün") };
            items.AddRange(repo.GetAll<Stock>(true).Where(p => !p.IsDeleted).OrderBy(p => p.Name).ToArray()
                .Select(o => new Select2Model(o.Id, o.Name)));
            return Json(items);
        }


        public ActionResult GetServiceCategories()
        {
            var items = new List<KeyContent> { new KeyContent(string.Empty, "Servis Kategorileri") };
            items.AddRange(repo.All<ServiceCategory>(true).Where(p => !p.IsDeleted).OrderBy(p => p.Name).ToArray()
                .Select(o => new KeyContent(o.Id, o.Name)));
            return Json(items);
        }
        public ActionResult GetPrinterModels(int? parent)
        {
            var items = new List<KeyContent>() { new KeyContent("", "Model") };
            if (parent.HasValue)
                items.AddRange(repo.All<PrinterModel>().Where(s => s.PrinterBrandId == parent.Value).ToList().Select(o => new KeyContent(o.Id, o.Name)));
            return Json(items);
        }
        public ActionResult GetPrinterServiceType()
        {
            var items = new List<KeyContent> { new KeyContent(string.Empty, "İşlem Türü") };
            items.AddRange(repo.GetAll<PrinterServiceType>().Where(p => !p.IsDeleted).OrderBy(p => p.Name).ToArray()
                .Select(o => new KeyContent(o.Id, o.Name)));
            return Json(items);
        }
        public ActionResult GetPrinterServiceTypeForSelect2()
        {
            var items = new List<Select2Model> { new Select2Model(string.Empty, "İşlem Türü") };
            items.AddRange(repo.GetAll<PrinterServiceType>().Where(p => !p.IsDeleted).OrderBy(p => p.Name).ToArray()
                .Select(o => new Select2Model(o.Id, o.Name)));
            return Json(items);
        }


        #region GEOGRAPHY
        public ActionResult GetCities()
        {
            var items = new List<KeyContent> { new KeyContent(string.Empty, "Şehir") };
            items.AddRange(repo.GetAll<City>().OrderBy(p => p.Name).ToArray()
                .Select(o => new KeyContent(o.Id, o.Name)));
            return Json(items);
        }
        public ActionResult GetAreas()
        {
            var items = new List<KeyContent> { new KeyContent(string.Empty, "Bölge") };
            items.AddRange(repo.GetAll<Area>().OrderBy(p => p.Name).ToArray()
                .Select(o => new KeyContent(o.Id, o.Name)));
            return Json(items);
        }
        public ActionResult GetTowns(int? parent)
        {
            var items = new List<KeyContent> { new KeyContent(string.Empty, "Semt") };
            if (parent.HasValue)
                items.AddRange(repo.GetAll<Town>().Where(s => s.CityId == parent).OrderBy(p => p.Name).ToArray()
                    .Select(o => new KeyContent(o.Id, o.Name)));
            return Json(items);
        }
        public ActionResult GetTownsAll()
        {
            var items = new List<KeyContent> { new KeyContent(string.Empty, "İlçe") };
            items.AddRange(repo.GetAll<Town>().OrderBy(p => p.Name).ToArray()
                .Select(o => new KeyContent(o.Id, o.Name)));
            return Json(items);
        }
        #endregion

        #region CONSTS

        [OutputCache(Duration = 60)]
        public ActionResult GetCurrency()
        {
            var items = new List<KeyContent> { new KeyContent(string.Empty, "Kur") };
            foreach (var item in repo.All<Currency>(true).ToList())
            {
                items.Add(new KeyContent
                {
                    Key = item.Id,
                    Content = item.Name,
                });
            }
            return Json(items);
        }

        [OutputCache(Duration = 60)]
        public ActionResult GetServiceStatus()
        {
            var items = new List<KeyContent> { new KeyContent(string.Empty, "Durum") };
            foreach (var item in ConstHelper.ServiceStatusList())
            {
                items.Add(new KeyContent
                {
                    Key = item.Key,
                    Content = item.Value,
                });
            }
            return Json(items);
        }
        [OutputCache(Duration = 60)]
        public ActionResult GetServicePrinterStatus()
        {
            var items = new List<KeyContent> { new KeyContent(string.Empty, "Durum") };
            foreach (var item in ConstHelper.ServicePrinterStatusList())
                items.Add(new KeyContent(item.Key, item.Value));

            return Json(items);
        }
        [OutputCache(Duration = 60)]
        public ActionResult GetServicePrinterStatusForSelect2()
        {
            var items = new List<Select2Model> { new Select2Model(string.Empty, "Durum") };
            foreach (var item in ConstHelper.ServicePrinterStatusList())
                items.Add(new Select2Model(item.Key, item.Value));

            return Json(items);
        }
        [OutputCache(Duration = 60)]
        public ActionResult GetServicePriority()
        {
            var items = new List<KeyContent> { new KeyContent(string.Empty, "Öncelik") };
            foreach (var item in ConstHelper.ServicePriorityList())
            {
                items.Add(new KeyContent()
                {
                    Key = item.Key,
                    Content = item.Value,
                });
            }
            return Json(items);
        }

        [OutputCache(Duration = 60)]
        public ActionResult GetOfferStatus()
        {
            var items = new List<KeyContent> { new KeyContent(string.Empty, "Durum") };
            foreach (var item in ConstHelper.OfferStatusList())
            {
                items.Add(new KeyContent
                {
                    Key = item.Key,
                    Content = item.Value,
                });
            }
            return Json(items);
        }
        [OutputCache(Duration = 60)]
        public ActionResult GetVisitStatus()
        {
            var items = new List<KeyContent> { new KeyContent(string.Empty, "Durum") };
            foreach (var item in ConstHelper.VisitStatusList())
            {
                items.Add(new KeyContent
                {
                    Key = item.Key,
                    Content = item.Value,
                });
            }
            return Json(items);
        }
        #endregion
    }
}