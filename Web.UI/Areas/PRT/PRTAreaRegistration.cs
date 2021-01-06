using System.Web.Mvc;

namespace Web.UI.Areas.PRT
{
    public class PRTAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "PRT";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "PRT_default",
                "PRT/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}