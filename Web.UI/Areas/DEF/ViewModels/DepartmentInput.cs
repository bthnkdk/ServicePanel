using System.ComponentModel.DataAnnotations;
using Web.UI.ViewModels;

namespace Web.UI.Areas.DEF
{
    public class DepartmentInput : BaseInput
    {
        [Required(ErrorMessage = "{0} gerekli")]
        [Display(Name = "Ad")]
        [MaxLength(50, ErrorMessage = "Max {1}")]
        [UIHint("String")]
        public string Name { get; set; }
        [Required(ErrorMessage = "{0} gerekli")]
        [Display(Name = "Kod")]
        [MaxLength(50, ErrorMessage = "Max {1}")]
        [UIHint("String")]
        public string Code { get; set; }
        [Display(Name = "OLmuyor oç")]
        //[UIHint("BooleanSwitch")] //test time
        [Switchery(SwitcheryFalseText = "Hayır", SwitcheryTrueText = "Evet")]
        public bool Test { get; set; }
    }
}