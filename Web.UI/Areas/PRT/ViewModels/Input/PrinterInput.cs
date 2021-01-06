using Omu.AwesomeMvc;
using System;
using System.ComponentModel.DataAnnotations;
using Web.UI.ViewModels;

namespace Web.UI.Areas.PRT
{
    public class PrinterInput : BaseInput
    {
        public Guid RowId { get; set; }
        public string CustomerName { get; set; }
        public int CustomerId { get; set; }
        [Display(Name = "IP Adres")]
        public string IPAdress { get; set; }
        public DateTime? PSRUpdateDate { get; set; }
        [Required(ErrorMessage = "{0} gerekli")]
        [Display(Name = "Lokasyon")]
        [UIHint("Odropdown")]
        [AweUrl(Controller = "Data", Action = "GetLocationByCustomer")]
        [AweParent(ParentId = "CustomerId")]
        public int LocationId { get; set; }
        [Display(Name = "Marka")]
        [UIHint("Odropdown")]
        [AweUrl(Controller = "Data", Action = "GetPrinterBrands")]
        public int BrandId { get; set; }
        [Required(ErrorMessage = "{0} gerekli")]
        [Display(Name = "Model")]
        [UIHint("Odropdown")]
        [AweUrl(Controller = "Data", Action = "GetPrinterModels")]
        [AweParent(ParentId = "BrandId")]
        public int PrinterModelId { get; set; }

        [Required(ErrorMessage = "{0} gerekli")]
        [MaxLength(75)]
        [Display(Name = "Seri No")]
        public string SerialNumber { get; set; }
        [Required(ErrorMessage = "{0} gerekli")]
        [MaxLength(150)]
        [Display(Name = "Yazıcı Adı")]
        public string Name { get; set; }
        [Required(ErrorMessage = "{0} gerekli")]
        [Display(Name = "Yazıcı No")]
        public int PrinterNumber { get; set; }
        [Display(Name = "Son Bakım Tarihi")]
        public DateTime? LastMaintenanceDate { get; set; }
        public int? LastMaintenanceUser { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedUserId { get; set; }
        [MaxLength(500)]
        [UIHint("Textarea")]
        [Display(Name = "Açıklama")]
        public string Description { get; set; }
    }
}