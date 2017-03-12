﻿using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace taxi
{
	public partial class FromLocationPage : ContentPage
	{
		public FromLocationPage()
		{
			InitializeComponent();

			fromMap.MoveToRegion(
			 MapSpan.FromCenterAndRadius(
				new Position(54.9884804, 73.32423620000009), Distance.FromMiles(1)));
		}
	}
}
