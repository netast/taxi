using System;
using Prism.Mvvm;
using Prism.Navigation;

namespace taxi
{
	public class OrderPageViewModel : BindableBase, INavigationAware
	{
		INavigationService _navigationService;



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
			OrderTime = Order.Time.TimeOfDay;
			OrderDate = Order.Time;
		}
	}
}
