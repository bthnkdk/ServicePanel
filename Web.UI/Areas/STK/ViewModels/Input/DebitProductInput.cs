using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Web.UI.ViewModels;

namespace Web.UI.Areas.STK
{
    public class DebitProductInput : BaseInput
    {
        [Required(ErrorMessage = "{0} gerekli")]
        [Display(Name = "Personel Adı")]
        public int AppUserId { get; set; }

        public List<DebitNewProductModel> StockList { get; set; } = new List<DebitNewProductModel>();
    }

    public class DebitNewProductModel : BaseInput
    {
        public int Count { get; set; }
        public string Name { get; set; }
        public int IsDeleted { get; set; }
    }
}