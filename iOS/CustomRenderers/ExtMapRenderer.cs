using System;

using Xamarin.Forms;
using Beers.Controls;

using CoreLocation;
using UIKit;
using MapKit;
using Foundation;

using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.iOS;
using Xamarin.Forms.Platform.iOS;

using Beers.iOS.CustomRenderers;

[assembly: ExportRenderer(typeof(ExtMap), typeof(ExtMapRenderer))]
namespace Beers.iOS.CustomRenderers
{
	public class ExtMapRenderer : MapRenderer
    {
		private readonly UITapGestureRecognizer _tapRecogniser;

        public ExtMapRenderer()
        {
            _tapRecogniser = new UITapGestureRecognizer(OnTap)
            {
                NumberOfTapsRequired = 1,
                NumberOfTouchesRequired = 1
            };
        }

        private void OnTap(UITapGestureRecognizer recognizer)
        {
            var cgPoint = recognizer.LocationInView(Control);

            var location = ((MKMapView)Control).ConvertPoint(cgPoint, Control);

            ((ExtMap)Element).OnTap(new Position(location.Latitude, location.Longitude));
        }

        protected override void OnElementChanged(ElementChangedEventArgs<View> e)
        {
            if (Control != null)
                Control.RemoveGestureRecognizer(_tapRecogniser);

            base.OnElementChanged(e);

            if (Control != null)
                Control.AddGestureRecognizer(_tapRecogniser);
        }
    }
}
