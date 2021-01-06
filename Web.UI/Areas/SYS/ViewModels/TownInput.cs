using Omu.AwesomeMvc;
using System.ComponentModel.DataAnnotations;
using Web.UI.ViewModels;

namespace Web.UI.Areas.SYS
{
    public class TownInput : BaseInput
    {
        [Required(ErrorMessage = "{0} gerekli")]
        [Display(Name = "Ad")]
        [MaxLength(75, ErrorMessage = "Max {1}")]
        [UIHint("String")]
        public string Name { get; set; }
        [Required(ErrorMessage = "{0} gerekli")]
        [Display(Name = "Şehir")]
        [UIHint("Odropdown")]
        [AweUrl(Controller = "Data", Action = "GetCities")]
        public int CityId { get; set; }
        [Required(ErrorMessage = "{0} gerekli")]
        [Display(Name = "Sıralama")]
        [UIHint("Int32")]
        public int OrderId { get; set; }
        [Display(Name = "Bölge")]
        [UIHint("Odropdown")]
        [AweUrl(Controller = "Data", Action = "GetAreas")]
        public int? AreaId { get; set; }
    }
}