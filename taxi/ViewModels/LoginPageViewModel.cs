using System;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using taxi.Service;

namespace taxi.ViewModels
{
	public class LoginPageViewModel : BindableBase
	{
		IPageDialogService _dialogService;
		ITaxiService _taxiService;

		public LoginPageViewModel( IPageDialogService dialogService, ITaxiService taxiService)
		{
			_dialogService = dialogService;
			_taxiService = taxiService;
		}

		public DelegateCommand ShowMessageCommand
		{
			get{
				return new DelegateCommand(()=>{
					_dialogService.DisplayAlertAsync("Hello from Prism","Test Message","OK");
				});
			}
		}


		private DelegateCommand sendPasswordCommand;
		public DelegateCommand SendPasswordCommand
		{
			get{
			return sendPasswordCommand = sendPasswordCommand ?? new DelegateCommand(async ()=>{
				
					var result = await _taxiService.ActivateClientBySMSAsync("9039818881");
				});
			}
		}
	}
}
