using Infra;
using Liftinsoft.WebApp.Helper;
using System;
using System.Security.Principal;
using System.Web;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using Web.UI.Controllers;
using Web.UI.Helper;
using Web.UI.Mappers;
using Web.UI.ViewModels;
using WebMarkupMin.AspNet4.Common;

namespace Web.UI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AwesomeConfig.Configure();
            MapperConfig.Configure();
            ControllerBuilder.Current.SetControllerFactory(new WindsorControllerFactory(IoC.Container));
            WindsorConfig.Configure();
            WebMarkupMinConfig.Configure(WebMarkupMinConfiguration.Instance);
            AntiForgeryConfig.SuppressIdentityHeuristicChecks = true;
            ModelBinders.Binders.DefaultBinder = new TrimModelBinder();
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            var app = sender as HttpApplication;
            if (app != null && app.Context != null)
                app.Context.Response.Headers.Remove("Server");
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            if (HttpContext.Current.User != null)
            {
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    if (HttpContext.Current.User.Identity is FormsIdentity)
                    {
                        FormsIdentity id = (FormsIdentity)HttpContext.Current.User.Identity;
                        FormsAuthenticationTicket ticket = id.Ticket;
                        HttpContext.Current.User = new GenericPrincipal(id, new string[] { "user" });
                    }
                }
            }
        }

        protected void Session_Start(Object sender, EventArgs e)
        {
            HttpContext currentContext = HttpContext.Current;
            if (currentContext != null)
            {
                if (!currentContext.Request.Browser.Crawler)
                {
                    if (!string.IsNullOrEmpty(currentContext.Request.UserAgent))
                    {
                        if (!currentContext.Request.UserAgent.Contains("uptimerobot"))
                        {
                            Visitor currentVisitor = new Visitor(currentContext);
                            OnlineVisitorsContainer.Visitors[currentVisitor.SessionId] = currentVisitor;
                        }
                    }
                }
            }
        }

        protected void Session_End(Object sender, EventArgs e)
        {
            if (this.Session != null)
            {
                Visitor visitor;
                OnlineVisitorsContainer.Visitors.TryRemove(this.Session.SessionID, out visitor);
            }
        }

        protected void Application_PreRequestHandlerExecute(object sender, EventArgs eventArgs)
        {
            var session = HttpContext.Current.Session;
            if (session != null && HttpContext.Current.User != null && HttpContext.Current.User.Identity.IsAuthenticated)
            {
                if (OnlineVisitorsContainer.Visitors.ContainsKey(session.SessionID))
                {
                    var userName = WebUserManager.GetUserInfo().FullName;
                    OnlineVisitorsContainer.Visitors[session.SessionID].AuthUser = userName;
                }
            }
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var exception = Server.GetLastError();
            Response.Clear();
            HttpContext.Current.Response.TrySkipIisCustomErrors = true;
            var httpException = exception as HttpException;
            var routeData = new RouteData();
            routeData.Values.Add("controller", "Error");
            if (httpException == null)
                routeData.Values.Add("action", "Index");
            else
            {
                switch (httpException.GetHttpCode())
                {
                    case 404:
                        routeData.Values.Add("action", "HttpError404");
                        break;
                    case 505:
                        routeData.Values.Add("action", "HttpError505");
                        break;
                    default:
                        routeData.Values.Add("action", "Index");
                        break;
                }
            }
            if (exception.Message.Contains("farklý bir alana"))
                routeData.Values.Add("error", new Exception(exception.Message));
            else
                routeData.Values.Add("error", exception);
            Server.ClearError();
            try
            {
                IController errorController = new ErrorController();
                errorController.Execute(new RequestContext(
                    new HttpContextWrapper(Context), routeData));
            }
            catch (Exception)
            {
                var rd = new RouteData();
                rd.Values.Add("controller", "Error");
                rd.Values.Add("action", "Master");
                rd.Values.Add("error", exception);
                IController errorController = new ErrorController();
                errorController.Execute(new RequestContext(
                    new HttpContextWrapper(Context), rd));
            }
        }
    }
}

