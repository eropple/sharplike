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
using System.Windows.Forms;
using System.Text;
using System.Drawing;

namespace Sharplike.Core.Input
{
    public abstract class AbstractInputProvider : IDisposable
    {
        public abstract void Poll();
        public abstract Point GetMousePosition();

        public InputSystem System
        {
            get;
            internal set;
        }

		protected void KeyDown(Keys keycode)
        {
			CommandData cmd = System.rootcstate.GetCommand(keycode, System.CommandSetKey);
            System.StartCommand(cmd);
        }

        protected void KeyUp(Keys keycode)
        {
            CommandData cmd = System.rootcstate.GetCommand(keycode, System.CommandSetKey);
            System.EndCommand(cmd);
        }

        protected void KeyPress(Keys keycode)
        {
            CommandData cmd = System.rootcstate.GetCommand(keycode, System.CommandSetKey);
            System.TriggerCommand(cmd);
        }

        protected void MouseDown(Keys k, Point screenCoords, Point tileCoords,
			Boolean shift, Boolean control, Boolean alt)
        {
			if (shift)
				k = k | Keys.Shift;
			if (control)
				k = k | Keys.Control;
			if (alt)
				k = k | Keys.Alt;

			CommandData cmd = System.rootcstate.GetCommand(k, System.CommandSetKey, true);
			System.StartCommand(cmd);
        }

		protected void MouseUp(Keys k, Point screenCoords, Point tileCoords,
			Boolean shift, Boolean control, Boolean alt)
        {
			if (shift)
				k = k | Keys.Shift;
			if (control)
				k = k | Keys.Control;
			if (alt)
				k = k | Keys.Alt;

			CommandData cmd = System.rootcstate.GetCommand(k, System.CommandSetKey, true);
			System.EndCommand(cmd);
        }

		protected void MouseWheel(Keys k, Point screenCoords, Point tileCoords,
			Boolean shift, Boolean control, Boolean alt)
		{
			if (shift)
				k = k | Keys.Shift;
			if (control)
				k = k | Keys.Control;
			if (alt)
				k = k | Keys.Alt;

			CommandData cmd = System.rootcstate.GetCommand(k, System.CommandSetKey, true);
			System.StartCommand(cmd);
			System.EndCommand(cmd);
		}

        public abstract void Dispose();
    }
}
