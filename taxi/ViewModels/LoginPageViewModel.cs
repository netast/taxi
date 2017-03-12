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
		INavigationService _navigationService;

		public LoginPageViewModel( IPageDialogService dialogService, 
		                           ITaxiService taxiService,
		                           INavigationService navigationService)
		{
			_dialogService = dialogService;
			_taxiService = taxiService;
			_navigationService = navigationService;
			#if DEBUG
			PhoneNumber="9039818881";
			Password="8144";
				
			#endif 
		}

		private bool isLoading;
		public bool IsLoading
		{
			get{
				return isLoading;
			}
			set{
				if(value != isLoading)
				{
					isLoading = value;
					OnPropertyChanged(nameof(IsLoading));
				}
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
						await _dialogService.DisplayAlertAsync("Телефон","Введите номер телефона","OK");
						return;
					}
						
					IsLoading = true;
					var result = await _taxiService.ActivateClientBySMSAsync(PhoneNumber);
					IsLoading = false;
					if(!result.result){
						await _dialogService.DisplayAlertAsync("Пароль","Не удалось запросить пароль, проверьте соединение " + result.message,"OK");
					}
					else
					{
						await _dialogService.DisplayAlertAsync("Пароль",result.message,"OK");
					}
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
						await _dialogService.DisplayAlertAsync("Телефон","Введите номер телефона","OK");
						return;
					}

					if(string.IsNullOrEmpty(Password)){
						await _dialogService.DisplayAlertAsync("Пароль","Не удалось запросить пароль, проверьте соединение","OK");
						return;
					}
					IsLoading = true;
					var loginResult = await _taxiService.LoginMobileAsync(PhoneNumber,Password);
					IsLoading = false;
					if(!loginResult.result){
						await _dialogService.DisplayAlertAsync("Номер телефона или пароль не верны ","Ошибка","OK");
						return;
					}

					await _navigationService.NavigateAsync("FromLocationPage");

				});
			}
			
		}
	}
}
