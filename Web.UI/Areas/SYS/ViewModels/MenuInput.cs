using Omu.AwesomeMvc;
using System.ComponentModel.DataAnnotations;
using Web.UI.ViewModels;

namespace Web.UI.Areas.SYS
{
    public class MenuInput : BaseInput
    {
        [Required(ErrorMessage = "{0} gerekli")]
        [Display(Name = "Ad")]
        [MaxLength(50, ErrorMessage = "Max {1}")]
        public string Name { get; set; }

        [Required(ErrorMessage = "{0} gerekli")]
        [Display(Name = "Kod")]
        [UIHint("Odropdown")]
        [AweUrl(Controller = "Data", Action = "GetAuthCodes")]
        public int AuthCodeId { get; set; }
        [Required(ErrorMessage = "{0} gerekli")]
        [Display(Name = "Url")]
        public string Url { get; set; }

        [Display(Name = "Simge")]
        [MaxLength(150, ErrorMessage = "Max {1}")]
        public string Icon { get; set; }
        [Required(ErrorMessage = "{0} gerekli")]
        [Display(Name = "Sıralama")]
        [UIHint("Int32")]
        public int OrderNumber { get; set; }

        [Required(ErrorMessage = "{0} gerekli")]
        [Display(Name = "Üst Menü")]
        [UIHint("Odropdown")]
        [AweUrl(Controller = "Data", Action = "GetParentMenuList")]
        public int? ParentId { get; set; }
    }
}