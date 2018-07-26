using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

using Beers.Controls;
using Beers.Controllers;
using Beers.Models;

using Xamarin.Forms.Maps;

using Xamarin.Forms;

using Plugin.Geolocator;

namespace Beers.Views
{
    public partial class SearchPubsOnMapPage : ContentPage
    {
		private double BaseLatitude { get; set; } 
		private double BaseLongitude { get; set; }
		private double TargetDistance { get; set; }
		private DateTime TargetDateTime { get; set; }
		private ExtMap map { get; set; }

		public SearchPubsOnMapPage()
        {
            InitializeComponent();

			BaseLatitude = -1;
			BaseLongitude = -1;         
            TargetDistance = 10000;
            TargetDateTime = DateTime.Now;

            InitMap();
        }

		public SearchPubsOnMapPage(double distance, DateTime targetDateTime)
        {
            InitializeComponent();

			BaseLatitude = -1;
            BaseLongitude = -1;           
            TargetDistance = distance;
            TargetDateTime = targetDateTime;
           
            InitMap();
        }

        public SearchPubsOnMapPage(double baselatitude, double baselongitude, double distance,DateTime targetDateTime)
        {
            InitializeComponent();

			BaseLatitude = baselatitude;
			BaseLongitude = baselongitude;
			TargetDistance = distance;
			TargetDateTime = targetDateTime;

			InitMap();
        }

		private async Task InitMap()
		{
			map = new ExtMap()
			{
				IsShowingUser = true,
				HeightRequest = 100,
				WidthRequest = 960,
				VerticalOptions = LayoutOptions.FillAndExpand
			};

			if (BaseLatitude == -1 && BaseLongitude == -1) await GetCurrentPosition();

			map.MoveToRegion(
				MapSpan.FromCenterAndRadius(
						new Position(BaseLatitude, BaseLongitude), Distance.FromMiles(0.3)));

            var stack = new StackLayout { Spacing = 0 };
            stack.Children.Add(map);
            Content = stack;

			map.Tapped += async (sender, e) =>  {
                await OnMapTap(e);
            };

			await SetPinInTargetScope(BaseLatitude, BaseLongitude, TargetDistance, TargetDateTime);
		}
      
		private async Task GetCurrentPosition()
        {
            Plugin.Geolocator.Abstractions.Position position = null;
			var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 50;

			if(IsLocationAvailable() && locator.IsGeolocationEnabled)
			{
				position = await locator.GetPositionAsync(TimeSpan.FromSeconds(10), null, false);

                BaseLatitude = position.Latitude;
                BaseLongitude = position.Longitude;
			}
            
        }

		private bool IsLocationAvailable()
        {
            if (!CrossGeolocator.IsSupported)
                return false;

            return CrossGeolocator.Current.IsGeolocationAvailable;
        }

		protected async Task OnMapTap(MapTapEventArgs e)
		{
			await SetPinInTargetScope(e.Position.Latitude, e.Position.Longitude, TargetDistance, TargetDateTime);
		}
        
        /// <summary>
        /// Sets the pin in target scope.
        /// </summary>
        /// <param name="baselatitude">Baselatitude.</param>
        /// <param name="baselongitude">Baselongitude.</param>
        /// <param name="distance">Distance.</param>
		private async Task SetPinInTargetScope(double baselatitude, double baselongitude, double distance,DateTime targetDateTime)
		{
			ObservableCollection<Pub> pubs = await PubController.GetPubsInTargetScope(baselatitude, baselongitude, distance,targetDateTime);

            foreach(Pub pub in pubs)
			{
				var position = new Position(pub.Latitude, pub.Longitude); // Latitude, Longitude
                var pin = new Pin
                {
                    Type = PinType.Place,
                    Position = position,
                    Label = pub.Name,
                    Address = pub.Place
                };
				pin.Clicked += (e, sender) =>
				{
					Device.OpenUri(new Uri(pub.HpUrl));
				};
                map.Pins.Add(pin);
			}         
		}
    }
}
