namespace Domain
{
    public interface IEntity
    {
        int Id { get; set; }
        string AuthorityCode { get; }
    }
}