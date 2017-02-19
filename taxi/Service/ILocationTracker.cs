using System;
namespace taxi
{
	public interface ILocationTracker
	{
		event EventHandler<GeographicLocation> LocationChanged;
		void StartTracking();
		void PauseTracking();
	}
}
