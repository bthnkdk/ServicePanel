using System.Web.Mvc;

namespace Web.UI.Helper
{
    public class TrimModelBinder : DefaultModelBinder
    {
        protected override void SetProperty(System.Web.Mvc.ControllerContext controllerContext,
          System.Web.Mvc.ModelBindingContext bindingContext,
          System.ComponentModel.PropertyDescriptor propertyDescriptor, object value)
        {
            if (propertyDescriptor.PropertyType == typeof(string))
            {
                var stringValue = (string)value;
                if (!string.IsNullOrEmpty(stringValue))
                {
                    value = stringValue.Trim();
                }
                else
                {
                    value = null;
                }
            }
            base.SetProperty(controllerContext, bindingContext,
                                propertyDescriptor, value);
        }
    }
}