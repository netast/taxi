using System;
using Android.App;
using Android.Content;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using taxi.Droid;
using Xamarin.Forms;

[assembly: Dependency(typeof(LocationTracker))]
namespace taxi.Droid
{

	public class LocationTracker : Java.Lang.Object, ILocationTracker, ILocationListener
	{
		LocationManager locationManager;
		public LocationTracker()
		{
			Activity activity = Toolkit.Activity;
			if (activity == null)
				throw new InvalidOperationException(
					"Must call Toolkit.Init before using LocationProvider"); locationManager =
				activity.GetSystemService(Context.LocationService) as LocationManager;

			locationManager = (LocationManager) activity.GetSystemService(Context.LocationService);
		}

		#region ILocationTracker
		public event EventHandler<GeographicLocation> LocationChanged;

		public void PauseTracking()
		{
			if(locationManager != null)
			locationManager.RemoveUpdates(this);
			
		}

		public void StartTracking()
		{
			var locationProviders = locationManager.AllProviders;
			foreach (string locationProvider in locationProviders) {
				locationManager.RequestLocationUpdates(locationProvider, 1000, 1, this); 
			}
		}

		#endregion

		#region ILocationListeners

		public void OnLocationChanged(Location location)
		{
			var handler = LocationChanged;
			if (handler != null)
			{
				handler(this, new GeographicLocation(location.Latitude, location.Longitude));
			}
		}

		public void OnProviderDisabled(string provider)
		{
			
		}

		public void OnProviderEnabled(string provider)
		{
			
		}

		public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras)
		{
			
		}
		#endregion

	}
}
