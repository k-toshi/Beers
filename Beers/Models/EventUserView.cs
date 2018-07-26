using System;
namespace Beers.Models
{
    public class EventUserView
    {
		public int Id { get; set; }
        public int EventId { get; set; }
        public int PubId { get; set; }
        public string UserId { get; set; }
        public string EventName { get; set; }
        public string PubName { get; set; }
    }
}
