using Domain;
using Omu.ValueInjecter;
using System;
using Web.UI.Mappers.Injections;
using Web.UI.ViewModels;

namespace Web.UI.Mappers
{
    public class MapperConfig
    {
        public static MapperInstance CrudMapper = new MapperInstance();

        public static void Configure()
        {
            CrudMapper.DefaultMap = (src, resType, tag) =>
                {
                    var res = tag != null && tag.GetType().IsSubclassOf(typeof(Entity)) ? tag : Activator.CreateInstance(resType);

                    res.InjectFrom(src);
                    var srcType = src.GetType();

                    if (srcType.IsSubclassOf(typeof(Entity)) && resType.IsSubclassOf(typeof(BaseInput)))
                    {
                        res.InjectFrom<NormalToNullables>(src)
                           .InjectFrom<EntitiesToInts>(src);
                    }
                    else if (srcType.IsSubclassOf(typeof(BaseInput)) && resType.IsSubclassOf(typeof(Entity)))
                    {
                        res.InjectFrom<IntsToEntities>(src)
                           .InjectFrom<NullablesToNormal>(src);
                    }

                    return res;
                };
        }
    }
}