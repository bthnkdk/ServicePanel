using Core;
using Domain;
using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Data
{
    public class DelRepo<T> : IDisposable, IDelRepo<T> where T : DelEntity
    {
        protected readonly DbContext dbContext;

        public DelRepo(IDbContextFactory dbContextFactory)
        {
            dbContext = dbContextFactory.GetContext();
        }

        public IQueryable<T> Where(Expression<Func<T, bool>> predicate, bool showDeleted = false)
        {
            var res = dbContext.Set<T>().Where(predicate);
            if (!showDeleted) res = res.Where(o => o.IsDeleted == false);
            return res;
        }

        public IQueryable<T> GetAll(bool asNoTracking = false)
        {
            if (asNoTracking)
                return dbContext.Set<T>().AsNoTracking().Where(o => o.IsDeleted == false);
            else
                return dbContext.Set<T>().Where(o => o.IsDeleted == false);
        }

        public void Restore(T o)
        {
            o.IsDeleted = false;
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