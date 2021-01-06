using System;

namespace Core
{
    public interface IAppUserPermissionRepo<AppUserPermission> : IRepo<AppUserPermission>
    {
        bool IsExists(DateTime StartDate, DateTime EndDate, int Id, int AppUserId);
    }
}