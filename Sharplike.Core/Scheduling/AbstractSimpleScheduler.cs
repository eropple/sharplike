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

namespace Sharplike.Core.Scheduling
{
	/// <summary>
	/// The abstract class for any simple scheduler (using either single or
	/// multi-threaded models). Any scheduler that inherits from
	/// AbstractSimpleScheduler should dispatch tasks in the order they are
	/// added to the scheduler. (Note, of course, that in multi-threaded
	/// models they may not return in the same order.)
	/// </summary>
	public abstract class AbstractSimpleScheduler : IScheduler
	{
		protected List<IScheduledTask> tasks;

		/// <summary>
		/// Constructor.
		/// </summary>
		public AbstractSimpleScheduler()
		{
			tasks = new List<IScheduledTask>();
		}

		/// <summary>
		/// Adds a task to the scheduler.
		/// </summary>
		/// <param name="task">The task to add to the scheduler.</param>
		public void AddTask(IScheduledTask task) { tasks.Add(task); }

		/// <summary>
		/// Removes a task from the scheduler. Is a fail-safe function;
		/// will not throw an exception if the task was not there to
		/// begin with.
		/// </summary>
		/// <param name="task">The task to remove from the scheduler.</param>
		/// <returns>True if the task was removed, false otherwise.</returns>
		public Boolean RemoveTask(IScheduledTask task)
		{
			if (tasks.Contains(task) == false) return false;
			tasks.Remove(task);
			return true;
		}

		/// <summary>
		/// Clears all tasks from the scheduler.
		/// </summary>
		public void ClearTasks() { tasks.Clear(); }

		/// <summary>
		/// Processes the scheduler. This should not be called in end user code
		/// unless the end user knows exactly what they're doing.
		/// </summary>
		public abstract void Process();
	}
}
