using System;
using System.Web.Mvc;
using WebMarkupMin.AspNet4.Mvc;

namespace Web.UI.Controllers
{
    [Authorize]
    [MinifyHtml]
    public class BaseController : Controller
    {
        protected override void OnException(ExceptionContext filterContext)
        {
            if (filterContext.ExceptionHandled)
                return;

            if (filterContext.Exception.GetType() == typeof(UnauthorizedAccessException))
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    filterContext.Result = new ViewResult
                    {
                        ViewName = "~/Views/Home/NoAccess.cshtml"
                    };
                }
                else
                {
                    filterContext.Result = new ViewResult
                    {
                        ViewName = "~/Views/Home/NoAccess.cshtml"
                    };
                }

                filterContext.ExceptionHandled = true;
                return;
            }
            base.OnException(filterContext);
        }
    }
}