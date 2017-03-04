using System;
using Xamarin.Forms;
using Prism.Mvvm;
using Xamarin.Forms.GoogleMaps;
using System.Windows.Input;
using Prism.Commands;
using System.Diagnostics;

namespace taxi
{
	public class FromLocationPageViewModel : BindableBase
	{
		private bool gpsGot = false;
		GeographicLocation currentGeographicLocation;
		ILocationTracker _locationTracker;
		private Geocoder geocoder = new Geocoder();

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
			currentGeographicLocation = e;
			MyLocation = e.ToString();
			if (!gpsGot)
			{
				CameraPosition = CenterMapPosition = new CameraPosition(new Position(currentGeographicLocation.Latitude, currentGeographicLocation.Longitude), 300);
				gpsGot = true;
			}
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
				if (myLocation == value)
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


		private CameraPosition centerMapPosition;
		public CameraPosition CenterMapPosition
		{
			get
			{
				return centerMapPosition;
			}
			set
			{
				if (value != centerMapPosition)
				{
					centerMapPosition = value;
					OnPropertyChanged(nameof(CenterMapPosition));
				}
			}
		}


		private CameraPosition cameraPosition;
		public CameraPosition CameraPosition
		{
			get
			{
				return cameraPosition;
			}
			set
			{
				if (cameraPosition != value)
				{
					cameraPosition = value;
					OnPropertyChanged(nameof(CameraPosition));
				}
			}
		}

		private string fromPlace;
		public string FromPlace
		{
			get
			{
				return fromPlace;
			}
			set
			{
				if (fromPlace != value)
				{
					fromPlace = value;

					OnPropertyChanged(nameof(FromPlace));
				}
			}
		}


		private string searchResult;
		public string SearchResult
		{
			get
			{
				return searchResult;
			}
			set
			{
				if (searchResult != value)
				{
					searchResult = value;
					OnPropertyChanged(nameof(SearchResult));
				}
			}
		}

		public Command NextCommand
		{
			get
			{
				return new Command(() =>
				{

				});
			}
		}

		private Command cameraPositionCommand;
		public Command CameraPositionCommand
		{
			get
			{
				return cameraPositionCommand = cameraPositionCommand ?? new Command(async (parameter) =>
				{
				var position = parameter as CameraPosition;
					if (position != null)
					{
						CenterLocation = (new GeographicLocation(position.Target.Latitude, position.Target.Longitude)).ToString();
						SearchResult = "Ищем Вас ..";
						var streets = await geocoder.GetAddressesForPositionAsync(new Position(position.Target.Latitude, position.Target.Longitude));
						var enumerator = streets.GetEnumerator();
						enumerator.MoveNext();
						SearchResult = "Я здесь";
						var place = enumerator.Current;
						FromPlace = place != null ? place.Split(new string[] { Environment.NewLine }, StringSplitOptions.None)[0] : "Не найдено";
#if DEBUG
						Debug.WriteLine(place);
#endif
					}
				});
			}
		}

		public Command GoToMyLocationCommand
		{
			get 
			{
				return new Command(() => 
				{
					if (gpsGot)
					{
						CameraPosition = CenterMapPosition = new CameraPosition(new Position(currentGeographicLocation.Latitude, currentGeographicLocation.Longitude), 300);
					}
				});
			}
		}



}
}
