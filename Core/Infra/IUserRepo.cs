namespace Core
{
    public interface IUserRepo<AppUser> : IRepo<AppUser>
    {
        AppUser GetActiveAppUserFromUsername(string username);

        void UpdatePassword(int id, string oldPassword, string newPassword);

        void ResetPassword(int id, string username, string code, string password);

        AppUser ForgetPassword(string username);
    }
}