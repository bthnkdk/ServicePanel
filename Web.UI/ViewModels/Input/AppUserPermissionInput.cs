using Omu.AwesomeMvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace Web.UI.ViewModels
{
    public class AppUserPermissionInput : BaseInput
    {
        [Required(ErrorMessage = "{0} gerekli")]
        [Display(Name = "Başlangıç Tarihi")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "{0} gerekli")]
        [Display(Name = "Bitiş Tarihi")]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "{0} gerekli")]
        [Display(Name = "Açıklama")]
        [MaxLength(500, ErrorMessage = "Max {1}")]
        [UIHint("Textarea")]
        public string Description { get; set; }

        [Required(ErrorMessage = "{0} gerekli")]
        [Display(Name = "Çalışan")]
        [UIHint("Odropdown")]
        [AweUrl(Controller = "Data", Action = "GetAppUsers")]
        public int AppUserId { get; set; }

        [Display(Name = "Oluşturan")]
        [UIHint("Readonly")]
        public string CreatedUser { get; set; }
        [Display(Name = "Tarih")]
        [UIHint("Readonly")]
        public DateTime CreatedDate { get; set; }

        [Required(ErrorMessage = "{0} gerekli")]
        [Display(Name = "İzin Süresi")]
        public int Duration { get; set; }
    }
}