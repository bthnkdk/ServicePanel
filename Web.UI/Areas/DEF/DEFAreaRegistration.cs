using System.Web.Mvc;

namespace Web.UI.Areas.DEF
{
    public class DEFAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "DEF";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "DEF_default",
                "DEF/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}