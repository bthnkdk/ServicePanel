using Omu.AwesomeMvc;
using System.ComponentModel.DataAnnotations;
using Web.UI.ViewModels;

namespace Web.UI.Areas.DEF
{
    public class VehicleInput : BaseInput
    {
        [Required(ErrorMessage = "{0} gerekli")]
        [Display(Name = "Ad")]
        [MaxLength(50, ErrorMessage = "Max {1}")]
        [UIHint("String")]
        public string Name { get; set; }
        [Required(ErrorMessage = "{0} gerekli")]
        [Display(Name = "Plaka")]
        [MaxLength(11, ErrorMessage = "Max {1}")]
        [UIHint("String")]
        public string Plate { get; set; }
        [Required(ErrorMessage = "{0} gerekli")]
        [Display(Name = "Zimmetli Personel")]
        [UIHint("Odropdown")]
        [AweUrl(Controller = "Data", Action = "GetAppUsers")]
        public int AppUserId { get; set; }
    }
}