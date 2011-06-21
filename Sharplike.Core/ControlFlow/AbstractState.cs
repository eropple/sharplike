///////////////////////////////////////////////////////////////////////////////
/// Sharplike, The Open Roguelike Library (C) 2010 2010 Ed Ropple.          ///
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

using Sharplike.Core.Runtime;
using Sharplike.Core.Input;

namespace Sharplike.Core.ControlFlow
{
	public abstract class AbstractState
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		public AbstractState()
		{
			
		}

		/// <summary>
		/// The parent state of this state. If "null," state is the
		/// first state in its stack.
		/// </summary>
		protected internal AbstractState Parent
		{
			get;
			internal set;
		}

		/// <summary>
		/// A reference to the StateMachine, for manipulation of pushing/popping
		/// and stack switching.
		/// </summary>
		protected internal StateMachine StateMachine
		{
			get;
			internal set;
		}

		/// <summary>
		/// The defined input command set for this state. Please remember that
		/// input sets are hierarchical; consult documentation for details.
		/// </summary>
		protected internal String InputCommandSet
		{
			get;
			protected set;
		}

		/// <summary>
		/// A state-based wrapper for Game.GameProcessing.
		/// 
		/// GameProcessing is called just before a frame is rendered to the screen.
		/// It may be used for graphical map effects, but should not contain game logic.
		/// </summary>
		protected internal virtual void GameProcessing()
		{

		}

		/// <summary>
		/// The logic tick at the core of the StepwiseGameLoop or RealtimeGameLoop.
		/// Primary game logic is done in this function.
		/// </summary>
		/// <param name="loop"></param>
		protected internal virtual void GameLoopTick(AbstractGameLoop loop)
		{
#if DEBUG
			Console.WriteLine(this.GetType().Name + ".GameLoopTick(" + loop.GetType().Name + ")");
#endif 
		}

		/// <summary>
		/// Invoked when the state is first started (added to the stack), but before
		/// any instances of GameLoopTick are called.
		/// </summary>
		protected internal virtual void StateStarted()
		{
#if DEBUG
			Console.WriteLine(this.GetType().Name + ".StateStarted()");
#endif 
		}

		/// <summary>
		/// Invoked after (or during) the last GameLoopTick of this state, during
		/// the process of popping it off the StateMachine's stack.
		/// </summary>
		protected internal virtual void StateEnded() 
		{
#if DEBUG
			Console.WriteLine(this.GetType().Name + ".StateEnded()");
#endif 
		}

		/// <summary>
		/// Invoked after another state has been added to the stack on top of this
		/// state, but before the other state's StateStarted function is called.
		/// </summary>
		protected internal virtual void StatePaused() 
		{
#if DEBUG
			Console.WriteLine(this.GetType().Name + ".StatePaused()");
#endif 
		}

		/// <summary>
		/// Invoked after a state is popped from the stack, after that state's StateEnded
		/// but before this state's next GameLoopTick.
		/// </summary>
		/// <param name="previousState">
		/// The state most recently popped from the state machine. Can be used with
		/// inherited states to access data (i.e., a dialog box's stateful contents).
		/// </param>
		protected internal virtual void StateResumed(AbstractState previousState) 
		{
#if DEBUG
			Console.WriteLine(this.GetType().Name + ".StateResumed(" + previousState.GetType().Name + ")");
#endif 
		}

		/// <summary>
		/// Invoked after (or during) the last GameLoopTick of this state, before control
		/// is switched over to a different stack.
		/// </summary>
		protected internal virtual void StackLostFocus() {
#if DEBUG
			Console.WriteLine(this.GetType().Name + ".StackLostFocus()");
#endif 
		}

		/// <summary>
		/// Invoked after control is switched back to this state's stack, but before this
		/// state's GameLoopTick is called.
		/// </summary>
		protected internal virtual void StackGotFocus() {
#if DEBUG
			Console.WriteLine(this.GetType().Name + ".StackGotFocus()");
#endif 
		}


		/// <summary>
		/// Invoked when the InputSystem detects a CommandTriggered event. This should NOT
		/// be used for games that use the StepwiseGameLoop; it is intended strictly for
		/// games using the RealtimeGameLoop.
		/// </summary>
		/// <param name="e">The command event details for the CommandTriggered event.</param>
		protected internal virtual void CommandTriggered(InputSystem.CommandEventArgs e) { }

		/// <summary>
		/// Invoked when the InputSystem detects a CommandStarted event. This should NOT
		/// be used for games that use the StepwiseGameLoop; it is intended strictly for
		/// games using the RealtimeGameLoop.
		/// </summary>
		/// <param name="e">The command event details for the CommandStarted event.</param>
		protected internal virtual void CommandStarted(InputSystem.CommandEventArgs e) { }

		/// <summary>
		/// Invoked when the InputSystem detects a CommandEnded event. This should NOT
		/// be used for games that use the StepwiseGameLoop; it is intended strictly for
		/// games using the RealtimeGameLoop.
		/// </summary>
		/// <param name="e">The command event details for the CommandEnded event.</param>
		protected internal virtual void CommandEnded(InputSystem.CommandEventArgs e) { }
	}
}
