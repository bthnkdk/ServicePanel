namespace Web.UI.Mappers
{
    public class Mapper : IMapper
    {
        public TResult Map<TSource, TResult>(TSource src, object tag = null)
        {
            return MapperConfig.CrudMapper.Map<TSource, TResult>(src, tag);
        }
    }
}