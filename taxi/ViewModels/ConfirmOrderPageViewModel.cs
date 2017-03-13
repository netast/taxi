using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using taxi.Model;
using taxi.Service;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace taxi
{
	public class ConfirmOrderPageViewModel : BindableBase, INavigationAware
	{
		IPageDialogService _dialogService;
		ITaxiService _taxiService;
		INavigationService _navigationService;
		WebOrder webOrder;

		public ConfirmOrderPageViewModel(IPageDialogService dialogService, ITaxiService taxiService, INavigationService navigationService)
		{
			_dialogService = dialogService;
			_taxiService = taxiService;
			_navigationService = navigationService;
		}


		private IList<Position> centerMapFromPositions;
		public IList<Position> CenterMapFromPositions
		{
			get
			{
				return centerMapFromPositions;
			}
			set
			{
				if (value != centerMapFromPositions)
				{
					centerMapFromPositions = value;
					OnPropertyChanged(nameof(CenterMapFromPositions));
				}
			}
		}


		public ObservableCollection<Polyline> Polylines { get; set; }
		public ObservableCollection<Pin> Pins { get; set; }

		public string Distance
		{
			get 
			{
				var distance = webOrder?.DistanceKM;
				return distance != null ? distance + " Км" : "";
			}
		}

		public string Amount
		{
			get 
			{
				var amount = webOrder?.OrderAmount; 
				return amount != null ? (int)amount + " руб." : "";
			}
		}


		public Command PlaceOrderCommand
		{
			get 
			{
				return new Command(async () => 
				{

					try
					{
						var result = await _taxiService.AddOrderAsync(webOrder);

						if (string.IsNullOrEmpty(result.Message))
						{
							await _dialogService.DisplayAlertAsync("Спасибо", "Заказ принят", "OK");

							await _navigationService.NavigateAsync("/FromLocationPage");
						}
						else
						{
							await _dialogService.DisplayAlertAsync("Заказ не принят", result.Message, "OK");
						}
					}
					catch (Exception ex)
					{
						await _dialogService.DisplayAlertAsync("Заказ не принят", "Проверьте интернет соендинение", "OK");
					}
				});
			}
		}


		public void OnNavigatedFrom(NavigationParameters parameters)
		{
			
		}

		public void OnNavigatedTo(NavigationParameters parameters)
		{
			var order = parameters["WebOrder"] as WebOrder;
			webOrder = order;
			if (order == null)
				return;


			showRoute();
		
			OnPropertyChanged(nameof(Distance));
			OnPropertyChanged(nameof(Amount));
		}



		private void showRoute()
		{
			var routePoints = GooglePolylineCoder.DecodeGooglePolyline(webOrder.RouteMetrics);

			var polyline = new Polyline();
			polyline.StrokeColor = Color.Blue;
			polyline.StrokeWidth = 5f;
			//polyline.Tag = "POLYLINE"; // Can set any object
		
			foreach (var point in routePoints)
			{
				polyline.Positions.Add(new Position(point.Lat, point.Lon));
			}

			var positions = routePoints.Select(rp => new Position(rp.Lat,rp.Lon)).ToList();
			CenterMapFromPositions = positions;

			Polylines.Clear();
			Polylines.Add(polyline);

		

			var startPoint = routePoints.FirstOrDefault();
			var endPin = routePoints.LastOrDefault();
			var startPin = new Pin
			{

				Position = new Position(startPoint.Lat, startPoint.Lon),
				IsDraggable = false,
				IsVisible = true,
				Label = "Старт",
				Address= webOrder.SrcAddress.StreetOrPlace + ", " + webOrder.SrcAddress.House,
				Type = PinType.Place
			};

			var assembly = typeof(ConfirmOrderPage).GetTypeInfo().Assembly;
			var stream = assembly.GetManifestResourceStream($"taxi.Images.start_s.png");
			startPin.Icon = BitmapDescriptorFactory.FromStream(stream);

			var finishPin = new Pin
			{

				Position = new Position(endPin.Lat, endPin.Lon),
				IsDraggable = false,
				IsVisible = true,
				Label = "Финиш",
				Type = PinType.Place,
				Address = webOrder.DstAddresses[0].StreetOrPlace + ", "  + webOrder.DstAddresses[0].House
			};

			stream = assembly.GetManifestResourceStream($"taxi.Images.finish_s.png");
			finishPin.Icon = BitmapDescriptorFactory.FromStream(stream);

			Pins.Add(startPin);
			Pins.Add(finishPin);
		}


		private double measurePositionDelta(RoutePoint one, RoutePoint two)
		{

			var lat1 = one.Lat;
			var lon1 = one.Lon;
			var lat2 = two.Lat;
			var lon2 = two.Lon;


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


		    private  RoutePoint getCentralGeoCoordinate(
			   IList<RoutePoint> geoCoordinates)
		{
			if (geoCoordinates.Count == 1)
			{
				return geoCoordinates.Single();
			}

			double x = 0;
			double y = 0;
			double z = 0;

			foreach (var geoCoordinate in geoCoordinates)
			{
				var latitude = geoCoordinate.Lat * Math.PI / 180;
				var longitude = geoCoordinate.Lon * Math.PI / 180;

				x += Math.Cos(latitude) * Math.Cos(longitude);
				y += Math.Cos(latitude) * Math.Sin(longitude);
				z += Math.Sin(latitude);
			}

			var total = geoCoordinates.Count;

			x = x / total;
			y = y / total;
			z = z / total;

			var centralLongitude = Math.Atan2(y, x);
			var centralSquareRoot = Math.Sqrt(x * x + y * y);
			var centralLatitude = Math.Atan2(z, centralSquareRoot);

			return new RoutePoint(centralLatitude * 180 / Math.PI, centralLongitude * 180 / Math.PI);
		}



	}
}
