using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using Microsoft.Practices.Unity;
using Prism.Unity;
using UIKit;

namespace taxi.iOS
{
	[Register("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
			global::Xamarin.Forms.Forms.Init();
			Xamarin.FormsGoogleMaps.Init("AIzaSyCsJCDHY_yizQY4ZKYSFZUbPUGSVRFl8q4");
			LoadApplication(new App(new AppleInitializer()));

			return base.FinishedLaunching(app, options);
		}
	}

	public class AppleInitializer : IPlatformInitializer
	{
		public void RegisterTypes(IUnityContainer container)
		{
			
		}
	}
}
