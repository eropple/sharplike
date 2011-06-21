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

using Sharplike.Core.Input;

namespace Sharplike.Core.Runtime
{
    public class StepwiseGameLoop : AbstractGameLoop
    {
		/// <summary>
        /// Creates a new step-based game loop, which processes game logic
        /// only in response to a keypress.
        /// </summary>
        /// <param name="callback">The game entry point.</param>
        public StepwiseGameLoop(Execute callback)
        {
            usercode = callback;
            Game.InputSystem.CommandTriggered += new EventHandler<Input.InputSystem.CommandEventArgs>(InputSystem_CommandTriggered);
			Game.Time = 0;
        }

		/// <summary>
		/// Creates a new step-based game loop, which processes game logic
		/// only in response to a keypress.
		/// </summary>
		/// <param name="callback">The game entry point.</param>
		/// <param name="startTime">The parameter to start the system at.</param>
		public StepwiseGameLoop(Execute callback, Int64 startTime)
		{
			usercode = callback;
			Game.InputSystem.CommandTriggered += new EventHandler<Input.InputSystem.CommandEventArgs>(InputSystem_CommandTriggered);
			Game.Time = startTime;
		}

		/// <summary>
		/// Creates a new step-based game loop, which processes game logic
		/// only in response to a keypress.
		/// </summary>
		/// <param name="stateMachine">The StateMachine used for this game's control flow.</param>
		public StepwiseGameLoop(Sharplike.Core.ControlFlow.StateMachine stateMachine)
		{
			usercode = stateMachine.GameLoopTick;
		}

        public override void Begin()
        {
			Boolean foo = true;
			while (foo == true && Game.Terminated == false)
			{
				foo = usercode(this);
				Game.Process();
			}
        }

		private CommandData DoWait()
		{
			while (lastcommands.Count == 0)
				Game.Process();
			return lastcommands.Dequeue();
		}
        
        /// <summary>
        /// Wait for a user to trigger one of the specified commands.
        /// This call will not return until the user triggers one of the expected commands.
        /// </summary>
        /// <param name="expected">The specific commands to expect.</param>
        /// <returns>The command data object.</returns>
        public CommandData WaitForInput(String[] expected)
        {
            List<String> exp = new List<string>(expected);
            CommandData cmd = null;
            do
            {
                cmd = DoWait();
            } while (!exp.Contains(cmd.Command));

			Game.Step();

            return cmd;
        }

        /// <summary>
        /// Wait for the user to press a key.
        /// </summary>
        /// <returns>The command raised by the user. See Sharpwise.Core.Input.InputSystem for details on input commands.</returns>
        public CommandData WaitForInput()
        {
			CommandData ret = DoWait();
			Game.Step();
			return ret;
        }

        void InputSystem_CommandTriggered(object sender, Input.InputSystem.CommandEventArgs e)
        {
            lastcommands.Enqueue(e.CommandData);
        }

        /// <summary>
        /// Defines the entry point of an application.
        /// </summary>
        /// <param name="loop">The StepwiseGameLoop that is in charge of this game.</param>
        public delegate Boolean Execute(StepwiseGameLoop loop);

        private Execute usercode;
		Queue<CommandData> lastcommands = new Queue<CommandData>();
    }
}
