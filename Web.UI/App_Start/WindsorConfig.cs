using Core;
using Data;
using Domain;
using Infra;

namespace Web.UI
{
    public class WindsorConfig
    {
        public static void Configure()
        {
            WindsorRegistrar.Register(typeof(IUserRepo<AppUser>), typeof(UserRepo));
            WindsorRegistrar.Register(typeof(IAppUserPermissionRepo<AppUserPermission>), typeof(AppUserPermissionRepo));

            WindsorRegistrar.RegisterAllFromAssemblies("Data");
            WindsorRegistrar.RegisterAllFromAssemblies("Web.UI");
        }
    }
}