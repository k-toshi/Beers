using System;
namespace Beers.Models
{
    public class EventUserCountView
    {
		public int EventId { get; set; }
        public int PubId { get; set; }
        public string PubName { get; set; }
        public int UserCount { get; set; }
        public string ImageUrl { get; set; }
    }
}
