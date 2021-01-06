using System;

namespace Web.UI.ViewModels
{
    public class AppUserPermissionGetAlertView
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Duration { get; set; }
        public string Description { get; set; }
    }
}