using System.Collections.Generic;

namespace Web.UI.Areas.STK
{
    public class StockUserInventoryViewModel
    {
        public int AppUserId { get; set; }
        public string FullName { get; set; }
        public List<ProductInput> Products { get; set; } = new List<ProductInput>();
        public int LocationId { get; set; }
        public int IsExit { get; set; }
    }
}