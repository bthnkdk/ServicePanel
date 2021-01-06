namespace Domain
{
    public class DelEntity : Entity, IDelete
    {
        public bool IsDeleted { get; set; }
    }
}