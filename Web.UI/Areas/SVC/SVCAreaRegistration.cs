using System.Web.Mvc;

namespace Web.UI.Areas.SVC
{
    public class SVCAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "SVC";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "SVC_default",
                "SVC/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}