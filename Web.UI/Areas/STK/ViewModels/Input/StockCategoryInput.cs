using Omu.AwesomeMvc;
using System.ComponentModel.DataAnnotations;
using Web.UI.ViewModels;

namespace Web.UI.Areas.STK
{
    public class StockCategoryInput : BaseInput
    {
        [Required(ErrorMessage = "{0} gerekli")]
        [Display(Name = "Ad")]
        [MaxLength(50, ErrorMessage = "Max {1}")]
        [UIHint("String")]
        public string Name { get; set; }

        [Required(ErrorMessage = "{0} gerekli")]
        [Display(Name = "Ana Stok Grubu")]
        [UIHint("Odropdown")]
        [AweUrl(Controller = "Data", Action = "GetMainStockCategory")]
        public int StockMainCategoryId { get; set; }
    }
}