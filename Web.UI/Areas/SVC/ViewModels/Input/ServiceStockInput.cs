using Web.UI.ViewModels;

namespace Web.UI.Areas.SVC
{
    public class ServiceStockInput : BaseInput
    {
        public int StockId { get; set; }
        public int ServiceId { get; set; }
        public int? PrinterId { get; set; }
        public int? StockMovementId { get; set; }
        public int Count { get; set; }

        public bool IsDelivered { get; set; }
        public int Status { get; set; }
        public string StatusName { get; set; }
        public string StockName { get; set; }
        public int IsDeleted { get; set; }
    }
}