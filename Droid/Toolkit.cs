using System;
using Android.App;

namespace taxi.Droid
{
	public static class Toolkit
	{
		public static void Init(Activity activity)
		{
			Activity = activity;
		}

		public static Activity Activity {get; private set;}
	}
}
