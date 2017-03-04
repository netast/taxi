using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace taxi
{
	public class ImageButton : View
	{
		
		public static  readonly BindableProperty  ImageSourceProperty = BindableProperty.Create("ImageSource",typeof(string), typeof(ImageButton),  null, BindingMode.TwoWay);

		public static readonly BindableProperty CommandProperty = BindableProperty.Create("Command", typeof(ICommand),typeof(ImageButton),  null,BindingMode.TwoWay);

		public string ImageSource
		{
			get { return (string)GetValue(ImageSourceProperty); }
			set { SetValue(ImageSourceProperty, value); }
		}

		public ICommand Command
		{
			get { return (ICommand)GetValue(CommandProperty); }
			set { SetValue(CommandProperty, value); }
		}
	}
}
