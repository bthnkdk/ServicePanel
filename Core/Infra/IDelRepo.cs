using System;
using System.Linq;
using System.Linq.Expressions;

namespace Core
{
    public interface IDelRepo<T>
    {
        IQueryable<T> Where(Expression<Func<T, bool>> predicate, bool showDeleted = false);
        IQueryable<T> GetAll(bool asNoTracking = false);
        void Restore(T o);
    }
}