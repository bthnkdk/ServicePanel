using System.Web.Mvc;

namespace Web.UI.Areas.STK
{
    public class STKAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "STK";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "STK_default",
                "STK/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}