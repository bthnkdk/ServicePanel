using System;
using System.IO;
using System.Web.Mvc;

namespace Web.UI.Helper
{
    public class PageSideSet : IDisposable
    {
        bool _disposed;
        readonly ViewContext _viewContext;
        readonly TextWriter _writer;

        public PageSideSet(ViewContext viewContext)
        {
            if (viewContext == null)
            {
                throw new ArgumentNullException("viewContext");
            }
            _viewContext = viewContext;
            _writer = viewContext.Writer;
        }

        public void Dispose()
        {
            Dispose(true /* disposing */);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                _disposed = true;
                _writer.Write("</ul></div></div>");
            }
        }

        public void EndDiv()
        {
            Dispose(true);
        }
    }

    public class PageSet : IDisposable
    {
        bool _disposed;
        readonly ViewContext _viewContext;
        readonly TextWriter _writer;

        public PageSet(ViewContext viewContext)
        {
            if (viewContext == null)
            {
                throw new ArgumentNullException("viewContext");
            }
            _viewContext = viewContext;
            _writer = viewContext.Writer;
        }

        public void Dispose()
        {
            Dispose(true /* disposing */);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                _disposed = true;
                _writer.Write("</div>");
            }
        }

        public void EndDiv()
        {
            Dispose(true);
        }
    }

    public class PageHeadSet : IDisposable
    {
        bool _disposed;
        readonly ViewContext _viewContext;
        readonly TextWriter _writer;

        public PageHeadSet(ViewContext viewContext)
        {
            if (viewContext == null)
            {
                throw new ArgumentNullException("viewContext");
            }
            _viewContext = viewContext;
            _writer = viewContext.Writer;
        }

        public void Dispose()
        {
            Dispose(true /* disposing */);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                _disposed = true;
                _writer.Write("</ul></div ></div ></div>");
            }
        }

        public void EndDiv()
        {
            Dispose(true);
        }
    }

    public static class ControlHelper
    {
        public static PageSet Page(this HtmlHelper htmlHelper)
        {
            htmlHelper.ViewContext.Writer.WriteLine(("<div class='content-wrapper'>"));
            return new PageSet(htmlHelper.ViewContext);
        }
        public static PageSet PageBody(this HtmlHelper htmlHelper)
        {
            htmlHelper.ViewContext.Writer.WriteLine("<div class='content-body'>");
            return new PageSet(htmlHelper.ViewContext);
        }
        public static PageSet Card(this HtmlHelper htmlHelper, string cardHeaderText, string cardBodyId, string cardId = "")
        {
            htmlHelper.ViewContext.Writer.WriteLine($"<div class='card' id='{cardId}'><div class='card-header'><h4 class='card-title' id='horz-layout-basic'>{cardHeaderText}</h4><a class='heading-elements-toggle'><i class='fa fa-ellipsis-v font-medium-3'></i></a><div class='heading-elements'><ul class='list-inline mb-0'><li><a data-action='collapse'><i class='ft-minus'></i></a></li><li><a data-action='expand'><i class='ft-maximize'></i></a></li></ul></div></div><div class='card-content collpase show'><div class='card-body' id='{cardBodyId}'>");
            return new PageSet(htmlHelper.ViewContext);
        }
        public static PageHeadSet PageHead(this HtmlHelper htmlHelper, string title)
        {
            //<div class='content-header-left col-md-6 col-12 mb-1'><h2 class='content-header-title'>" + title + "</h2></div> TITLE
            htmlHelper.ViewContext.Writer.WriteLine("<div class='content-header row' style='padding:0'><div class='content-header-left breadcrumbs-left breadcrumbs-top col-md-6 col-12'><div class='breadcrumb-wrapper col-12'><ol class='breadcrumb'><li class='breadcrumb-item'><a href='/#/Home/Main'><i class='ft-home' style='font-size:18px'></i></a></li>");
            return new PageHeadSet(htmlHelper.ViewContext);
        }
        public static PageHeadSet GridActionBar(this HtmlHelper htmlHelper)
        {
            htmlHelper.ViewContext.Writer.WriteLine("<div class='btn-toolbar justify-content-between bar' role='toolbar' style='padding-top:5px;padding-bottom:5px;padding-right: 5px;'>");
            return new PageHeadSet(htmlHelper.ViewContext);
        }
        //public static MvcHtmlString ActionButton(this HtmlHelper html,string class ="", string text = "", string url = "#")
        //{
        //    return MvcHtmlString.Create($"<li class=\"breadcrumb-item {bold}\"><a href =\"#\">{text}</a></li>");
        //}
        public static MvcHtmlString BreadCrumb(this HtmlHelper html, string text = "", bool isActive = false, string url = "#", bool isLink = true)
        {
            string bold = isActive ? "active" : "";
            if (!isLink)
                return MvcHtmlString.Create($"<li class=\"breadcrumb-item {bold}\">{text}</li>");
            if (url == "#")
                return MvcHtmlString.Create($"<li class=\"breadcrumb-item {bold}\"><a href =\"#\">{text}</a></li>");
            else
                return MvcHtmlString.Create($"<li class=\"breadcrumb-item {bold}\"><a href =\"{url}\">{text}</a></li>");
        }
        public static MvcHtmlString BreadCrumb(this HtmlHelper html, string text = "")
        {
            return MvcHtmlString.Create($"<li class=\"breadcrumb-item \">{text}</li>");
        }
        public static MvcHtmlString ValidateSummaryEagle(this HtmlHelper html, string text = "")
        {
            if (html.ViewData.ModelState.IsValid)
                return null;

            string errorsHtml = "<ul>";
            foreach (var value in html.ViewData.ModelState.Values)
            {
                foreach (var error in value.Errors)
                {
                    errorsHtml += "<li>" + error.ErrorMessage + "</li>";
                }
            }
            errorsHtml += "</ul>";
            return MvcHtmlString.Create($"<div id='validationDiv' class='alert alert-danger alert-dismissible mb-2' role='alert'><button type='button' class='close' data-dismiss='alert' aria-label='Kapat'><span aria-hidden='true'>×</span></button><h4 class='alert-heading mb-2'>Lütfen zorunlu alanları kontrol edin</h4> { errorsHtml}</div");
        }
    }
}