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
		public static readonly BindableProperty CenterMapPositionProperty = BindableProperty.Create(nameof(CenterMapPosition), typeof(CameraPosition), typeof(EMap), propertyChanged: onCenterCameraPositionChanged);

		static void onCenterCameraPositionChanged(BindableObject bindable, object oldValue, object newValue)
		{
			var map = bindable as Map;

			if (map != null) {
				var center = newValue as CameraPosition;
				if (center != null)
				{
					//map.MoveCamera(CameraUpdateFactory.NewPositionZoom(center.Target, center.Zoom));

					map.MoveToRegion(MapSpan.FromCenterAndRadius(center.Target, new Distance(center.Zoom)));
				}
			}
		}


		public CameraPosition CenterMapPosition
		{
			get
			{
				return (CameraPosition)GetValue(CenterMapPositionProperty);
			}
			set
			{
				SetValue(CenterMapPositionProperty, value);
			}
		}


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
