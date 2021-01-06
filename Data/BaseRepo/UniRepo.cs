using Core;
using Domain;
using Omu.ValueInjecter;
using System;
using System.Data.Entity;
using System.Linq;

namespace Data
{
    public class UniRepo : IDisposable, IUniRepo
    {
        private readonly DbContext c;

        public UniRepo(IDbContextFactory a)
        {
            c = a.GetContext();
        }

        public T Insert<T>(T o) where T : Entity, new()
        {
            var t = new T();
            t.InjectFrom(o);
            c.Set<T>().Add(t);
            return t;
        }

        public void Save()
        {
            c.SaveChanges();
        }

        public T Get<T>(int id) where T : Entity
        {
            return c.Set<T>().Find(id);
        }

        public IQueryable<T> All<T>(bool asNoTracking = false) where T : Entity
        {
            if (asNoTracking)
                return c.Set<T>().AsNoTracking();
            else
                return c.Set<T>();
        }

        public IQueryable<T> GetAll<T>(bool showDeleted) where T : DelEntity
        {
            if (showDeleted)
            {
                return c.Set<T>();
            }

            return c.Set<T>().Where(o => !o.IsDeleted);
        }

        #region Dispose

        bool disposed;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if (c != null)
                        c.Dispose();
                }
                disposed = true;
            }
        }

        #endregion
    }
}