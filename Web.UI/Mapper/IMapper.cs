namespace Web.UI.Mappers
{
    public interface IMapper
    {
        TResult Map<TSource, TResult>(TSource src, object tag = null);
    }
}