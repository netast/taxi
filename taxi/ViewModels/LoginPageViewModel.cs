using System;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;

namespace taxi.ViewModels
{
	public class LoginPageViewModel : BindableBase
	{
		IPageDialogService _dialogService;

		public LoginPageViewModel( IPageDialogService dialogService)
		{
			_dialogService = dialogService;
		}

		public DelegateCommand ShowMessageCommand
		{
			get{
				return new DelegateCommand(()=>{
					_dialogService.DisplayAlertAsync("Hello from Prism","Test Message","OK");
				});
			}
		}
	}
}
