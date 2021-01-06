using System.Web.Mvc;

namespace Web.UI.Areas.SYS
{
    public class DEFAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "SYS";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "SYS_default",
                "SYS/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}