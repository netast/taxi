using System;
using Prism.Unity;
using taxi.ViewModels;
using taxi.Views;
using Xamarin.Forms;
using taxi.Service;
using Microsoft.Practices.Unity;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation (XamlCompilationOptions.Compile)]
namespace taxi
{
	public partial class App : PrismApplication
	{
		public App(IPlatformInitializer initializer = null) : base(initializer) { }

		protected override void OnInitialized()
		{
			InitializeComponent();
			//NavigationService.NavigateAsync("LoginPage");

			NavigationService.NavigateAsync("FromLocationPage");
		}

		protected override void RegisterTypes()
		{
			#region Pages
			Container.RegisterTypeForNavigation<LoginPage, LoginPageViewModel>();
			Container.RegisterTypeForNavigation<DestinationPage,DestinationPageViewModel>();
			Container.RegisterTypeForNavigation<FromLocationPage,FromLocationPageViewModel>();
			#endregion

			#region Services
			Container.RegisterType<IRestService,RestService>(new ContainerControlledLifetimeManager());
			Container.RegisterType<ITaxiService, TaxiService>(new ContainerControlledLifetimeManager());
			#endregion
		}
	}
}
