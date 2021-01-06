using Core;
using Domain;
using System;
using System.Linq;

namespace Data
{
    public class UserRepo : Repo<AppUser>, IUserRepo<AppUser>
    {
        public UserRepo(IDbContextFactory dbCtxFact) : base(dbCtxFact)
        {
        }

        public AppUser ForgetPassword(string username)
        {
            var user = GetActiveAppUserFromUsername(username);
            if (user == null)
                throw new Exception("Girilen e-posta adresine ait kullanıcı bulunamadı !");

            user.TokenKey = Guid.NewGuid().ToString().ToLower();
            base.Save();
            return user;
        }

        public AppUser GetActiveAppUserFromUsername(string username)
        {
            return Where(p => p.Username == username && !p.IsDeleted && !p.IsLock).FirstOrDefault();
        }

        public void ResetPassword(int id, string username, string code, string password)
        {
            var user = GetActiveAppUserFromUsername(username);
            if (user == null)
                throw new Exception("E-Posta adresi kayıtlı değil !");
            if (id != user.Id)
                throw new Exception("Geçersiz kullanıcı isteği");
            if (code != user.TokenKey)
                throw new Exception("Geçersiz istek kodu");
            user.HashPassword(password);
            user.TokenKey = Guid.NewGuid().ToString().ToLower();
            base.Save();
        }

        public void UpdatePassword(int id, string oldPassword, string newPassword)
        {
            var entity = Get(id);
            if (!entity.VerifyHashedPassword(oldPassword))
                throw new Exception("Mevcut parolanız hatalı !");
            entity.HashPassword(newPassword);
            Save();
        }
    }
}