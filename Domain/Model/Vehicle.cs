namespace Domain
{
    public class Vehicle : DelEntity
    {
        public override string AuthorityCode => "DEF";

        public string Name { get; set; }
        public string Plate { get; set; }
        public int AppUserId { get; set; }
        public virtual AppUser AppUser { get; set; }
    }
}
