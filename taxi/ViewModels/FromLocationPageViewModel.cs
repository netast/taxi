using System;
using Xamarin.Forms;
using Prism.Mvvm;
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
    }
}
