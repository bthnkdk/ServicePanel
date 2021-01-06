using Domain;
using System.Linq;

namespace Core
{
    public interface IUniRepo
    {
        T Insert<T>(T o) where T : Entity, new();
        void Save();
        T Get<T>(int id) where T : Entity;
        IQueryable<T> All<T>(bool asNoTracking = false) where T : Entity;
        IQueryable<T> GetAll<T>(bool showDeleted = false) where T : DelEntity;
    }
}