using Core;
using Domain;
using Omu.AwesomeMvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Web.UI.Helper;

namespace Web.UI.Controllers
{
    /// <summary>
    /// Sadece Veri çekmek için (Select işlemleri)
    /// </summary>
    /// <typeparam name="T">Entity</typeparam>
    public class ROGenericController<T> : BaseController
        where T : Entity, new()
    {
        protected readonly IRepo<T> repo;

        public ROGenericController(IRepo<T> repo)
        {
            this.repo = repo;
        }

        string GetCode()
        {
            return Activator.CreateInstance<T>().AuthorityCode;
        }

        public virtual ActionResult Index()
        {
            WebUserManager.CheckIsAuthorized($"{GetCode()}.S");
            return PartialView();
        }

        protected virtual object MapEntityToGridModel(T entity)
        {
            return entity;
        }

        public virtual object MakeFooter(GroupInfo<T> info)
        {
            return null;
        }

        public virtual GridModelDto<T> GetGridModelDto(GridParams g, IQueryable<T> items)
        {
            return new GridModelBuilder<T>(items, g)
            {
                Key = "Id",
                Map = MapEntityToGridModel,
                MakeFooter = MakeFooter,
                GetItem = () => repo.Get(Convert.ToInt32(g.Key))
            }.Build();
        }

        public virtual GridModel<T> GetGridModel(GridParams g, IQueryable<T> items)
        {
            return new GridModelBuilder<T>(items, g)
            {
                Key = "Id",
                Map = MapEntityToGridModel,
                GetItem = () => repo.Get(Convert.ToInt32(g.Key))
            }.BuildModel();
        }

        protected string[] GetFields()
        {
            List<string> fields = new List<string>();
            Type t = typeof(T);
            foreach (var item in t.GetProperties())
                fields.Add(item.Name);
            return fields.ToArray();
        }
    }
}