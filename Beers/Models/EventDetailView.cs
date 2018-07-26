using System;
namespace Beers.Models
{
    public class EventDetailView
    {
		public int Id { get; set; }
        public string Name { get; set; }
        public string EventImageUrl { get; set; }
        public string Detail { get; set; }
        public string Place { get; set; }
        public string Schedule { get; set; }
        public string AimTotalPrize { get; set; }
        public int PubId { get; set; }
        public string PubName { get; set; }
        public string HpUrl { get; set; }
        public string PubPlace { get; set; }
        public string PubImageUrl { get; set; }
    }
}
