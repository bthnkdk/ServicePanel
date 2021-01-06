using System;
using System.Web.Configuration;
using System.Web.Mvc;
using WebMarkupMin.AspNet4.Mvc;

namespace Web.UI.Controllers
{
    [MinifyHtml]
    public class ErrorController : Controller
    {
        public ActionResult Index(Exception error)
        {
            SetMessage(error);
            if (error.Message != null && error.Message.Contains("The parameters dictionary contains") || error is ArgumentNullException)
            {
                Response.StatusCode = 404;
                if (Request.IsAjaxRequest())
                    return PartialView("Resource404");
                return View("Resource404");
            }

            if (error is Exception)
            {
                if (Request.IsAjaxRequest())
                    return PartialView("Expected");
                return View("Expected");
            }

            if (Response.StatusCode == 200)
                Response.StatusCode = 500;

            if (Request.IsAjaxRequest())
                return PartialView("ErrorPartial");

            return View("Error");
        }

        public ActionResult Master(Exception error)
        {
            SetMessage(error);
            return View();
        }

        public ActionResult HttpError404(Exception error)
        {
            Response.StatusCode = 404;
            if (Request.IsAjaxRequest())
                return Content("404 not found");
            return View();
        }

        public ActionResult HttpError505(Exception error)
        {
            return View();
        }

        private void SetMessage(Exception error)
        {
            var compilationSection = (CompilationSection)System.Configuration.ConfigurationManager.GetSection(@"system.web/compilation");
            if (compilationSection.Debug)
            {
                ViewData["debugInfo"] = "This message is showing because there is <code>compilation debug=\"true\"</code> in web.config";
                ViewData["message"] = error.ToString();
            }
            else
            {
                ViewData["debugInfo"] = "Set <code>compilation debug=\"true\"</code> in web.config to get more details";
                ViewData["message"] = Message(error);
            }
        }

        private string Message(Exception ex)
        {
            if (ex == null) return "";
            if (HttpContext.Request.IsLocal)
                return ex.Message + "\n" + Message(ex.InnerException);
            else
                return ex.Message;
        }
    }
}