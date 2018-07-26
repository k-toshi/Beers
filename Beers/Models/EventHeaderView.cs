using System;
namespace Beers.Models
{
    public class EventHeaderView
    {
		public int Id { get; set; }
        public string Name { get; set; }
		private Uri eventImageUrl { get; set; }
		public Uri EventImageUrl
        {
			get { return eventImageUrl; }
			set { eventImageUrl = Util.GetUriFromImageUrl(value); }
        }
        public string Detail { get; set; }
        public string Place { get; set; }
        public string Schedule { get; set; }
        public string AimTotalPrize { get; set; }
    }
}
