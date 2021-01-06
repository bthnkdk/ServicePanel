using Core;
using Domain;
using Omu.AwesomeMvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Web.UI.Helper;
using Web.UI.Mappers;
using Web.UI.ViewModels;
using static Web.UI.EnumHelper;

namespace Web.UI.Controllers
{
    /// <summary>
    /// Del Entity için
    /// </summary>
    /// <typeparam name="T">Entity</typeparam>
    /// <typeparam name="CI">Create Input</typeparam>
    /// <typeparam name="EI">Edit Input</typeparam>
    public class XGenericController<T, CI, EI> : BaseController
        where T : DelEntity, new()
        where CI : BaseInput, new()
        where EI : BaseInput, new()
    {
        protected readonly IRepo<T> repo;
        protected readonly IMapper mapper;

        public XGenericController(IRepo<T> repo, IMapper mapper)
        {
            this.repo = repo;
            this.mapper = mapper;
        }

        public string GetCode()
        {
            return Activator.CreateInstance<T>().AuthorityCode;
        }
        public void CheckIsAuthorized(AuthorizeMethod method)
        {
            switch (method)
            {
                case AuthorizeMethod.Insert:
                    WebUserManager.CheckIsAuthorized($"{GetCode()}.I");
                    break;
                case AuthorizeMethod.Delete:
                    WebUserManager.CheckIsAuthorized($"{GetCode()}.D");
                    break;
                case AuthorizeMethod.Select:
                    WebUserManager.CheckIsAuthorized($"{GetCode()}.S");
                    break;
                case AuthorizeMethod.Update:
                    WebUserManager.CheckIsAuthorized($"{GetCode()}.U");
                    break;
                default:
                    break;
            }
        }

        public virtual ActionResult Index()
        {
            CheckIsAuthorized(AuthorizeMethod.Select);
            return PartialView();
        }

        protected virtual string EditView
        {
            get { return "Create"; }
        }

        protected virtual object MapEntityToGridModel(T entity)
        {
            return entity;
        }

        public virtual object MakeFooter(GroupInfo<T> info)
        {
            return null;
        }

        public virtual ActionResult Create()
        {
            try
            {
                CheckIsAuthorized(AuthorizeMethod.Select);
                return PartialView(mapper.Map<T, CI>(new T()));
            }
            catch (UnauthorizedAccessException)
            {
                return PartialView("_NoAccess");
            }
            catch (Exception ex)
            {
                return PartialView("_Error", ex.Message);
            }
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

        [HttpPost]
        public virtual ActionResult Create(CI input)
        {
            try
            {
                CheckIsAuthorized(AuthorizeMethod.Insert);
                if (!ModelState.IsValid)
                    return PartialView(input);

                var entity = repo.Insert(mapper.Map<CI, T>(input));
                repo.Save();
                return Json(MapEntityToGridModel(repo.Get(entity.Id)));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return PartialView(input);
            }
        }

        public virtual ActionResult Edit(int id)
        {
            try
            {
                CheckIsAuthorized(AuthorizeMethod.Select);
                var entity = repo.Get(id);
                return PartialView(EditView, mapper.Map<T, EI>(entity));
            }
            catch (UnauthorizedAccessException)
            {
                return PartialView("_NoAccess");
            }
            catch (Exception ex)
            {
                return PartialView("_Error", ex.Message);
            }
        }

        [HttpPost]
        public virtual ActionResult Edit(EI input)
        {
            try
            {
                CheckIsAuthorized(AuthorizeMethod.Update);
                if (!ModelState.IsValid)
                    return PartialView(EditView, input);

                var entity = mapper.Map<EI, T>(input, repo.Get(input.Id));
                repo.Save();
                return Json(MapEntityToGridModel(repo.Get(entity.Id)));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return PartialView("Create", input);
            }
        }

        [HttpPost]
        public virtual ActionResult Restore(int id)
        {
            repo.Restore(repo.Get(id));
            var item = repo.Get(id);
            return Json(new { Id = id, Content = this.RenderPartialView(string.Empty, new[] { item }), Type = typeof(T).Name.ToLower() });
        }

        public virtual ActionResult Delete(int id, string gridId)
        {
            return PartialView("ConfirmDelete", new DeleteConfirmInput
            {
                Id = id,
                Message = "Kayıt Silinecek !",
                GridId = gridId
            });
        }

        [HttpPost]
        public virtual ActionResult Delete(DeleteConfirmInput input)
        {
            CheckIsAuthorized(AuthorizeMethod.Delete);
            var entity = repo.Get(input.Id);
            repo.Delete(entity);
            repo.Save();
            return Json(new { input.Id });
        }
    }
}