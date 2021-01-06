using Core;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.UI.Controllers;
using Web.UI.Mappers;

namespace Web.UI.Areas.STK.Controllers
{
    public class StockAlertController : ROGenericController<Stock>
    {
        public StockAlertController(IRepo<Stock> repo) : base(repo)
        {

        }
        public ActionResult StockAlert()
        {
            var stockAlert = repo.Where(s => s.Count <= /*s.CountAlert*/9999).OrderByDescending(s => s.Count).ToList()
                .Select(s => new StockAlertViewModel
                {
                    Barcode = s.Barcode,
                    CategoryName = s.StockCategory.Name,
                    Count = s.Count,
                    Name = s.Name,
                    PriceBuy = s.PriceBuy
                });
            return PartialView("StockAlert", stockAlert);
        }
    }
}