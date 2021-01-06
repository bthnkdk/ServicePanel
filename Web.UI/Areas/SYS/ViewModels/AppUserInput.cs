using Omu.AwesomeMvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Web.UI.ViewModels;

namespace Web.UI.Areas.SYS
{
    public class AppUserInput : BaseInput
    {
        public Guid RowId { get; set; }

        [Required(ErrorMessage = "{0} gerekli")]
        [Display(Name = "Ad")]
        [MaxLength(50, ErrorMessage = "Max {1}")]
        public string Firstname { get; set; }

        [Required(ErrorMessage = "{0} gerekli")]
        [Display(Name = "Soyad")]
        [MaxLength(50, ErrorMessage = "Max {1}")]
        public string Lastname { get; set; }

        [Display(Name = "Ünvan")]
        [MaxLength(150, ErrorMessage = "Max {1}")]
        public string SubTitle { get; set; }

        [Required(ErrorMessage = "{0} gerekli")]
        [Display(Name = "E-Mail")]
        [MaxLength(150, ErrorMessage = "Max {1}")]
        [UIHint("String")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi girin")]
        public string Username { get; set; }

        [Display(Name = "Kilitli ?")]
        public bool IsLock { get; set; }

        [Display(Name = "Yönetici ?")]
        public bool IsAdmin { get; set; }

        [Display(Name = "Telefon")]
        [MaxLength(20, ErrorMessage = "Max {1}")]
        public string Mobile { get; set; }

        [Display(Name = "Açıklama")]
        [UIHint("Textarea")]
        [MaxLength(500, ErrorMessage = "Max {1}")]
        public string Description { get; set; }

        [Required(ErrorMessage = "{0} gerekli")]
        [Display(Name = "Görev")]
        [UIHint("Odropdown")]
        [AweUrl(Controller = "Data", Action = "GetTitles")]
        public int TitleId { get; set; }

        [Required(ErrorMessage = "{0} gerekli")]
        [Display(Name = "Departman")]
        [UIHint("Odropdown")]
        [AweUrl(Controller = "Data", Action = "GetDepartments")]
        public int DepartmentId { get; set; }

        [Display(Name = "Kullanıcı Yetki Taslağı")]
        [UIHint("Odropdown")]
        [AweUrl(Controller = "Data", Action = "GetAppUsers")]
        public int? CopyAppUserId { get; set; }

        [Display(Name = "İşe Baş. Tar.")]
        public DateTime? StartDate { get; set; }

        [Display(Name = "İşten Çıkç Tar.")]
        public DateTime? EndDate { get; set; }
    }
}