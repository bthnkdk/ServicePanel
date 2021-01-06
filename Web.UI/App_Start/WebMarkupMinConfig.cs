using WebMarkupMin.AspNet4.Common;

namespace Web.UI
{
    public class WebMarkupMinConfig
    {
        public static void Configure(WebMarkupMinConfiguration configuration)
        {
            configuration.AllowMinificationInDebugMode = false;
            configuration.AllowCompressionInDebugMode = false;
        }
    }
}