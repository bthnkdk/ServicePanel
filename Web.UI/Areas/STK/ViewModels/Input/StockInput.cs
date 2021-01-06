using Omu.AwesomeMvc;
using System;
using System.ComponentModel.DataAnnotations;
using Web.UI.ViewModels;

namespace Web.UI.Areas.STK
{
    public class StockInput : BaseInput
    {
        public Guid RowId { get; set; }

        [Required(ErrorMessage = "{0} gerekli")]
        [MaxLength(30, ErrorMessage = "Max {1}")]
        [UIHint("String")]
        [Display(Name = "Stok Adı")]
        public string Name { get; set; }

        [Required(ErrorMessage = "{0} gerekli")]
        [Display(Name = "Kategori Tipi")]
        [UIHint("Odropdown")]
        [AweUrl(Controller = "Data", Action = "GetStockCategories")]
        public int StockCategoryId { get; set; }

        [Required(ErrorMessage = "{0} gerekli")]
        [MaxLength(75, ErrorMessage = "Max {1}")]
        [UIHint("String")]
        [Display(Name = "Barkod")]
        public string Barcode { get; set; }

        [MaxLength(75, ErrorMessage = "Max {1}")]
        [UIHint("String")]
        [Display(Name = "Opsiyonel Barkod")]
        public string OptionalBarcode { get; set; }

        [Required(ErrorMessage = "{0} gerekli")]
        [Display(Name = "Adet")]
        public int Count { get; set; }

        [Required(ErrorMessage = "{0} gerekli")]
        [Display(Name = "Adet Alarm")]
        public int CountAlert { get; set; }

        [Required(ErrorMessage = "{0} gerekli")]
        [MaxLength(20, ErrorMessage = "Max {1}")]
        [UIHint("String")]
        [Display(Name = "Tip")]
        public string Type { get; set; }

        [Required(ErrorMessage = "{0} gerekli")]
        [MaxLength(100, ErrorMessage = "Max {1}")]
        [UIHint("String")]
        [Display(Name = "Adres")]
        public string Address { get; set; }

        [Required(ErrorMessage = "{0} gerekli")]
        [UIHint("String")]
        [Display(Name = "Alış Fiyatı")]
        public decimal PriceBuy { get; set; }

        [Required(ErrorMessage = "{0} gerekli")]
        [UIHint("String")]
        [Display(Name = "Satış Fiyatı")]
        public decimal PriceSell { get; set; }

        [Required(ErrorMessage = "{0} gerekli")]
        [UIHint("String")]
        [Display(Name = "KDV'li Alış Fiyatı")]
        public decimal PriceBuyKDV { get; set; }

        [Required(ErrorMessage = "{0} gerekli")]
        [UIHint("String")]
        [Display(Name = "KDV'li Satış Fiyatı")]
        public decimal PriceSellKDV { get; set; }

    }
}