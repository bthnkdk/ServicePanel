using System.Globalization;
using Web.UI.ViewModels;

namespace Web.UI.Helper
{
    public static class ClientSideHelper
    {
        public static ClientDict GetClientDict()
        {
            return new ClientDict
            {
                GridInfo = " arası toplam {0}",
                Select = "Seçin",
                SearchForRes = "daha fazla ...",
                Searchp = "ara ...",
                Months = DateTimeFormatInfo.CurrentInfo.MonthNames,
                Days = DateTimeFormatInfo.CurrentInfo.ShortestDayNames,
                NoRecFound = "Hiç kayıt bulunamadı"
            };
        }
    }
}