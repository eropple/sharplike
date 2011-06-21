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
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Xml;
using System.IO;
using Nini.Ini;

namespace Sharplike.Core.Input
{
    public sealed class InputSystem
    {
        internal InputSystem()
        {
        }

        /// <summary>
        /// Clears all current control bindings.
        /// </summary>
        public void ClearBindings()
        {
            rootcstate = new ControlState();
        }

        #region Load/Save Helpers
        /// <summary>
        /// Loads control bindings from a specified path.
        /// </summary>
        /// <param name="filename">The path to load from. Note: This does NOT automatically use Game.PathTo()</param>
        public void LoadConfiguration(String filename)
        {
            using (FileStream fs = new FileStream(filename, FileMode.Open))
                LoadConfiguration(fs);
        }

        /// <summary>
        /// Saves the current control bindings to a path on the filesystem.
        /// </summary>
        /// <param name="filename">The location of the file to write to. NOTE: This does NOT automatically use Game.PathTo()</param>
        public void SaveConfiguration(String filename)
        {
            using (FileStream fs = new FileStream(filename, FileMode.Create))
                SaveConfiguration(fs);
        }
        #endregion

        #region Load/Save
        /// <summary>
        /// Load control bindings from an arbitrary stream.
        /// If there are already bindings, the new ones will be added.
        /// New bindings overwrite old bindings.
        /// </summary>
        /// <param name="file">The source of the INI data</param>
        public void LoadConfiguration(Stream file)
        {
            using (IniReader r = new IniReader(file))
            {
                while (true)
                {
                    if (r.Type != IniType.Section)
                    {
                        if (!r.MoveToNextSection())
                            break;
                    }
                    String[] parts = r.Name.Split(' ');
                    if (parts[0] == "KeyBindings")
                    {
                        if (parts.Length > 1)
                        {
                            ControlState cs = rootcstate.GetChild(parts[1], true);
                            cs.ReadIni(r);
                        }
                        else
                        {
                            rootcstate.ReadIni(r);
                        }
					}
					/*else if (parts[0] == "MouseButtons")
					{
					}*/
					else if (parts[0] == "WindowEvents")
					{
						while (r.Read())
						{
							if (r.Type == IniType.Section)
								break;

							if (r.Type == IniType.Key)
								winEvents[r.Name] = r.Value;
						}
					}
					else
						r.MoveToNextSection();
                }
            }
        }

        /// <summary>
        /// Saves the current control bindings to an arbitrary stream.
        /// </summary>
        /// <param name="file">The writable stream to save to.</param>
        public void SaveConfiguration(Stream file)
        {
            using (IniWriter w = new IniWriter(file))
            {
                w.WriteSection("KeyBindings");
                rootcstate.WriteIni(w);
            }
        }

        internal ControlState rootcstate = new ControlState();
		internal Dictionary<String, String> winEvents = new Dictionary<string, string>();
        #endregion

        /// <summary>
        /// Retrieves the current screen-space mouse position.
        /// </summary>
        /// <returns>The mouse position in screen pixel space.</returns>
        public Point GetMousePosition()
        {
            return Provider.GetMousePosition();
        }

        #region Properties
		/// <summary>
		/// Gets or sets the current command set key. Command sets are hierarchical
		/// input systems (so that any active commands in Foo are active in Foo.Bar
		/// unless explicitly denied).
		/// 
		/// A null value or an empty string indicates to InputSystem to look only
		/// at the root command set.
		/// </summary>
		public String CommandSetKey
		{
			get;
			set;
		}

        public AbstractInputProvider Provider
        {
            get 
            { 
                return iprovider; 
            }
            internal set 
            { 
                iprovider = value; iprovider.System = this; 
            }
        }
        private AbstractInputProvider iprovider;
        #endregion

        #region Command Events

        /// <summary>
        /// Performs a one-shot trigger of a given command.
        /// </summary>
        /// <param name="command">The command to trigger.</param>
        public void TriggerCommand(CommandData command)
        {
            if (command == null)
                return;
            if (this.CommandTriggered != null)
                CommandTriggered(this, new CommandEventArgs(command));
        }

        /// <summary>
        /// Starts a game command. This command will be duplicated as a trigger, and will be
        /// re-triggered as a key-repeat until it is ended by a call to EndCommand().
        /// </summary>
        /// <param name="command">The command to start.</param>
        public void StartCommand(CommandData command)
        {
            if (command == null)
                return;
            if (!keytimers.ContainsKey(command.Command))
            {
                if (this.CommandStarted != null)
                    CommandStarted(this, new CommandEventArgs(command));

                TriggerCommand(command);
                Timer t = new Timer();
                t.Interval = 250;
                t.Start();
                t.Tick += delegate(object sender, EventArgs e)
                {
                    TriggerCommand(command);
                    t.Interval = 100;
                    t.Stop();
                    t.Start();
                };
                keytimers.Add(command.Command, t);
            }
        }

        /// <summary>
        /// Ends a command that was previously started. This will stop the command from being repeat-triggered.
        /// </summary>
        /// <param name="command">The name of the command to stop</param>
        public void EndCommand(CommandData command)
        {
            if (command == null)
                return;
            if (this.CommandEnded != null)
                CommandEnded(this, new CommandEventArgs(command));

            if (keytimers.ContainsKey(command.Command))
            {
                keytimers[command.Command].Dispose();
                keytimers.Remove(command.Command);
            }
        }

		/// <summary>
		/// Triggers a window event.
		/// </summary>
		/// <param name="eventname">The name of the window event to trigger.</param>
		public void WindowCommand(String eventname)
		{
			String cmd;
			if (winEvents.TryGetValue(eventname, out cmd))
				TriggerCommand(new CommandData(cmd));
		}

		/// <summary>
		/// Tests if the input system has a command for a particular event.
		/// </summary>
		/// <param name="eventname">The event to test.</param>
		/// <returns>True if a command has been assigned, false otherwise.</returns>
		public bool HasWindowEvent(String eventname)
		{
			return winEvents.ContainsKey(eventname);
		}

        public sealed class CommandEventArgs : EventArgs
        {

            public CommandEventArgs(CommandData cmdData)
            {
                Handled = false;
                CommandData = cmdData;
            }

            public Boolean Handled
            {
                get;
                set;
            }

            public CommandData CommandData
            {
                get;
                private set;
            }
        }

        Dictionary<String, Timer> keytimers = new Dictionary<String, Timer>();

        public delegate void CommandTriggeredEventHandler(object sender, CommandEventArgs e);
		/// <summary>
		/// Invoked when a command first reaches the InputSystem (analogous to the
		/// WinForms KeyPress event).
		/// </summary>
        public event EventHandler<CommandEventArgs> CommandTriggered;

        public delegate void CommandStartedEventHandler(object sender, CommandEventArgs e);
		/// <summary>
		/// Invoked when a command begins (analogous to the WinForms KeyDown event).
		/// </summary>
        public event EventHandler<CommandEventArgs> CommandStarted;

        public delegate void CommandEndedEventHandler(object sender, CommandEventArgs e);
		/// <summary>
		/// Invoked when a command begins (analogous to the WinForms KeyDown event).
		/// </summary>
        public event EventHandler<CommandEventArgs> CommandEnded;
        #endregion //Command events
    }
}
