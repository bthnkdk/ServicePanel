using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;

namespace Web.UI.Helper
{
    public static class ExceptionHelper
    {
        public static string GetMessage(this Exception ex)
        {
            StringBuilder expS = new StringBuilder();
            expS.Append(ex.Message);
            Exception inner = ex.InnerException;
            while (inner != null)
            {
                expS.AppendFormat("<br>-{0}", inner.Message);
                inner = inner.InnerException;
            }
            return expS.ToString();
        }

        public static string[] ValidationErrors(ModelStateDictionary model)
        {
            List<string> errorList = new List<string>();
            foreach (var value in model.Values)
                foreach (var error in value.Errors)
                    errorList.Add(error.ErrorMessage);

            return errorList.ToArray();
        }
    }
}