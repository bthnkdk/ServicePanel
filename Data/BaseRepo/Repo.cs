using Core;
using Domain;
using Infra;
using Omu.ValueInjecter;
using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Data
{
    public class Repo<T> : IDisposable, IRepo<T> where T : Entity, new()
    {
        protected readonly DbContext dbContext;

        public Repo(IDbContextFactory dbCtxFact)
        {
            if (dbContext == null)
                dbContext = dbCtxFact.GetContext();
        }

        public void Save()
        {
            dbContext.SaveChanges();
        }

        public T Insert(T o)
        {
            var t = dbContext.Set<T>().Create();
            t.InjectFrom(o);
            dbContext.Set<T>().Add(t);
            return t;
        }

        public virtual void Delete(T o)
        {
            var del = o as IDelete;
            if (del != null)
                del.IsDeleted = true;
            else
                dbContext.Set<T>().Remove(o);
        }

        public T Get(int id)
        {
            var entity = dbContext.Set<T>().Find(id);
            if (entity == null) throw new Exception("Kayıt mevcut değil !");
            return entity;
        }

        public T GetCopy(int id, string includeProperties = "")
        {
            var query = dbContext.Set<T>().AsNoTracking();
            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
            var entity = query.SingleOrDefault(p => p.Id == id);
            if (entity == null) throw new Exception("Kayıt mevcut değil !");
            return entity;
        }

        public void Restore(T o)
        {
            if (o is IDelete)
                IoC.Resolve<IDelRepo<T>>().Restore(o);
        }

        public virtual IQueryable<T> Where(Expression<Func<T, bool>> predicate, bool showDeleted = false)
        {
            if (typeof(IDelete).IsAssignableFrom(typeof(T)))
                return IoC.Resolve<IDelRepo<T>>().Where(predicate, showDeleted);
            return dbContext.Set<T>().Where(predicate);
        }

        public virtual IQueryable<T> GetAll(bool asNoTracking = false)
        {
            if (typeof(IDelete).IsAssignableFrom(typeof(T)))
                return IoC.Resolve<IDelRepo<T>>().GetAll(asNoTracking);

            if (asNoTracking)
                return dbContext.Set<T>().AsNoTracking();
            else
                return dbContext.Set<T>();
        }

        public int Count(Expression<Func<T, bool>> predicate)
        {
            return dbContext.Set<T>().Count(predicate);
        }

        public bool Any(Expression<Func<T, bool>> predicate)
        {
            return dbContext.Set<T>().Any(predicate);
        }

        public void ExecuteSqlCommand(string sql)
        {
            dbContext.Database.ExecuteSqlCommand(sql);
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
                    if (dbContext != null)
                        dbContext.Dispose();
                }
                disposed = true;
            }
        }

        #endregion
    }

    public class ViewRepo<V> : IDisposable, IViewRepo<V> where V : ViewEntity
    {
        protected readonly DbContext dbContext;

        public ViewRepo(IDbContextFactory dbCtxFact)
        {
            dbContext = dbCtxFact.GetContext();
        }

        public V Get(int id)
        {
            var entity = dbContext.Set<V>().Find(id);
            if (entity == null) throw new Exception("Kayıt mevcut değil !");
            return entity;
        }

        public virtual IQueryable<V> Where(Expression<Func<V, bool>> predicate, bool showDeleted = false)
        {
            return dbContext.Set<V>().Where(predicate);
        }

        public virtual IQueryable<V> GetAll(bool asNoTracking = false)
        {
            if (asNoTracking)
                return dbContext.Set<V>().AsNoTracking();
            else
                return dbContext.Set<V>();
        }

        public int Count(Expression<Func<V, bool>> predicate)
        {
            return dbContext.Set<V>().Count(predicate);
        }

        public bool Any(Expression<Func<V, bool>> predicate)
        {
            return dbContext.Set<V>().Any(predicate);
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
                    if (dbContext != null)
                        dbContext.Dispose();
                }
                disposed = true;
            }
        }

        #endregion
    }
}