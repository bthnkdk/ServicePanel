using System.Collections.Generic;

namespace Web.UI.Areas.STK
{
    public class ExitProductInput
    {
        //[Required(ErrorMessage = "{0} gerekli")]
        //[Display(Name = "Barkod")]
        //[UIHint("Odropdown")]
        //[AweUrl(Controller = "Data", Action = "StockAutoComplete")]
        public int LocationId { get; set; }
        public int AppUserId { get; set; }
        public int IsExit { get; set; }
        public List<ProductInput> Products { get; set; } = new List<ProductInput>();
    }
}