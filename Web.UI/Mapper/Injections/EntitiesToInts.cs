using Domain;
using Omu.ValueInjecter.Injections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Web.UI.Mappers.Injections
{
    public class EntitiesToInts : LoopInjection
    {
        protected override bool MatchTypes(Type src, Type trg)
        {
            return trg == typeof(int[])
                && src.IsGenericType
                && src.GetGenericTypeDefinition() == typeof(ICollection<>)
                && src.GetGenericArguments()[0].IsSubclassOf(typeof(Entity));
        }

        protected override void SetValue(object source, object target, PropertyInfo sp, PropertyInfo tp)
        {
            var val = sp.GetValue(source);
            if (val != null)
            {
                tp.SetValue(target, (val as IEnumerable<Entity>).Select(o => o.Id).ToArray());
            }
        }
    }
}