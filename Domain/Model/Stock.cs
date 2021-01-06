using System;
using System.Collections.Generic;

namespace Domain
{
    public class Stock : DelEntity
    {
        public override string AuthorityCode => "STK";
        public Stock()
        {
            StockMovements = new HashSet<StockMovement>();
            StockUserInventories = new HashSet<StockUserInventory>();
        }

        public Guid RowId { get; set; }
        public string Name { get; set; }
        public int StockCategoryId { get; set; }
        public int CurrencyId { get; set; }
        public string Barcode { get; set; }
        public string OptionalBarcode { get; set; }
        public int Count { get; set; }
        public int CountAlert { get; set; }
        public string Type { get; set; }
        public string Address { get; set; }
        public double PriceBuy { get; set; }
        public double PriceSell { get; set; }
        public double PriceBuyKDV { get; set; }
        public double PriceSellKDV { get; set; }

        public virtual Currency Currency { get; set; }
        public virtual StockCategory StockCategory { get; set; }
        public virtual ICollection<StockMovement> StockMovements { get; set; }
        public virtual ICollection<StockUserInventory> StockUserInventories { get; set; }
    }
    public class StockCategory : DelEntity
    {
        public override string AuthorityCode => "STK";
        public string Name { get; set; }
        public int StockMainCategoryId { get; set; }

        public virtual StockMainCategory StockMainCategory { get; set; }
    }
    public class StockMainCategory : DelEntity
    {
        public override string AuthorityCode => "STK";
        public string Name { get; set; }
    }
    public class StockMovement : Entity
    {
        public override string AuthorityCode => "STK";

        public int StockId { get; set; }
        public int LocationId { get; set; }
        public int Count { get; set; }
        public int AppUserId { get; set; }
        public DateTime Date { get; set; }
        public double Price { get; set; }
        public int Action { get; set; }
        public int CreateAppUserId { get; set; }
       
        public virtual Stock Stock { get; set; }
        public virtual Location Location { get; set; }
        public virtual AppUser AppUser { get; set; }
        public virtual AppUser CreateAppUser { get; set; }
    }
    public class StockUserInventory : Entity
    {
        public override string AuthorityCode => "STK";

        public int AppUserId { get; set; }
        public int StockId { get; set; }
        public int Count { get; set; }
        public virtual AppUser AppUser { get; set; }
        public virtual Stock Stock { get; set; }
    }
}
