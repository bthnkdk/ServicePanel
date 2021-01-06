using System.ComponentModel.DataAnnotations;

namespace Web.UI.ViewModels
{
    public class TitleInput : BaseInput
    {
        [Required(ErrorMessage = "{0} gerekli")]
        [Display(Name = "Ad")]
        [MaxLength(50, ErrorMessage = "Max {1}")]
        public string Name { get; set; }
    }
}