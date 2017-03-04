using System;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;

namespace taxi
{
	public partial class LocationButton : ContentView
	{
		public static readonly BindableProperty ClickedCommandProperty = BindableProperty.Create(nameof(ClickedCommand), typeof(ICommand), typeof(LocationButton));

		public ICommand ClickedCommand 
		{
			get 
			{
				return (ICommand)GetValue(ClickedCommandProperty);
			}
			set
			{
				SetValue(ClickedCommandProperty, value);
			}
		}

		public LocationButton()
		{
			InitializeComponent();
		}

		void OnTapped(object sender, EventArgs args)
		{
			if (ClickedCommand != null && ClickedCommand.CanExecute(null)) 
			{
				ClickedCommand.Execute(null);
			}
		}
	}
}
