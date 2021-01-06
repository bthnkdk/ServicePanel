using Core;
using Omu.AwesomeMvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using Web.UI.Helper;
using Web.UI.Mappers;
using Web.UI.ViewModels;
using WebMarkupMin.AspNet4.Mvc;

namespace Web.UI.Controllers
{
    [MinifyHtml]
    public class FileController : GenericController<Domain.File, FileInput, FileInput>
    {
        public FileController(IRepo<Domain.File> repo, IMapper mapper) : base(repo, mapper)
        {
        }

        protected override object MapEntityToGridModel(Domain.File model)
        {
            return new { model.FileName, model.CreatedDate, AppUserName = model.CreatedAppUser.FullName, FileTypeName = model.FileType.Name };
        }

        public FileResult Download(int id)
        {
            var entry = repo.Get(id);
            string fullPath = string.Format(
                    "{0}/{1}/{2}{3}",
                    Server.MapPath("~/App_Data/Media/Files/"),
                    entry.RowId,
                    entry.StoreId,
                    entry.Extension);
            return File(fullPath, entry.MimeType, string.Concat(entry.FileName, entry.Extension));
        }
        public ActionResult Save(IEnumerable<HttpPostedFileBase> ufiles, string parameterId)
        {
            Guid? rowId = null;
            try
            {
                rowId = Guid.Parse(parameterId);
                foreach (var ufile in ufiles)
                {
                    var file = new Domain.File();
                    file.RowId = rowId.Value;
                    file.StoreId = Guid.NewGuid();
                    file.FileName = Path.GetFileNameWithoutExtension(ufile.FileName);
                    file.Extension = Path.GetExtension(ufile.FileName);
                    file.ContentLength = ufile.ContentLength;
                    file.MimeType = ufile.ContentType;
                    file.CreatedDate = DateTime.Now;
                    file.CreatedUserId = WebUserManager.GetUserInfo().Id;

                    var fullPath = string.Format(@"{0}/{1}", Server.MapPath("~/App_Data/Media/Files"), rowId);

                    if (!Directory.Exists(fullPath))
                        Directory.CreateDirectory(fullPath);

                    var path = string.Format("{0}/{1}{2}", fullPath, file.StoreId, file.Extension);
                    ufile.SaveAs(path);
                    FileHelper.CompressImage(path);

                    repo.Insert(file);
                    break;
                }
                repo.Save();
                return Json("", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                if (rowId.HasValue)
                {
                    var fullPath = string.Format(@"{0}/{1}", Server.MapPath("~/App_Data/Media/Files"), rowId);
                    foreach (var ufile in ufiles)
                    {
                        if (Directory.Exists(fullPath))
                            Directory.Delete(fullPath);
                    }
                }
                return Json(ex.GetMessage(), JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult GridGetItems(GridParams g, string parent)
        {
            parent = (parent ?? "").ToLower();
            var data = repo.Where(o => o.FileName.ToLower().Contains(parent));
            var model = GetGridModelDto(g, data);
            return Json(model);
        }
    }
}