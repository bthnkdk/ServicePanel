using Omu.AwesomeMvc;
using System.ComponentModel.DataAnnotations;
using Web.UI.ViewModels;

namespace Web.UI.Areas.DEF
{
    public class PrinterModelInput : BaseInput
    {

        [Required(ErrorMessage = "{0} gerekli")]
        [Display(Name = "Ad")]
        [MaxLength(50, ErrorMessage = "Max {1}")]
        [UIHint("String")]
        public string Name { get; set; }

        [Display(Name = "Renkli ?")]
        public bool IsColor { get; set; }

        [Required(ErrorMessage = "{0} gerekli")]
        [Display(Name = "Marka")]
        [UIHint("Odropdown")]
        [AweUrl(Controller = "Data", Action = "GetPrinterBrands")]
        public int PrinterBrandId { get; set; }

    }
}