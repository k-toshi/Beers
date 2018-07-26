using System;

using Android.App;
using Android.Util;
using Android.Content;
using Android.OS;
using Android.Locations;

using Xamarin.Forms;
using Beers.Controls;

using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.Android;
using Xamarin.Forms.Platform.Android;

using Android.Gms.Maps;
using Android.Gms.Maps.Model;

using Xamarin.Forms.Internals;

using Beers.Droid.CustomRenderers;

[assembly: ExportRenderer(typeof(ExtMap), typeof(ExtMapRenderer))]
namespace Beers.Droid.CustomRenderers
{
	public class ExtMapRenderer : MapRenderer, IOnMapReadyCallback
    {
		public ExtMapRenderer(Context context)
		{
		}
        // We use a native google map for Android
        private GoogleMap _map;

        public void OnMapReady(GoogleMap googleMap)
        {
            _map = googleMap;

            if (_map != null)
                _map.MapClick += googleMap_MapClick;
        }
        
        protected void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.View> e)
        {
            if (_map != null)
                _map.MapClick -= googleMap_MapClick;

            //OnElementChanged(e);

            if (Control != null)
                ((MapView)Control).GetMapAsync(this);
        }

        private void googleMap_MapClick(object sender, GoogleMap.MapClickEventArgs e)
        {
            ((ExtMap)Element).OnTap(new Position(e.Point.Latitude, e.Point.Longitude));
        }
    }

}
