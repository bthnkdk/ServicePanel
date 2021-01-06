using Omu.AwesomeMvc;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Web.UI.Helper
{
    public static class GridHelpers
    {
        private static UrlHelper GetUrlHelper<T>(HtmlHelper<T> html)
        {
            return new UrlHelper(html.ViewContext.RequestContext);
        }

        public static IHtmlString CreatePopup<T>(this HtmlHelper<T> html, string controller, string action, int popupHeight = 430, int popupWidth = 450, string name = "", string onLoad = "", string title = "", bool fullScreen = false,string success="")
        {
            var url = GetUrlHelper(html);

            var result =
                html.Awe().InitPopupForm()
                .Name(name)
                .Height(200)
                .Url(url.Action(action, controller))
                .UseDefaultButtons(true)
                .Fullscreen(fullScreen)
                .OnLoad(onLoad)
                .Modal(true)
                .Title(title)
                .Success(success)
                .OkText("Kaydet")
                .ToString();
            return new MvcHtmlString(result);
        }

        public static IHtmlString InitCrudPopupsForGrid<T>(this HtmlHelper<T> html, string gridId, string crudController, int createPopupHeight = 430, int createPopupWidth = 450, string title = "", bool fullScreen = false, string clas = "", string optionalCrud = "")
        {
            var url = GetUrlHelper(html);

            gridId = html.Awe().GetContextPrefix() + gridId;

            var result =
            html.Awe()
                .InitPopupForm()
                .Name("create" + gridId)
                .PopupClass(clas)
                .Group(gridId)
                .Height(createPopupHeight)
                .Width(createPopupWidth)
                .Fullscreen(fullScreen)
                .Url(url.Action(optionalCrud + "Create", crudController))
                .Success("utils.itemCreated('" + gridId + "')")
                .Modal(true)
                .Title(title)
                .OkText("Kaydet")
                .ToString()

            + html.Awe()
                  .InitPopupForm()
                  .Name("edit" + gridId)
                  .Group(gridId)
                  .Height(createPopupHeight)
                  .Width(createPopupWidth)
                  .Fullscreen(fullScreen)
                  .Url(url.Action(optionalCrud + "Edit", crudController))
                  .Modal(true)
                  .OkText("Güncelle")
                  .Title(title)
                  .Success("utils.itemEdited('" + gridId + "')")

            + html.Awe()
                  .InitPopupForm()
                  .Name("delete" + gridId)
                  .Group(gridId)
                  .Url(url.Action(optionalCrud + "Delete", crudController))
                  .Success("utils.itemDeleted('" + gridId + "')")
                  .OnLoad("utils.delConfirmLoad('" + gridId + "')")
                  .Parameter("gridId", gridId) // used to call grid.api.select and emphasize the row
                  .Height(200)
                  .Modal(true)
                  .OkText("Tamam")
                  .Title(title);

            return new MvcHtmlString(result);
        }

        public static IHtmlString InitCrudPopupsForAjaxList<T>(
            this HtmlHelper<T> html, string ajaxListId, string controller, string popupName, string afterUpdateFunc = null, bool fullScreen = false, int createPopupHeight = 430)
        {

            var editSuccessFunc = afterUpdateFunc == null
                          ? string.Format("utils.itemEditedAl('{0}')", ajaxListId)
                          : string.Format("utils.itemEditedAl('{0}', {1})", ajaxListId, afterUpdateFunc);

            var url = GetUrlHelper(html);

            var result =
                html.Awe()
                    .InitPopupForm()
                    .Name("create" + popupName)
                    .Url(url.Action("Create", controller))
                    .Parameter("usingAjaxList", true)
                    .Height(createPopupHeight)
                    .Fullscreen(fullScreen)
                    .Success("utils.itemCreatedAl('" + ajaxListId + "')")
                    .Group(ajaxListId)
                    .Title("Yeni")
                    .ToString()

                + html.Awe()
                      .InitPopupForm()
                      .Name("edit" + popupName)
                      .Url(url.Action("Edit", controller))
                      .Parameter("usingAjaxList", true)
                      .Height(createPopupHeight)
                      .Fullscreen(fullScreen)
                      .Success(editSuccessFunc)
                      .Group(ajaxListId)
                      .Title("Düzenle")

                + html.Awe()
                      .InitPopupForm()
                      .Name("delete" + popupName)
                      .Url(url.Action("Delete", controller))
                      .Success("utils.itemDeletedAl('" + ajaxListId + "')")
                      .Group(ajaxListId)
                      .OkText("Evet")
                      .CancelText("Hayır")
                      .Height(200)
                      .Modal()
                      .Title("Sil")

                + html.Awe()
                      .InitCall("restore" + popupName)
                      .Url(url.Action("Restore", controller))
                      .Success(editSuccessFunc);

            return new MvcHtmlString(result);
        }

        public static IHtmlString InitDeletePopupForGrid<T>(this HtmlHelper<T> html, string gridId, string crudController, string action = "Delete")
        {
            var url = GetUrlHelper(html);
            gridId = html.Awe().GetContextPrefix() + gridId;

            var result =
                html.Awe()
                  .InitPopupForm()
                  .Name("delete" + gridId)
                  .Group(gridId)
                  .Url(url.Action(action, crudController))
                  .Success("utils.itemDeleted('" + gridId + "')")
                  .OnLoad("utils.delConfirmLoad('" + gridId + "')") // calls grid.api.select and animates the row
                  .Height(200)
                  .Modal()
                  .Title("Sil")
                  .OkText("Tamam")
                  .ToString();

            return new MvcHtmlString(result);
        }
    }

    public static class GridUtils
    {
        private static string GetPopupName<T>(this HtmlHelper<T> html, string action, string gridId)
        {
            return action + html.Awe().GetContextPrefix() + gridId;
        }

        public static string InlineDeleteFormatForGrid<T>(this HtmlHelper<T> html, string gridId, string key = "Id", bool nofocus = false, string text = "İptal")
        {
            var popupName = html.GetPopupName("delete", gridId);

            return DeleteFormat(popupName, key, btnClass: "o-glh", nofocus: nofocus)
                   + html.Awe()
                       .Button()
                       .Text(text)
                       .CssClass("o-glcanb awe-nonselect o-gl o-pad");
        }

        public static IHtmlString InlineCreateButtonForGrid<T>(this HtmlHelper<T> html, string gridId, object parameters = null, string text = "Create")
        {
            gridId = html.Awe().GetContextPrefix() + gridId;
            var parms = new JavaScriptSerializer().Serialize(parameters);

            return html.Awe().Button()
                .Text(text)
                .CssClass("mbtn")
                .OnClick(string.Format("$('#{0}').data('api').inlineCreate({1})", gridId, parms));
        }

        public static IHtmlString CreateButtonForGrid<T>(this HtmlHelper<T> html, string gridId, object parameters = null, string text = "Yeni")
        {
            return html.Awe().Button()
                .Text(text)
                .CssClass("mbtn")
                .OnClick(html.Awe().OpenPopup(html.GetPopupName("create", gridId)).Params(parameters));
        }

        public static string EditFormatForGrid<T>(this HtmlHelper<T> html, string gridId, string key = "Id", bool setId = false, bool nofocus = false, int? height = null)
        {
            var popupName = html.GetPopupName("edit", gridId);

            var click = html.Awe().OpenPopup(popupName).Params(new { id = "." + key });
            if (height.HasValue)
            {
                click.Height(height.Value);
            }

            var button = html.Awe().Button()
                .CssClass("awe-nonselect editbtn")
                //.Text("<span class='ico-crud ico-edit'></span>")
                .Text("<span class='fa fa-edit'>1</span>")
                .OnClick(click);

            var attrdict = new Dictionary<string, object>();

            if (setId)
            {
                attrdict.Add("id", $"gbtn{popupName}.{key}");
            }

            if (nofocus)
            {
                attrdict.Add("tabindex", "-1");
            }

            button.HtmlAttributes(attrdict);

            return button.ToString();
        }

        public static string DeleteFormatForGrid<T>(this HtmlHelper<T> html, string gridId, string key = "Id", bool nofocus = false)
        {
            gridId = html.Awe().GetContextPrefix() + gridId;

            return DeleteFormatForGrid(gridId, key, nofocus);
        }

        public static string EditFormat(string popupName, string key = "Id", bool setId = false, bool nofocus = false)
        {
            var idattr = "";
            if (setId)
            {
                idattr = $"id = 'gbtn{popupName}.{key}'";
            }

            var tabindex = nofocus ? "tabindex = \"-1\"" : string.Empty;

            return string.Format("<button type=\"button\" class=\"awe-btn awe-nonselect editbtn\" {3} {2} onclick=\"awe.open('{0}', {{ params:{{ id: '.{1}' }} }}, event)\"><i class='icon-pencil text-info'/></button>",
                popupName, key, idattr, tabindex);
        }

        public static string GoEditFormat(string url, string key = "Id")
        {

            return string.Format("<button type=\"button\" class=\"awe-btn awe-nonselect editbtn\" onclick=\"document.location.href='{0}.{1}'\"><span class='fa fa-edit text-info'></span></button>",
                url, key);
        }

        public static string DeleteFormat(string popupName, string key = "Id", string deleteContent = null, string btnClass = null, bool nofocus = false)
        {
            if (deleteContent == null)
            {
                deleteContent = "<span class='fa fa-trash text-danger'></span>";
            }

            var tabindex = nofocus ? "tabindex = \"-1\"" : string.Empty;

            return string.Format("<button type=\"button\" class=\"awe-btn awe-nonselect delbtn {3}\" {4} onclick=\"awe.open('{0}', {{ params:{{ id: '.{1}' }} }}, event)\"><i class='icon-trash text-danger'/></button>",
                popupName, key, deleteContent, btnClass, tabindex);
        }

        public static string InlineEditFormat(bool nofocus = false)
        {
            var tabindex = nofocus ? "tabindex = \"-1\"" : string.Empty;
            return string.Format("<button type=\"button\" class=\"awe-btn o-gledtb awe-nonselect o-glh o-pad\" {0} >Düzenle</button><button type=\"button\" class=\"awe-btn o-glsvb awe-nonselect o-gl o-pad\">Kaydet</button>", tabindex);
        }

        public static string EditFormatForGrid(string gridId, string key = "Id", bool setId = false, bool nofocus = false)
        {
            return EditFormat("edit" + gridId, key, setId, nofocus);
        }

        public static string DeleteFormatForGrid(string gridId, string key = "Id", bool nofocus = false)
        {
            return DeleteFormat("delete" + gridId, key, null, null, nofocus);
        }

        public static string EditGridNestFormat()
        {
            return "<button type='button' class='awe-btn editnst'><span class='ico-crud ico-edit'></span></button>";
        }

        public static string DeleteGridNestFormat()
        {
            return "<button type='button' class='awe-btn delnst'><span class='ico-crud ico-del'></span></button>";
        }

        public static string AddChildFormat()
        {
            return "<button type='button' class='awe-btn awe-nonselect o-pad' onclick=\"awe.open('createNode', { params:{ parentId: '.Id' } })\"><span class='fa fa-plus'></span></button>";
        }

        public static string MoveBtn()
        {
            return "<button type=\"button\" class=\"awe-movebtn awe-btn\" tabindex=\"-1\"><i class=\"awe-icon\"></i></button>";
        }
    }
}