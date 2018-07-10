using System;
using Android.OS;

namespace Beers.Droid.Managers
{
	public class LocationServiceBinder : Binder
    {
		public LocationManager_Droid Service
        {
            get { return this.service; }
        }
		protected LocationManager_Droid service;

        public bool IsBound { get; set; }

        // constructor
		public LocationServiceBinder(LocationManager_Droid service)
        {
            this.service = service;
        }
    }
}
