using System.Web.Mvc.Ajax;

namespace Web.UI.Helper
{
    public class PostHelper
    {
        public static AjaxOptions ClassicPost(string url)
        {
            return new AjaxOptions
            {
                OnFailure = "OnAjaxFailure",
                Url = url,
                HttpMethod = "POST",
                OnComplete = "OnAjaxComplete"
            };
        }
    }
}