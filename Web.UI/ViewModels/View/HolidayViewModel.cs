using Newtonsoft.Json;
using System;

namespace Web.UI.ViewModel.View
{
    public class HolidayViewModel
    {
        [JsonProperty("holidayName")]
        public string HolidayName { get; set; }
        [JsonProperty("start")]
        public DateTime Start { get; set; }
        [JsonProperty("end")]
        public DateTime End { get; set; }
    }
}