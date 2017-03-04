using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Widget;
using Android.Views;
using taxi.Droid;

[assembly: ExportRenderer(typeof(ImageButton), typeof(ImageButtonRenderer))]
namespace taxi.Droid
{
	public class ImageButtonRenderer : ViewRenderer<ImageButton, ImageView>
	{
		protected override void OnElementChanged(ElementChangedEventArgs<ImageButton> e)
		{
			base.OnElementChanged(e);

			if (Control == null)
			{
				var _control = new ImageView(this.Context);
				SetNativeControl(_control);
			}

			if (e.OldElement != null)
			{
				Control.Click -= onClick;
			}

			if (e.NewElement != null)
			{
				if (Element != null)
				{
					setClickHandler();
					setImage();
				}
			}
		}


		private void setClickHandler()
		{
			//	Control.Clickable = true;
			//	Control.Click += onClick;
			//	Control.Touch += onTouch;
		}

		void onTouch(object sender, TouchEventArgs e)
		{
			if (e.Event.Action == MotionEventActions.Move)
			{

			}
		}



		void setImage()
		{
			if (string.IsNullOrEmpty(Element.ImageSource))
				return;
			var drawableName = Element.ImageSource
									  .Split('.')[0]
									  .ToLower();
			var imageResourceId = Resources.GetIdentifier(drawableName, "drawable", Context.PackageName);
			if (imageResourceId != 0)

				Control.SetImageResource(imageResourceId);

		}


		protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

		}

		void onClick(object sender, EventArgs e)
		{

			var animator = Animate();
			animator.SetDuration(250);
			animator.Alpha(0.5f);
			animator.SetInterpolator(new R);
			animator.Start();

			if (Element != null && Element.Command != null && Element.Command.CanExecute(null))
				Element.Command.Execute(null);

		}
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			if (Control != null)
				Control.Click -= onClick;

		}

	}
}
