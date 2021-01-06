using System.ComponentModel.DataAnnotations;
using Web.UI.ViewModels;

namespace Web.UI.Areas.SYS
{
    public class AuthCodeInput : BaseInput
    {
        [Required(ErrorMessage = "{0} gerekli")]
        [Display(Name = "Ad")]
        [UIHint("String")]
        public string Name { get; set; }
        [Required(ErrorMessage = "{0} gerekli")]
        [Display(Name = "Kısa Kod")]
        [UIHint("String")]
        [MaxLength(12)]
        public string Code { get; set; }
    }
}