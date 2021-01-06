using Omu.AwesomeMvc;

namespace Web.UI
{
    public static class AwesomeConfig
    {
        public static void Configure()
        {
            Settings.Lookup.HighlightChange = true;
            Settings.MultiLookup.HighlightChange = true;
            Settings.GetText = GetTranslate;
        }

        private static string GetTranslate(string type, string key)
        {
            if (type == "Form.ConfirmOptions" && key == "Title") return "";
            if (type == "Form.ConfirmOptions" && key == "Message") return "Silinecek ?";
            if (type == "PopupForm" && key == "Title") return "";
            if (type == "Lookup" && key == "Title") return "";
            if (type == "MultiLookup" && key == "Title") return "";

            switch (key)
            {
                case "CancelText": return "İptal";
                case "YesText": return "Evet";
                case "Yes": return "Evet";
                case "NoText": return "Hayır";
                case "No": return "Hayır";
                case "MoreText": return "Daha fazla ...";
                case "SearchText": return "ara ...";
                case "GroupBarText": return "Gruplamak istediğiniz alanları sürükleyin";
            }

            return null;
        }
    }
}