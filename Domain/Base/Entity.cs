namespace Domain
{
    public class Entity : IEntity
    {
        public virtual int Id { get; set; }
        public virtual string AuthorityCode { get { return null; } }
    }
}