using System;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace taxi
{
	public class EMap : Map
	{
		public EMap()
		{
			
		}

		BindableProperty CenterPositionProperty = BindableProperty.Create(nameof(CenterPosition),typeof(EMap),typeof(Position), defaultBindingMode: BindingMode.TwoWay, propertyChanged: centerPositionChanged);
		BindableProperty AreaProperty = BindableProperty.Create(nameof(Area),typeof(EMap), typeof(double),defaultBindingMode: BindingMode.TwoWay,propertyChanged:areaChanged);

		public Position CenterPosition
		{
			get{
				return (Position) GetValue(CenterPositionProperty);
			}
			set{
				SetValue(CenterPositionProperty,value);
			}
		}


		public double Area{
			get{
				return (double) GetValue(AreaProperty);
			}
			set{
				SetValue(AreaProperty,value);
			}
		}

		static void centerPositionChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if(bindable is EMap){
				var map = (EMap) bindable;


				updateMapPosition(map);
			}
		}

		static void areaChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if(bindable is EMap){
				var map = (EMap) bindable;

				updateMapPosition(map);
			}
		}



		static void updateMapPosition(EMap map){
			map.MoveToRegion(MapSpan.FromCenterAndRadius( map.CenterPosition, Distance.FromKilometers(map.Area)));
		}
}
}
