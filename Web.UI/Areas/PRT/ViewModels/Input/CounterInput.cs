using System;
using Web.UI.ViewModels;

namespace Web.UI.Areas.PRT
{
    public class CounterInput : BaseInput
    {
        public int PrinterId { get; set; }
        public Guid RowId { get; set; }
        public int? Mono { get; set; }
        public int? Color { get; set; }
        public int? Cyan { get; set; }
        public int? Magenta { get; set; }
        public int? Yellow { get; set; }
        public int? Black { get; set; }
    }
}