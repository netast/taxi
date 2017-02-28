using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace taxi
{
	public partial class FromLocationPage : ContentPage
	{
		public FromLocationPage()
		{
			InitializeComponent();

			MyMap.MoveToRegion(
				MapSpan.FromCenterAndRadius(
					new Position(37,-122), Distance.FromMiles(1)));
		}
	}
}
