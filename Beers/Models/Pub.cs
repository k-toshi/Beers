using System;
namespace Beers.Models
{
    public class Pub
    {
		public int Id { get; set; }
        public string Name { get; set; }
        public string HpUrl { get; set; }
        public string ImageUrl { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string Place { get; set; }
        public string AppType { get; set; }
    }
}
