namespace Web.UI.Areas.STK
{
    public class StockAlertViewModel
    {
        public string Barcode { get; set; }
        public string Name { get; set; }
        public string CategoryName { get; set; }
        public int Count { get; set; }
        public double PriceBuy { get; set; }
    }
}