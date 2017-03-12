using System;
using Xamarin.Forms;
using Prism.Mvvm;
using Xamarin.Forms.GoogleMaps;
using System.Windows.Input;
using Prism.Commands;
using System.Diagnostics;
using System.Threading.Tasks;
using Prism.Navigation;
using taxi.Service;

namespace taxi
{
	public class FromLocationPageViewModel : BindableBase
	{
		private bool gpsGot = false;
		GeographicLocation currentGeographicLocation;

		ILocationTracker _locationTracker;
		INavigationService _navigationService;
		ITaxiService _taxiService;


		private Geocoder geocoder = new Geocoder();
		private const double deltaMeters = 100;
		Timer timer;
		private CameraPosition lastProcessedPosition;
		private CameraPosition lastPosition;
		private OrderRequest order;

		public FromLocationPageViewModel(ILocationTracker locationTracker, 
		                                 INavigationService navigationService,
										 ITaxiService taxiService )
		{
			_locationTracker = locationTracker;
			_navigationService = navigationService;
			_taxiService = taxiService;
			init();
		}

		void init()
		{

			order = new OrderRequest();
			MyLocation = "No Data";
			_locationTracker.LocationChanged += onLocationChanged;
			_locationTracker.StartTracking();

			timer = new Timer(async (a) => {
			//	Debug.WriteLine("Tick");
				if (lastProcessedPosition != null && lastProcessedPosition == lastPosition)
					return;

				if (lastPosition == null)
					return;

				lastProcessedPosition = lastPosition;
				await processPosition(lastProcessedPosition);



			}, null, 500, 500);
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
				return new Command(async () => await gotoOrderPage());
			}
		}





		private Command cameraPositionCommand;
		public Command CameraPositionCommand
		{
			get
			{
				return cameraPositionCommand = cameraPositionCommand ?? new Command((parameter) =>
				{
					
				    var position = parameter as CameraPosition;
					lastPosition = position;

					//if (lastPosition != null && position != null) 
					//{
					//	var positionChange = measurePositionDelta(lastPosition.Target.Latitude, lastPosition.Target.Longitude, position.Target.Latitude, position.Target.Longitude);
					//	if (positionChange < deltaMeters)
					//		return;
					//}


				
						
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


		private Command selectAddressCommand;
		public Command SelectAddressCommand
		{
			get 
			{
				return selectAddressCommand = selectAddressCommand ?? new Command(async()=> await gotoOrderPage());
			}
		}



		private async Task gotoOrderPage () 
		{
			var navParams = new NavigationParameters();
			order.Time = DateTime.Now;
			navParams.Add("Order", order);
					await _navigationService.NavigateAsync("/NavigationPage/OrderPage", navParams);
         }

		private double measurePositionDelta(double lat1,double lon1,double lat2,double lon2)
		{  
			var R = 6378.137; // Radius of earth in KM
			var dLat = lat2 * Math.PI / 180 - lat1 * Math.PI / 180;
			var dLon = lon2 * Math.PI / 180 - lon1 * Math.PI / 180;
			var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
			Math.Cos(lat1 * Math.PI / 180) * Math.Cos(lat2 * Math.PI / 180) *
			Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
			var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
			var d = R * c;
			return d * 1000; // meters
		}


		private async Task processPosition(CameraPosition position)
		{
			if (position != null)
			{
				CenterLocation = (new GeographicLocation(position.Target.Latitude, position.Target.Longitude)).ToString();
				SearchResult = "Ищем Вас ..";

				var lat = position.Target.Latitude;
				var lon = position.Target.Longitude;

				//var streets = await geocoder.GetAddressesForPositionAsync(new Position(lat, lon));
				//var enumerator = streets.GetEnumerator();
				//enumerator.MoveNext();
				//
				//var place = enumerator.Current;
				//FromPlace = place != null ? place.Split(new string[] { Environment.NewLine }, StringSplitOptions.None)[0] : "Не найдено";
				var addr = await _taxiService.GetAddressByCoord(lat, lon);
				order.FromHouse = addr.House;
				order.FromStreet = addr.StreetOrPlace;
				FromPlace = addr != null ? addr.StreetOrPlace + (string.IsNullOrEmpty(addr.House) ? "" :  ", " + addr.House ): "";
				SearchResult = "Я здесь";

#if DEBUG
			 
				Debug.WriteLine(FromPlace);
				//Debug.WriteLine(place);
#endif

			}

		}
}
}
