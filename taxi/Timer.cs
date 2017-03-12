using System;
using System.Threading;
using System.Threading.Tasks;

namespace taxi
{
	internal sealed class Timer : CancellationTokenSource, IDisposable
	{
		public Timer(Action<Object> callback, object state, int dueTime, int period)
		{
			Task.Delay(dueTime, Token).ContinueWith(async (t, s) =>
			{
				var tuple = (Tuple<Action<object>, object>)s;

				while (true)
				{
					if (IsCancellationRequested)
						break;
					await Task.Run(() => tuple.Item1(tuple.Item2));
					await Task.Delay(period);
				}

			}, Tuple.Create(callback, state), CancellationToken.None,
				TaskContinuationOptions.ExecuteSynchronously | TaskContinuationOptions.OnlyOnRanToCompletion,
				TaskScheduler.Default);
		}

		public new void Dispose() { base.Cancel(); }
	}
}
