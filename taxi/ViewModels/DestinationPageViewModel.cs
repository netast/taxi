using System;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using taxi.Service;
using System.Collections.Generic;
using System.Windows.Input;
using Prism.Commands;
using System.Diagnostics;
using System.Linq;

namespace taxi
{
	public class DestinationPageViewModel : BindableBase, INavigationAware
	{
		IPageDialogService _dialogService;
		ITaxiService _taxiService;
		INavigationService _navigationService;

		OrderRequest _order;

		public DestinationPageViewModel(IPageDialogService dialogService, ITaxiService taxiService,INavigationService navigationService)
		{
			_dialogService = dialogService;
			_taxiService = taxiService;
			_navigationService = navigationService;
		}


		private string text;
		public string Text{
			get{
				return text;
			}
			set{
				text = value;
				OnPropertyChanged(nameof(Text));
			}
		}

		private List<string> itemsSource;
		public List<string> ItemsSource{
			get{
				return itemsSource;
			}
			set
			{
				if( value == itemsSource)
					return;

				itemsSource = value;
				OnPropertyChanged(nameof(ItemsSource));
			}
		}

		private DelegateCommand textChangedCommand;
		public DelegateCommand TextChangedCommand{
			get{
				return textChangedCommand = textChangedCommand ?? new DelegateCommand(async () => {
					try{
						var result = await _taxiService.GetStreetsOrPlacesAsync(Text);
						//if(result != null && result.Length > 0)
							ItemsSource = result.ToList();
					}catch(Exception ex){
						#if DEBUG
						Debug.WriteLine("Error getting street list " + ex.Message);
						#endif
					}

				});
			}
		}


		private DelegateCommand doneCommand;
		public DelegateCommand DoneCommand
		{
			get
			{
				return doneCommand = doneCommand ?? new DelegateCommand(async () =>
				{
					_order.ToStreet = Text;
					var navParams = new NavigationParameters();
					navParams.Add("Order", _order);
					await _navigationService.NavigateAsync("OrderPage", navParams);
				});
			}
		}

		public void OnNavigatedFrom(NavigationParameters parameters)
		{
			
		}

	
			public void OnNavigatedTo(NavigationParameters parameters)
		{
			var order = parameters["Order"] as OrderRequest;
			_order = order;
			if (_order != null && !string.IsNullOrEmpty(_order.ToStreet))
			{
				Text = _order.ToStreet;
			}
		}

	}
}
