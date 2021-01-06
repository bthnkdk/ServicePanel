using System.Collections.Generic;

namespace Domain
{
    public class AuthCode : Entity
    {
        public AuthCode()
        {
            AppUserAuthorities = new HashSet<AppUserAuthority>();
            Menus = new HashSet<Menu>();
        }
        public string Name { get; set; }
        public string Code { get; set; }
        public virtual ICollection<AppUserAuthority> AppUserAuthorities { get; set; }
        public virtual ICollection<Menu> Menus { get; set; }
    }
}
