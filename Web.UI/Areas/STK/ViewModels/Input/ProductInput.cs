using Omu.AwesomeMvc;
using System.ComponentModel.DataAnnotations;

namespace Web.UI.Areas.STK
{
    public class ProductInput
    {
        //[Required(ErrorMessage = "{0} gerekli")]
        //[Display(Name = "Barkod")]
        //[UIHint("Odropdown")]
        //[AweUrl(Controller = "Data", Action = "StockAutoComplete")]
        public int StockId { get; set; }
        public string Barkod { get; set; }

        [Required(ErrorMessage = "{0} gerekli")]
        [Display(Name = "Adet")]
        public int Count { get; set; }
        public string Name { get; set; } //TO DO: exitproduct.cshtml optimize edilecek
        public int IsDeleted { get; set; }
    }
}