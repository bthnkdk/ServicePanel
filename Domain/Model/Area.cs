using System.Collections.Generic;

namespace Domain
{
    public class Area : DelEntity
    {
        public Area()
        {
            Towns = new HashSet<Town>();
        }
        public override string AuthorityCode => "DEF";
        public string Name { get; set; }
        public ICollection<Town> Towns { get; set; }
    }
    public class City : DelEntity
    {
        public City()
        {
            Towns = new HashSet<Town>();
        }
        public override string AuthorityCode => "DEF";
        public string Name { get; set; }
        public int Plate { get; set; }
        public int OrderId { get; set; }
        public virtual ICollection<Town> Towns { get; set; }

    }
    public class Town : DelEntity
    {
        public override string AuthorityCode => "DEF";
        public string Name { get; set; }
        public int CityId { get; set; }
        public int OrderId { get; set; }
        public int? AreaId { get; set; }
        public virtual Area Area { get; set; }
        public virtual City City { get; set; }
    }
}
