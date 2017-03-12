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
			Resolver = Container;

			InitializeComponent();
			//NavigationService.NavigateAsync("NavigationPage/LoginPage");

			NavigationService.NavigateAsync("LoginPage");
		}

		protected override void RegisterTypes()
		{

			Container.RegisterTypeForNavigation<NavigationPage>();
			#region Pages
			Container.RegisterTypeForNavigation<LoginPage, LoginPageViewModel>();
			Container.RegisterTypeForNavigation<DestinationPage,DestinationPageViewModel>();
			Container.RegisterTypeForNavigation<FromLocationPage,FromLocationPageViewModel>();
			Container.RegisterTypeForNavigation<FromLocationAddressPage, FromLocationAddressPageViewModel>();
			Container.RegisterTypeForNavigation<OrderPage, OrderPageViewModel>();
			Container.RegisterTypeForNavigation<ConfirmOrderPage, ConfirmOrderPageViewModel>();
			#endregion

			#region Services
			Container.RegisterType<IRestService,RestService>(new ContainerControlledLifetimeManager());
			Container.RegisterType<ITaxiService, TaxiService>(new ContainerControlledLifetimeManager());
			#endregion
		}


		public static IUnityContainer Resolver { get; set; }
	}
}
