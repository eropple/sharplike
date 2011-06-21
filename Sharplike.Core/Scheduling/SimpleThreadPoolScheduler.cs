///////////////////////////////////////////////////////////////////////////////
/// Sharplike, The Open Roguelike Library (C) 2010 Ed Ropple.               ///
///                                                                         ///
/// This code is part of the Sharplike Roguelike library, and is licensed   ///
/// under the Common Public Attribution License (CPAL), version 1.0. Use of ///
/// this code is purusant to this license. The CPAL grants you certain      ///
/// permissions and requirements and should be read carefully before using  ///
/// this library.                                                           ///
///                                                                         ///
/// A copy of this license can be found in the Sharplike root directory,    ///
/// and must be included with all projects released using this library.     ///
///////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Sharplike.Core.Scheduling
{
	public class SimpleThreadPoolScheduler : AbstractSimpleScheduler
	{
		protected ManualResetEvent[] doneEvents;
		public override void Process()
		{
			doneEvents = new ManualResetEvent[tasks.Count];
			Int32 i = 0;
			foreach (IScheduledTask task in tasks)
			{
				doneEvents[i] = new ManualResetEvent(false);
				ThreadPool.QueueUserWorkItem(RunTask, new ThreadTask(task, doneEvents[i]));
				++i;
			}

			WaitHandle.WaitAll(doneEvents); // will wait for all events to complete
		}

		static void RunTask(Object data)
		{
			((ThreadTask)data).Task.ScheduledAction();
		}
	}
}
