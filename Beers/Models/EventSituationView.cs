using System;
namespace Beers.Models
{
    public class EventSituationView
    {
		public int EventId { get; set; }
        public string EventName { get; set; }
        public int TotalPrize { get; set; }
        public string EndDateTime { get; set; }
        public int EventUnitPrice { get; set; }
        public int MaxUnitPrize { get; set; }
    }
}
