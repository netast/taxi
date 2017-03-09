using System;
using Prism.Unity;
using taxi.ViewModels;

namespace taxi
{
	public static class ViewModelLocator
	{

		private static LoginPageViewModel loginPageViewModel;
		public static LoginPageViewModel LoginPageViewModel
		{
			get{
				//var loginVM = App.Resolver.Resolve(typeof(LoginPageViewModel), "loginPageViewModel") as LoginPageViewModel;
				return null;//loginPageViewModel = loginPageViewModel
						//?? loginVM;
			}
		}
	}
}
