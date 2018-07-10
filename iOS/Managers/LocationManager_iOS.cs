using System;
using CoreLocation;
using UIKit;
using Foundation;
using Beers.Models;

namespace Beers.iOS.Managers
{
    public class LocationManager_iOS
    {
        protected CLLocationManager LocMgr { get; set; }
        // event for the location changing
        public event EventHandler<LocationUpdatedEventArgs> LocationUpdated = delegate { };
        public Location CurrentLocation { get; set; }

        public LocationManager_iOS()
        {

            this.LocMgr = new CLLocationManager();
            this.CurrentLocation = new Location();

            this.LocMgr.PausesLocationUpdatesAutomatically = false;

            // iOS 8 has additional permissions requirements
            if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
            {
                this.LocMgr.RequestAlwaysAuthorization(); // works in background
                                                     //locMgr.RequestWhenInUseAuthorization (); // only in foreground
            }

            if (UIDevice.CurrentDevice.CheckSystemVersion(9, 0))
            {
                this.LocMgr.AllowsBackgroundLocationUpdates = true;
            }
            LocationUpdated += UpdateLocation;

        }

        public void StartLocationUpdates()
        {
            // We need the user's permission for our app to use the GPS in iOS. This is done either by the user accepting
            // the popover when the app is first launched, or by changing the permissions for the app in Settings

            if (CLLocationManager.LocationServicesEnabled)
            {

                this.LocMgr.DesiredAccuracy = CLLocation.AccurracyBestForNavigation; // sets the accuracy that we want in meters
                this.LocMgr.ActivityType = CLActivityType.Fitness;
                this.LocMgr.DistanceFilter = 1.0;

                // Location updates are handled differently pre-iOS 6. If we want to support older versions of iOS,
                // we want to do perform this check and let our LocationManager know how to handle location updates.

                if (UIDevice.CurrentDevice.CheckSystemVersion(6, 0))
                {

                    this.LocMgr.LocationsUpdated += (object sender, CLLocationsUpdatedEventArgs e) => {
                        // fire our custom Location Updated event
                        this.LocationUpdated(this, new LocationUpdatedEventArgs(e.Locations[e.Locations.Length - 1]));
                    };

                }
                else
                {

                    // this won't be called on iOS 6 (deprecated). We will get a warning here when we build.
                    this.LocMgr.UpdatedLocation += (object sender, CLLocationUpdatedEventArgs e) => {
                        this.LocationUpdated(this, new LocationUpdatedEventArgs(e.NewLocation));
                    };
                }

                // Start our location updates
                this.LocMgr.StartUpdatingLocation();

                // Get some output from our manager in case of failure
                this.LocMgr.Failed += (object sender, NSErrorEventArgs e) => {
                    Console.WriteLine(e.Error);
                };

            }
            else
            {

                //Let the user know that they need to enable LocationServices
                Console.WriteLine("Location services not enabled, please enable this in your Settings");

            }
        }

        public void StopLocationUpdates()
        {
            this.LocMgr.StopUpdatingLocation();
        }

        public double GetDistance(Location target)
        {
            CLLocation currentLoc = new CLLocation(this.CurrentLocation.Latitude, this.CurrentLocation.Longitude);
            CLLocation targetLoc = new CLLocation(target.Latitude, target.Longitude);

            return currentLoc.DistanceFrom(targetLoc);
        }

        //This will keep going in the background and the foreground
        public void UpdateLocation(object sender, LocationUpdatedEventArgs e)
        {
            CLLocation location = e.Location;
            this.CurrentLocation.Longitude = location.Coordinate.Longitude;
            this.CurrentLocation.Latitude = location.Coordinate.Latitude;
			this.CurrentLocation.Speed = location.Speed * 3.600;
        }
    }
}
    