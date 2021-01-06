using Core;
using Domain;
using System;

namespace Data
{
    public class AppUserPermissionRepo : Repo<AppUserPermission>, IAppUserPermissionRepo<AppUserPermission>
    {
        public AppUserPermissionRepo(IDbContextFactory dbCtxFact) : base(dbCtxFact)
        {
        }

        public bool IsExists(DateTime StartDate, DateTime EndDate, int Id, int AppUserId)
        {
            return Any(p => ((p.StartDate >= StartDate & p.StartDate <= EndDate) || (p.EndDate >= StartDate & p.EndDate <= EndDate)) && p.Id != Id && p.AppUserId == AppUserId);
        }
    }
}
