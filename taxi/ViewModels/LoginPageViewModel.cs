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


		public string PhoneNumber {get;set;}
		public string Password {get;set;}


		private DelegateCommand sendPasswordCommand;
		public DelegateCommand SendPasswordCommand
		{
			get{
			return sendPasswordCommand = sendPasswordCommand ?? new DelegateCommand(async ()=>{
					if(string.IsNullOrEmpty(PhoneNumber)){
						await _dialogService.DisplayAlertAsync("Phone Number must be entered","Missing Login","OK");
						return;
					}
						

					var result = await _taxiService.ActivateClientBySMSAsync(PhoneNumber);
				});
			}
		}

		private DelegateCommand loginCommand;
		public DelegateCommand LoginCommand
		{
			get
			{
				return loginCommand = loginCommand ?? new DelegateCommand(async ()=>{
					if(string.IsNullOrEmpty(PhoneNumber)){
						await _dialogService.DisplayAlertAsync("Phone Number must be entered","Missing Login","OK");
						return;
					}

					if(string.IsNullOrEmpty(Password)){
						await _dialogService.DisplayAlertAsync("Password must be entered","Missing Password","OK");
						return;
					}

					var result = await _taxiService.LoginMobileAsync(PhoneNumber,Password);

				});
			}
			
		}
	}
}
