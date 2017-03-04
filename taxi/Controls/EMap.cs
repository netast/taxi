using System;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace taxi
{
	public class EMap : Map
	{

		public EMap()
		{
			CameraChanged += onCameraChanged;
		}

		public static readonly BindableProperty CameraPositionChangedCommandProperty = BindableProperty.Create(nameof(CameraPositionChangedCommand), typeof(ICommand), typeof(EMap));

		public ICommand CameraPositionChangedCommand
		{
			get
			{
				return (ICommand)GetValue(CameraPositionChangedCommandProperty);
			}
			set
			{
				SetValue(CameraPositionChangedCommandProperty, value);
			}
		}

		void onCameraChanged(object sender, CameraChangedEventArgs e)
		{
			if (CameraPositionChangedCommand != null && CameraPositionChangedCommand.CanExecute(null)) 
			{
				CameraPositionChangedCommand.Execute(e.Position);
			} 
		}



	}

}
