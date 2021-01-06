using System.Collections.Generic;

namespace Domain
{
    public class Menu : DelEntity, IDelete
    {
        public Menu()
        {
            Menu1 = new HashSet<Menu>();
        }

        public override string AuthorityCode => "SYS";
        public string Name { get; set; }
        public string Url { get; set; }
        public string Icon { get; set; }
        public int OrderNumber { get; set; }
        public int? ParentId { get; set; }
        public int AuthCodeId { get; set; }

        public virtual Menu Parent { get; set; }
        public virtual AuthCode AuthCode { get; set; }
        public virtual ICollection<Menu> Menu1 { get; set; }
    }
}
