using System;
using CoreLocation;
using taxi.iOS;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(LocationTracker))]
namespace taxi.iOS
{
	public class LocationTracker : ILocationTracker
	{
		CLLocationManager locationManager;

		public LocationTracker()
		{
			locationManager = new CLLocationManager();
			// Request authorization for iOS 8 and above.
			if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
			{
				locationManager.RequestWhenInUseAuthorization();
			}
			locationManager.LocationsUpdated +=
				    (object sender, CLLocationsUpdatedEventArgs args) =>
			{

				CLLocationCoordinate2D coordinate = args.Locations[0].Coordinate;
				EventHandler<GeographicLocation> handler = LocationChanged;
				if (handler != null)
				{
					handler(this, new GeographicLocation(coordinate.Latitude,
					                 coordinate.Longitude));
				}
			};
		}

		public event EventHandler<GeographicLocation> LocationChanged;

		public void PauseTracking()
		{
			locationManager.StopUpdatingLocation();
		}

		public void StartTracking()
		{
			locationManager.StartUpdatingLocation();
		}
	}
}
