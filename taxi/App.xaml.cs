using System;
using Prism.Unity;
using taxi.ViewModels;
using taxi.Views;
using Xamarin.Forms;

namespace taxi
{
	public partial class App : PrismApplication
	{
		public App(IPlatformInitializer initializer = null) : base(initializer) { }

		protected override void OnInitialized()
		{
			InitializeComponent();
			NavigationService.NavigateAsync("LoginPage");
		}

		protected override void RegisterTypes()
		{


			Container.RegisterTypeForNavigation<LoginPage, LoginPageViewModel>();

		}
	}
}
