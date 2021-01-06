using System.Collections.Generic;

namespace Web.UI.Areas.SYS
{
    public class AppUserAuthorityInput
    {
        public string AppUserName { get; set; }
        public int AppUserId { get; set; }
        public IList<AuthorityInput> Codes { get; set; }
    }
    public class AuthorityInput
    {
        public int AuthCodeId { get; set; }
        public string Access { get; set; }
    }
}