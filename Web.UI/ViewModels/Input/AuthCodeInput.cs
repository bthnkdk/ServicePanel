using System.Collections.Generic;

namespace Web.UI.ViewModelss
{
    public class AuthCodeInput
    {
        public string AppUserName { get; set; }
        public int AppUserId { get; set; }
        public IList<string> Codes { get; set; }
    }
}