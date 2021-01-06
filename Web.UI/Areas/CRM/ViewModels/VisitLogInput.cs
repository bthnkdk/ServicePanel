using Omu.AwesomeMvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Web.UI.ViewModels;

namespace Web.UI.Areas.CRM
{
    public class VisitLogInput:BaseInput
    {
        public int Status { get; set; }
        public int VisitId { get; set; }
        public int AppUserId { get; set; }
      
        [Display(Name = "Açıklama")]
        [UIHint("Textarea")]
        public string Description { get; set; }
     
        [Display(Name = "Açıklama 2")]
        [UIHint("Textarea")]
        public string Description2 { get; set; }
        public DateTime Date { get; set; }
    }
}