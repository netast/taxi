using System;
using System.Collections.Generic;
using System.Diagnostics;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using taxi.Model;
using taxi.Service;
using Xamarin.Forms;

namespace taxi
{
	public class OrderPageViewModel : BindableBase, INavigationAware
	{
		INavigationService _navigationService;
		ITaxiService _taxiService;
		IPageDialogService _dialogService;
		 

		public OrderPageViewModel(INavigationService navigationService, ITaxiService taxiService, IPageDialogService dislogService)
		{
			_taxiService = taxiService;
			_navigationService = navigationService;
			_dialogService = dislogService;
		}

		private OrderRequest _order;
		public OrderRequest Order 
		{
			get 
			{
				return _order;
			}
			set {
				if (value != _order) 
				{
					_order = value;
					OnPropertyChanged(nameof(Order));
				}
			}
		}


		private bool setTime;
		public bool SetTime 
		{
			get
			{ return setTime; }
			set 
			{ 
				if (value != setTime) 
				{
					setTime = value;
					OnPropertyChanged(nameof(SetTime));
				}
			}
		}


		private TimeSpan orderTime;
		public TimeSpan OrderTime 
		{
			get 
			{
				return orderTime;
			}
			set
			{
				if (value != orderTime) 
				{
					orderTime = value;
					OnPropertyChanged(nameof(OrderTime));
				}
			}
		}


		private DateTime orderDate;
		public DateTime OrderDate
		{
			get
			{
				return orderDate;
			}
			set
			{
				if (value != orderDate) 
				{
					orderDate = value;
					OnPropertyChanged(nameof(OrderDate));
				}
			}
		}



		public string FromStreet 
		{
			get 
			{
				return Order?.FromStreet;
			}
			set {
				if (Order == null)
					return;

				if (Order.FromStreet == value)
					return;

				Order.FromStreet = value;
				OnPropertyChanged(nameof(FromStreet));
			}
		}

		public string ToStreet
		{
			get
			{
				return Order?.ToStreet;
			}
			set
			{
				if (Order == null)
					return;

				if (Order.ToStreet == value)
					return;

				Order.ToStreet = value;
				OnPropertyChanged(nameof(ToStreet));
			}
		}

		public OrderPageViewModel(INavigationService navigationService)
		{
			_navigationService = navigationService;
		}


		public void OnNavigatedFrom(NavigationParameters parameters)
		{
			
		}

		public void OnNavigatedTo(NavigationParameters parameters)
		{
			var order = parameters["Order"] as OrderRequest;
			Order = order;
			if (order == null)
				return;
			OrderTime = Order.Time.TimeOfDay;
			OrderDate = Order.Time;
			OnPropertyChanged(nameof(FromStreet));
			OnPropertyChanged(nameof(ToStreet));
		}



		public Command FromStreetTeppedCommand 
		{
			get {
				return new Command(async () => {
					Debug.WriteLine("From Street Tapped");
					var navParam = new NavigationParameters();
					navParam.Add("Order", Order);
					await _navigationService.NavigateAsync("FromLocationAddressPage", navParam);
				});
			}
		}

		public Command ToStreetTeppedCommand
		{
			get
			{
				return new Command(async () =>
				{
					Debug.WriteLine("To Street Tapped");
					var navParam = new NavigationParameters();
					navParam.Add("Order", Order);
					await _navigationService.NavigateAsync("DestinationPage", navParam);
				});
			}
		}


		public Command CalculateCommand
		{
			get 
			{
				return new Command(async ()=>
				{
					var newWebOrder = new WebOrder();
					newWebOrder.SrcAddress = new WebOrderAddress(Order.FromStreet, Order.FromHouse);
					newWebOrder.DstAddresses = new List<WebOrderAddress> { new WebOrderAddress(Order.ToStreet, Order.ToHouse) };

					try
					{
						var result = await _taxiService.GetOrderInfoAsync(newWebOrder);
						if (result == null) { 
						  await _dialogService.DisplayAlertAsync("Ошибка", "Заказ не принят, проверьте соединение", "ОК");
							return;
						
						}

						if (!string.IsNullOrEmpty(result.Message))
						{
							await _dialogService.DisplayAlertAsync("Заказ не принят", result.Message,"OK");
							return;
						}

						var navParams = new NavigationParameters();
						navParams.Add("WebOrder", result.WebOrder);

						await _navigationService.NavigateAsync("ConfirmOrderPage", navParams);
					}
					catch (Exception ex) 
					{
						await _dialogService.DisplayAlertAsync("Ошибка", "Заказ не принят, проверьте соединение", "ОК");
						return;
					}


				});
			}
		}
	}
}
