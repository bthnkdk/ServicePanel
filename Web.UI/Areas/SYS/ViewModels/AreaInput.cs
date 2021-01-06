using System.ComponentModel.DataAnnotations;
using Web.UI.ViewModels;

namespace Web.UI.Areas.SYS
{
    public class AreaInput : BaseInput
    {
        [Required(ErrorMessage = "{0} gerekli")]
        [Display(Name = "Ad")]
        [MaxLength(75, ErrorMessage = "Max {1}")]
        [UIHint("String")]
        public string Name { get; set; }
    }
}