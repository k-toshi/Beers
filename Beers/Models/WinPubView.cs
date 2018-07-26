using System;
namespace Beers.Models
{
    public class WinPubView
    {
		public int Id { get; set; }
        public int EventId { get; set; }
        public int WinPubId { get; set; }
        public string WinPubName { get; set; }
        public int UnitPrize { get; set; }
        public int Rank { get; set; }
    }
}
