using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.UI.Areas.SVC
{
    public class ServiceListViewModel
    {
        public int AllServiceCount { get; set; }
        public int CompletedServiceCount { get; set; }
        public int WaitApprovalCount { get; set; }
        //public int ServiceCategoryCount { get; set; }
        //public int ServicePersonalCount { get; set; }
        //public int ServiceTownCount { get; set; }
        public List<ServiceCategoryViewModel> ServiceCategories { get; set; }
        public List<ServicePersonalViewModel> ServicePersonals { get; set; }
        public List<ServiceTownViewModel> ServiceTowns { get; set; }
    }

    public class ServicePersonalViewModel
    {
        public string Name { get; set; }
        public int Count { get; set; }
        public int? ServicePersonalId { get; set; }
    }
    public class ServiceTownViewModel
    {
        public string Name { get; set; }
        public int Count { get; set; }
        public int? ServiceTownId { get; set; }
    }
    public class ServiceCategoryViewModel
    {
        public string Name { get; set; }
        public int Count { get; set; }
        public int? ServiceCategoryId { get; set; }
    }

    
}