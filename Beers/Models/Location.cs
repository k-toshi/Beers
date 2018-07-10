using System;
using Xamarin.Forms;

namespace Beers.Models
{
	public static class LocationType
	{
       public enum ActivityType
		{
			Walk,
            Run,
            Car
		}
	}

    public class Location
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
		public double Speed { get; set; }

        public Location()
        {
			this.Latitude = 35.693918;
			this.Longitude = 139.710088;
			this.Speed = 0.0;
        }

		public Location(double latitude,double longitude)
        {
			this.Latitude = latitude;
			this.Longitude = longitude;
            this.Speed = 0.0;
        }
        
		public LocationType.ActivityType GetActivityType()
		{
			if (this.Speed <= 6) return LocationType.ActivityType.Walk;
			else if (this.Speed <= 20) return LocationType.ActivityType.Run;
			else return LocationType.ActivityType.Car;
		}
  
    }
}
