using System;
using Xamarin.Forms;
using Prism.Mvvm;
using Xamarin.Forms.GoogleMaps;
using System.Windows.Input;
using Prism.Commands;

namespace taxi
{
	public class FromLocationPageViewModel : BindableBase
	{
		ILocationTracker _locationTracker;

		public FromLocationPageViewModel(ILocationTracker locationTracker)
		{
			_locationTracker = locationTracker;
			init();
		}

		void init()
		{
			MyLocation = "No Data";
			_locationTracker.LocationChanged += onLocationChanged;
			_locationTracker.StartTracking();
		}

		void onLocationChanged(object sender, GeographicLocation e)
		{
			MyLocation = e.ToString();
		}

		private string myLocation;
		public string MyLocation
		{
			get
			{
				return myLocation;
			}
			set
			{
				if(myLocation == value)
					return;

				myLocation = value;
				OnPropertyChanged(nameof(MyLocation));
			}
		}

		private string centerLocation;
		public string CenterLocation
		{
			get
			{
				return centerLocation;
			}
			set
			{
				if (centerLocation == value)
					return;

				centerLocation = value;
				OnPropertyChanged(nameof(CenterLocation));
			}
		}


		public Command CameraPositionCommand
		{
			get
			{
				return new Command((parameter) => {
					var position = parameter as CameraPosition;
					if (position != null)
					{
						CenterLocation = (new GeographicLocation(position.Target.Latitude,position.Target.Longitude)).ToString();
					}
				});
			}
		}



}
}
