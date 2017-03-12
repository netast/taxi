using System;
using System.Collections.Generic;
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
		public static readonly BindableProperty CenterMapFromPositionsProperty = BindableProperty.Create(nameof(CenterMapFromPositions), typeof(IList<Position>), typeof(EMap), propertyChanged:onCenterPositionsChanged);

		static void onCenterCameraPositionChanged(BindableObject bindable, object oldValue, object newValue)
		{
			var map = bindable as Map;

			if (map != null) {
				var center = newValue as CameraPosition;
				if (center != null)
				{
					map.MoveToRegion(MapSpan.FromCenterAndRadius(center.Target, new Distance(center.Zoom)));
				}
			}
		}

		static void onCenterPositionsChanged(BindableObject bindable, object oldValue, object newValue)
		{
			var map = bindable as Map;

			if (map != null)
			{
				var positions = newValue as IList<Position>;
				if (positions != null)
				{
					map.MoveToRegion(MapSpan.FromPositions(positions));
				}
			}
		}

		public IList<Position> CenterMapFromPositions
		{ 
			get 
			{
				return (IList<Position>)GetValue(CenterMapFromPositionsProperty);
			}
			set 
			{
				SetValue(CenterMapFromPositionsProperty, value);
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
