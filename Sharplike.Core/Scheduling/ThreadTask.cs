using System;
using System.Threading;

namespace Sharplike.Core.Scheduling
{
	internal struct ThreadTask
	{
		internal readonly IScheduledTask Task;
		internal readonly ManualResetEvent DoneEvent;

		internal ThreadTask(IScheduledTask task, ManualResetEvent doneEvent)
		{
			this.Task = task;
			this.DoneEvent = doneEvent;
		}
	}
}
