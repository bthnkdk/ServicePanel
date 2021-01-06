using Domain;
using Omu.AwesomeMvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Web.UI.ViewModels;

namespace Web.UI.Areas.SVC
{
    public class ServiceInput : BaseInput
    {
        public int CreatedUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? UpdateUserId { get; set; }
        public Guid RowId { get; set; }
        public string CustomerName { get; set; }
        public int CustomerId { get; set; }
        [Display(Name = "Lokasyon")]
        [Required(ErrorMessage = "{0} gerekli")]
        public int LocationId { get; set; }
        [Required(ErrorMessage = "{0} gerekli")]
        [Display(Name = "Durum")]
        [UIHint("Odropdown")]
        [AweUrl(Controller = "Data", Action = "GetServiceStatus")]
        public int Status { get; set; }
        [Required(ErrorMessage = "{0} gerekli")]
        [Display(Name = "Kategori")]
        [UIHint("Odropdown")]
        [AweUrl(Controller = "Data", Action = "GetServiceCategories")]
        public int ServiceCategoryId { get; set; }
        [Required(ErrorMessage = "{0} gerekli")]
        [Display(Name = "Öncelik")]
        [UIHint("Odropdown")]
        [AweUrl(Controller = "Data", Action = "GetServicePriority")]
        public int Priority { get; set; }
        [Required(ErrorMessage = "{0} gerekli")]
        [Display(Name = "Yetkili Adı")]
        public string ResponsibleName { get; set; }
        [Display(Name = "Yetkili Telefon")]
        public string ResponsiblePhone { get; set; }
        [Required(ErrorMessage = "{0} gerekli")]
        [Display(Name = "Servis Tarihi")]
        public DateTime ServiceDate { get; set; }
        [Display(Name = "Açıklama")]
        [UIHint("Textarea")]
        public string Description { get; set; }
        [Display(Name = "İşlem")]
        [UIHint("Textarea")]
        public string Process { get; set; }
        [Display(Name = "Ücret")]
        [UIHint("Price")]
        [AweUrl(Controller = "Data", Action = "GetCurrency")]
        public decimal Price { get; set; }
        public int? PriceCurrency { get; set; }

        public List<ServicePrinterInput> ServicePrinters { get; set; } = new List<ServicePrinterInput>();
        public List<ServiceStockInput> ServiceStocks { get; set; } = new List<ServiceStockInput>();
        public int[] ServicePersons { get; set; }
        public int[] ServiceVehicles { get; set; }
        public virtual string LocationName { get; set; }

    }
}