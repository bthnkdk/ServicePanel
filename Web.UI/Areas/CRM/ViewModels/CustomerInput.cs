using Omu.AwesomeMvc;
using System;
using System.ComponentModel.DataAnnotations;
using Web.UI.ViewModels;

namespace Web.UI.Areas.CRM
{
    public class CustomerInput : BaseInput
    {
        public Guid RowId { get; set; }

        [Required(ErrorMessage = "{0} gerekli")]
        [MaxLength(250)]
        [Display(Name = "Ad")]
        public string Name { get; set; }
        [MaxLength(50)]
        [Display(Name = "Müşteri Kodu")]
        public string CustomerCode { get; set; }
        [MaxLength(150)]
        [Display(Name = "Website")]
        public string WebSite { get; set; }
        [Required(ErrorMessage = "{0} gerekli")]
        [Display(Name = "Müşteri Tipi")]
        [UIHint("Odropdown")]
        [AweUrl(Controller = "Data", Action = "GetCustomerTypes")]
        public int CustomerTypeId { get; set; }
        [Required(ErrorMessage = "{0} gerekli")]
        [MaxLength(75)]
        [Display(Name = "Vergi Dairesi")]
        public string VerDar { get; set; }

        [MinLength(10,ErrorMessage ="{0} numarası en az {1} haneli olmalı")]
        [MaxLength(10, ErrorMessage = "{0} numarası en fazla {1} haneli olmalı")]
        [Required(ErrorMessage = "{0} gerekli"), Display(Name = "Vergi Numarası")]
        [UIHint("Int32")]
        public string VerNo { get; set; }
        public LocationInput Location { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedUserId { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? UpdateUserId { get; set; }
    }
}