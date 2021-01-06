using Omu.AwesomeMvc;
using System;
using System.ComponentModel.DataAnnotations;
using Web.UI.ViewModels;

namespace Web.UI.Areas.CRM
{
    public class LocationInput : BaseInput
    {
        public Guid RowId { get; set; }

        [Required(ErrorMessage = "{0} gerekli")]
        [Display(Name = "Müşteri Adı")]
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "{0} gerekli")]
        [MaxLength(100)]
        [Display(Name = "Ad")]
        public string Name { get; set; }

        [Display(Name = "E-Posta")]
        [MaxLength(120, ErrorMessage = "Max {1}")]
        [UIHint("String")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi girin")]
        public string Mail { get; set; }

        [UIHint("Int32"), MaxLength(20)]
        [Display(Name = "Telefon")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "{0} gerekli")]
        [Display(Name = "Şehir")]
        [UIHint("Odropdown")]
        [AweUrl(Controller = "Data", Action = "GetCities")]
        public int CityId { get; set; }

        [Required(ErrorMessage = "{0} gerekli")]
        [Display(Name = "Semt")]
        [UIHint("Odropdown")]
        [AweUrl(Controller = "Data", Action = "GetTowns")]
        [AweParent(ParentId = "CityId")]
        public int TownId { get; set; }
        [Required(ErrorMessage = "{0} gerekli")]
        [MaxLength(350)]
        [Display(Name = "Adres")]
        public string Address { get; set; }
        [Required(ErrorMessage = "{0} gerekli")]
        [MaxLength(100)]
        [Display(Name = "Yetkili Adı")]
        public string ResponsibleName { get; set; }

        [Display(Name = "Yetkili E-Posta")]
        [MaxLength(150, ErrorMessage = "Max {1}")]
        [UIHint("String")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi girin")]
        public string ResponsibleMail { get; set; }

        [UIHint("Int32"),MaxLength(20)]
        [Display(Name = "Yetkili Telefon")]
        public string ResponsiblePhone { get; set; }

        [Display(Name = "İlgilenecek Departman")]
        [UIHint("Odropdown")]
        [AweUrl(Controller = "Data", Action = "GetDepartments")]
        public int? DefaultDepartmentId { get; set; }

        [Display(Name = "İlgilenecek Personel")]
        [UIHint("Odropdown")]
        [AweUrl(Controller = "Data", Action = "GetAppUsers")]
        [AweParent(ParentId = "DepartmentId")]
        public int? DefaultAppUserId { get; set; }

        public DateTime CreatedDate { get; set; }
        public int CreatedUserId { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? UpdateUserId { get; set; }
    }
}