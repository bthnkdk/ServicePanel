using System;
using System.Linq;
using System.Linq.Expressions;

namespace Core
{
    public interface IRepo<T>
    {
        T Get(int id);
        T GetCopy(int id, string includeProperties);
        IQueryable<T> GetAll(bool asNoTracking = false);
        IQueryable<T> Where(Expression<Func<T, bool>> predicate, bool showDeleted = false);
        T Insert(T o);
        int Count(Expression<Func<T, bool>> predicate);
        bool Any(Expression<Func<T, bool>> predicate);
        void Save();
        void Delete(T o);
        void Restore(T o);
        void ExecuteSqlCommand(string sql);
    }

    public interface IViewRepo<V>
    {
        V Get(int id);
        IQueryable<V> GetAll(bool asNoTracking = false);
        IQueryable<V> Where(Expression<Func<V, bool>> predicate, bool showDeleted = false);
        int Count(Expression<Func<V, bool>> predicate);
        bool Any(Expression<Func<V, bool>> predicate);
    }
}