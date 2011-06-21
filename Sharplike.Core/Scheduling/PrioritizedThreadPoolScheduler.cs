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
	/// <summary>
	/// The class for a prioritized scheduler. Internally the class
	/// uses SortedList for relatively efficient insertion and deletion
	/// of tasks, and is recommended only for multi-threaded schedulers.
	/// </summary>
	public class PrioritizedThreadPoolScheduler : IScheduler
	{
		protected SortedList<Int32, IScheduledTask> priorityList;

		/// <summary>
		/// Constructor.
		/// </summary>
		public PrioritizedThreadPoolScheduler()
		{
			priorityList = new SortedList<Int32, IScheduledTask>();
		}

		/// <summary>
		/// Adds a task to the priority queue. As no priority value has
		/// been given, the scheduler will use the delegate in
		/// TaskPriorityAlgorithm to determine a priority for the task.
		/// 
		/// Failure to set TaskPriorityAlgorithm ahead of time will
		/// throw an exception.
		/// </summary>
		/// <param name="task">The task to add to the scheduler.</param>
		public void AddTask(IScheduledTask task)
		{
			if (this.TaskPriorityAlgorithm == null)
				throw new InvalidOperationException("Must set TaskPriorityAlgorithm before using " +
					"the default AddTask function. Use the 2-argument form instead.");

			this.AddTask(this.TaskPriorityAlgorithm(task), task);
		}
		/// <summary>
		/// Adds a task to the scheduler with the specified priority.
		/// </summary>
		/// <param name="priority">The current task's priority for scheduling.</param>
		/// <param name="task">The task to be queued.</param>
		public void AddTask(Int32 priority, IScheduledTask task)
		{
			this.RemoveTask(task);
			priorityList.Add(priority, task);
		}
		/// <summary>
		/// Remove a task from the scheduler. This is a fail-safe function; it will
		/// not throw an exception if the task is not in the scheduler. Note that this
		/// function only removes the first occasion of a task; if you have accidentally
		/// registered the same task multiple times, it only removes one.
		/// </summary>
		/// <param name="task">The task to remove.</param>
		/// <returns>True if removed, false otherwise.</returns>
		public Boolean RemoveTask(IScheduledTask task)
		{
			Int32 foo = priorityList.IndexOfValue(task);
			if (foo > -1)
			{
				priorityList.Remove(foo);
				return true;
			}
			return false;
		}

		/// <summary>
		/// Clears the scheduler of all tasks.
		/// </summary>
		public void ClearTasks()
		{
			priorityList.Clear();
		}

		/// <summary>
		/// Feeds all currently enqueued tasks through the designated
		/// TaskPriorityAlgorithm to reorder them. This is a computationally
		/// slow function and should be called with caution.
		/// </summary>
		public void ReorderTasks()
		{
			if (this.TaskPriorityAlgorithm == null)
				throw new InvalidOperationException("Must set TaskPriorityAlgorithm before using " +
					"ReorderTasks.");

			IList<IScheduledTask> foo = priorityList.Values;

			SortedList<Int32, IScheduledTask> newTasks = new SortedList<Int32, IScheduledTask>();

			foreach (IScheduledTask t in foo)
			{
				newTasks.Add(this.TaskPriorityAlgorithm(t), t);
			}

			priorityList = newTasks;
		}

		/// <summary>
		/// Dispatches a "go" message to all subscribed processes.
		/// </summary>
		public void Process()
		{
			ManualResetEvent[] doneEvents = new ManualResetEvent[priorityList.Count];
			Int32 i = 0;
			foreach (IScheduledTask task in priorityList.Values)
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

		public delegate Int32 TaskPriorityDelegate(IScheduledTask newTask);
		/// <summary>
		/// Gets or sets the task prioritization algorithm. Used to automatically
		/// prioritize tasks as they're brought into the scheduler; must be used
		/// before calling AddTask without a defined priority or ReorderTasks.
		/// </summary>
		public TaskPriorityDelegate TaskPriorityAlgorithm { get; set; }
	}
}
