using Omu.AwesomeMvc;
using System;
using System.ComponentModel.DataAnnotations;
using Web.UI.ViewModels;

namespace Web.UI.Areas.CRM
{
    public class VisitInput : BaseInput
    {

        [Required(ErrorMessage = "{0} gerekli")]
        [Display(Name = "Durum")]
        [UIHint("Odropdown")]
        [AweUrl(Controller = "Data", Action = "GetVisitStatus")]
        public int Status { get; set; }

        [Required(ErrorMessage = "{0} gerekli")]
        [Display(Name = "Ziyaret Tipi")]
        [UIHint("Odropdown")]
        [AweUrl(Controller = "Data", Action = "GetVisitTypes")]
        public int VisitTypeId { get; set; }

        [Required(ErrorMessage = "{0} gerekli")]
        [Display(Name = "Ziyaret Kategorisi")]
        [UIHint("Odropdown")]
        [AweUrl(Controller = "Data", Action = "GetVisitCategories")]
        public int VisitCategoryId { get; set; }

        [Display(Name = "Personel")]
        [UIHint("Odropdown")]
        [AweUrl(Controller = "Data", Action = "GetAppUsers")]
        public int AppUserId { get; set; }

        [Required(ErrorMessage = "{0} gerekli")]
        [Display(Name = "Ziyaret Tarihi")]
        public DateTime VisitDate { get; set; }

        [Required(ErrorMessage = "{0} gerekli")]
        [Display(Name = "Ziyaret Konusu")]
        [UIHint("String")]
        public string Subject { get; set; }
        public int CustomerId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedUserId { get; set; }
        public VisitLogInput VisitLog { get; set; }


    }
}