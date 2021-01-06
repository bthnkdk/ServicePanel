using Omu.AwesomeMvc;
using System;
using System.ComponentModel.DataAnnotations;
using Web.UI.ViewModels;

namespace Web.UI.Areas.CRM
{
    public class PreApplicationInput : BaseInput
    {

        [Required(ErrorMessage = "{0} gerekli")]
        [Display(Name = "Başvuru Tipi")]
        [UIHint("Odropdown")]
        [AweUrl(Controller = "Data", Action = "GetPreApplicationTypes")]
        public int PreApplicationTypeId { get; set; }

        [Required(ErrorMessage = "{0} gerekli")]
        [Display(Name = "Ön Başvuru Durumu")]
        [UIHint("Odropdown")]
        [AweUrl(Controller = "Data", Action = "GetPreApplicationStatus")]
        public int PreApplicationStatusId { get; set; }

        [Required(ErrorMessage = "{0} gerekli")]
        [MaxLength(100)]
        [UIHint("String")]
        [Display(Name = "Firma İsmi")]
        public string CustomerName { get; set; }

        [Required(ErrorMessage = "{0} gerekli")]
        [MaxLength(50)]
        [UIHint("String")]
        [Display(Name = "Yetkili")]
        public string CustomerAuthorized { get; set; }

        [Required(ErrorMessage = "{0} gerekli")]
        [Display(Name = "E-Mail")]
        [MaxLength(100, ErrorMessage = "Max {1}")]
        [UIHint("String")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi girin")]
        public string CustomerEmail { get; set; }

        [Required(ErrorMessage = "{0} gerekli")]
        [MaxLength(10)]
        [UIHint("String")]
        [Display(Name = "Telefon")]
        public string CustomerPhone { get; set; }

        [Display(Name = "Açıklama")]
        [MaxLength(250, ErrorMessage = "Max {1}")]
        [UIHint("Textarea")]
        public string Description { get; set; }
        public int CreatedUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? UpdatedUserId { get; set; }
        public DateTime? UpdatedDate { get; set; }





    }
}