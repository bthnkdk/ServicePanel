using Omu.AwesomeMvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace Web.UI.ViewModels
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

        //[Required(ErrorMessage = "{0} gerekli")]
        //[Display(Name = "Ofis")]
        //[UIHint("Odropdown")]
        //[AweUrl(Controller = "Data", Action = "GetRegions")]
        //public int RegionId { get; set; }

        //[Display(Name = "Belge Tarihi")]
        //public DateTime? DocDate { get; set; }

        //[Display(Name = "Belge No")]
        //[MaxLength(20, ErrorMessage = "Max {1}")]
        //public string DocNumber { get; set; }

        //[Display(Name = "Belgeyi Veren Kurum")]
        //[MaxLength(150, ErrorMessage = "Max {1}")]
        //public string DocFirm { get; set; }

        //[Display(Name = "Belge Geç. Süresi")]
        //[MaxLength(50, ErrorMessage = "Max {1}")]
        //public string DocExpire { get; set; }

        //[Display(Name = "Asansör Tipleri")]
        //[MaxLength(100, ErrorMessage = "Max {1}")]
        //public string LiftTypes { get; set; }

        //[Display(Name = "Vekalet Eden")]
        //[UIHint("Odropdown")]
        //[AweUrl(Controller = "Data", Action = "GetAppUserMP")]
        //public int? ProxyUserId { get; set; }

        [Display(Name = "İşe Baş. Tar.")]
        public DateTime? StartDate { get; set; }

        [Display(Name = "İşten Çıkç Tar.")]
        public DateTime? EndDate { get; set; }

        //[Display(Name = "Daha Önce Baş. Kur. Çalıştı")]
        //public bool IsWorkBefore { get; set; }
    }

    public class AppUserFileInput
    {
        public int Id { get; set; }
    }
}