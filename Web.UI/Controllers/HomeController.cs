using Core;
using Domain;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Web.UI.Helper;
using WebMarkupMin.AspNet4.Mvc;

namespace Web.UI.Controllers
{
    [MinifyHtml]
    public class HomeController : BaseController
    {
        readonly IUniRepo repo;

        public HomeController(IUniRepo repo)
        {
            this.repo = repo;
        }

        public ActionResult Main()
        {
            return PartialView(WebUserManager.GetUserInfo());
        }
        public ActionResult Index()
        {
            return PartialView();
        }

        public ActionResult NoAccess()
        {
            return PartialView();
        }

        [ChildActionOnly]
        public ActionResult MenuList()
        {
            var userInfo = WebUserManager.GetUserInfo();
            var authCodeList = WebUserManager.GetAuthCodes().Where(p => p.Key == userInfo.Id);
            if (userInfo.IsAdmin)
                return PartialView("_Menu", repo.GetAll<Menu>().FirstOrDefault());
            else
            {
                int appUserId = userInfo.Id;
                var authCodes = authCodeList.Where(p => p.Value.EndsWith(".S")).Select(p => p.Value).ToList().Select(p => p.Split(".".ToCharArray())[0]).ToList();
                authCodes.Add("#");
                //var menus = AuthorizedMenus(repo.GetAll<Menu>().FirstOrDefault(s => s.ParentId == null),authCodes);
                var menuList = repo.GetAll<Menu>().Where(p => p.ParentId == null).ToList().Select(s => new Menu
                {
                    Name = s.Name,
                    ParentId = s.ParentId,
                    IsDeleted = s.IsDeleted,
                    Url = s.Url,
                    Icon = s.Icon,
                    Parent = s.Parent,
                     OrderNumber = s.OrderNumber,
                     Id=s.Id,
                    Menu1 = s.Menu1.Where(c => authCodes.Contains(c.AuthCode.Code)).Select(x => new Menu
                    {
                        Name = x.Name,
                        ParentId = x.ParentId,
                        IsDeleted = x.IsDeleted,
                        Url = x.Url,
                        Icon = x.Icon,
                        Parent = x.Parent,
                         OrderNumber =x.OrderNumber,
                         Id = x.Id,
                        Menu1 = x.Menu1.Where(m => authCodes.Contains(m.AuthCode.Code)).Select(y => new Menu
                        {
                            Name = y.Name,
                            ParentId = y.ParentId,
                            IsDeleted = y.IsDeleted,
                            Url = y.Url,
                            Icon = y.Icon,
                            Parent = y.Parent,
                             OrderNumber =y.OrderNumber,
                            Id = y.Id,
                            Menu1 = y.Menu1.Where(q => authCodes.Contains(q.AuthCode.Code)).Select(b => new Menu
                            {
                                Name = b.Name,
                                ParentId = b.ParentId,
                                IsDeleted = b.IsDeleted,
                                Url = b.Url,
                                Icon = b.Icon,
                                Parent = b.Parent,
                                 OrderNumber =b.OrderNumber,
                                 Id = b.Id,
                                Menu1 = b.Menu1.Where(g => authCodes.Contains(g.AuthCode.Code)).ToList()
                            }).ToList()
                        }).ToList()
                    }).ToList()
                }).FirstOrDefault();
                return PartialView("_Menu", menuList);
            }
        }
        public List<Menu> AuthorizedMenus(Menu menu, List<string> authorityCodes)
        {
            List<Menu> menuList = new List<Menu>();
            menuList.Add(menu);

            foreach (var menuItem in menu.Menu1.Where(p => authorityCodes.Contains(p.AuthCode.Code)))
            {
                menuList.AddRange(AuthorizedMenus(menuItem, authorityCodes));
            }
            return menuList;
        }
    }
}