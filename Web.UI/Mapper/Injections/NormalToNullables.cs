using Omu.ValueInjecter.Injections;
using System;
using System.Reflection;

namespace Web.UI.Mappers.Injections
{
    public class NormalToNullables : LoopInjection
    {
        protected override bool MatchTypes(Type source, Type target)
        {
            return source == Nullable.GetUnderlyingType(target);
        }

        protected override void SetValue(object source, object target, PropertyInfo sp, PropertyInfo tp)
        {
            var val = sp.GetValue(source);

            if (sp.PropertyType == typeof(int) && (int)val == default(int) ||
                sp.PropertyType == typeof(DateTime) && (DateTime)val == default(DateTime))
                return;

            tp.SetValue(target, val);
        }
    }
}