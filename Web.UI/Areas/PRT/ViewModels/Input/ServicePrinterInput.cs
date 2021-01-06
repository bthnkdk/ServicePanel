using Omu.Awem;
using Omu.AwesomeMvc;
using System.ComponentModel.DataAnnotations;
using Web.UI.ViewModels;

namespace Web.UI.Areas.PRT
{
    public class ServicePrinterInput : BaseInput
    {
        public int LocationId { get; set; }
        [Display(Name = "Siyah")]
        public int Mono { get; set; }
        [Display(Name = "Renkli")]
        public int Color { get; set; }
        [Required(ErrorMessage = "{0} gerekli")]
        [Display(Name = "Durum")]
        [UIHint("odropdown")]
        [AweUrl(Controller = "Data", Action = "GetServicePrinterStatus")]
        public int Status { get; set; }
        [Required(ErrorMessage = "{0} gerekli")]
        [Display(Name = "Problem")]
        [UIHint("Textarea")]
        public string Description { get; set; }
        [Display(Name = "İşlem")]
        [UIHint("Textarea")]
        public string Process { get; set; }
        [Required(ErrorMessage = "{0} gerekli")]
        [Display(Name = "Bakım")]
        public bool IsMaintenanceOk { get; set; }
        public int? CounterId { get; set; }
        [Required(ErrorMessage = "{0} gerekli")]
        public int ServiceId { get; set; }
        [Required(ErrorMessage = "{0} gerekli")]
        public int PrinterId { get; set; }
        [Required(ErrorMessage = "{0} gerekli")]
        [Display(Name = "İşlem Tipi")]
        [UIHint("odropdown")]
        [AweUrl(Controller = "Data", Action = "GetPrinterServiceType")]
        public int PrinterServiceTypeId { get; set; }
    }
}