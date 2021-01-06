using System.ComponentModel.DataAnnotations;
using Web.UI.ViewModels;

namespace Web.UI.Areas.SYS
{
    public class CityInput : BaseInput
    {
        [Required(ErrorMessage = "{0} gerekli")]
        [Display(Name = "Ad")]
        [MaxLength(50, ErrorMessage = "Max {1}")]
        [UIHint("String")]
        public string Name { get; set; }

        [Required(ErrorMessage = "{0} gerekli")]
        [Display(Name = "Plaka")]
        [UIHint("Int32")]
        public int Plate { get; set; }

        [Required(ErrorMessage = "{0} gerekli")]
        [Display(Name = "Sıralama")]
        [UIHint("Int32")]
        public int OrderId { get; set; }
    }
}