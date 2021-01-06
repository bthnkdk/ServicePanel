using System.Collections.Generic;

namespace Domain
{
    public class Department : DelEntity
    {
        public override string AuthorityCode => "DEF";

        public Department()
        {
            AppUsers = new HashSet<AppUser>();
        }
        public string Name { get; set; }
        public string Code { get; set; }
        public virtual ICollection<AppUser> AppUsers { get; set; }
    }
}
