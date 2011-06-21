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
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Nini.Ini;

namespace Sharplike.Core.Input
{
    public class ControlState
    {
        public ControlState()
        {
            parent = null;
        }

        private ControlState(ControlState p)
        {
            parent = p;
        }

		public CommandData GetCommand(Keys keypress, String state)
		{
			return this.GetCommand(keypress, state, false);
		}
        public CommandData GetCommand(Keys keypress, String state, Boolean isMouse)
        {
            if (state == null || state == String.Empty)
            {
				if (keycommands.ContainsKey(keypress))
				{
					CommandData cmd = new CommandData(keycommands[keypress], isMouse);
					return cmd;
				}
				else
				{
					return null;
				}
            }

            int dotindex = state.IndexOf('.');
            String childname = null;
            String childns = null;
            if (dotindex == -1)
                childname = state;
            else
            {
                childname = state.Substring(0, dotindex);
                childns = state.Substring(dotindex + 1);
            }

            CommandData childresult = null;
            if (children.ContainsKey(childname))
                childresult = children[childname].GetCommand(keypress, childns);

            if (childresult == null)
            {
				if (keycommands.ContainsKey(keypress))
				{
					CommandData cmd = new CommandData(keycommands[keypress], isMouse);
					return cmd;
				}
				else
				{
					return null;
				}
            }
            return childresult;
        }

        public void SetCommand(Keys keycode, String command, String statename)
        {
            ControlState cs = GetChild(statename, true);
            if (cs == null)
                keycommands[keycode] = command;
            cs.keycommands[keycode] = command;
        }

        private String GetCommand(Keys keypress)
        {
            if (!keycommands.ContainsKey(keypress))
            {
                if (parent != null)
                    return parent.GetCommand(keypress);
                return null;
            }
            return keycommands[keypress];
        }

        internal ControlState GetChild(String location, bool create)
        {
            if (location == null)
                return this;

            int dotindex = location.IndexOf('.');
            String childname = null;
            String childns = null;
            if (dotindex == -1)
                childname = location;
            else
            {
                childname = location.Substring(0, dotindex);
                childns = location.Substring(dotindex + 1);
            }

            if (!children.ContainsKey(childname) && create)
            {
                ControlState cs = new ControlState(this);
                children[childname] = cs;
            }

            if (children.ContainsKey(childname))
                return children[childname].GetChild(childns, create);
            else
                return null;
        }

        #region Save/Load
        public void WriteIni(IniWriter w)
        {
            WriteIni(w, null);
        }
        private void WriteIni(IniWriter w, String ownns)
        {
            foreach (KeyValuePair<Keys, String> kvp in keycommands)
            {
                w.WriteKey(kvp.Key.ToString(), kvp.Value);
            }

            w.WriteEmpty();

            foreach (KeyValuePair<String, ControlState> kvp in children)
            {
                String childns = kvp.Key;
                if (ownns != null)
                    childns = String.Format("{0}.{1}", ownns, kvp.Key);

                w.WriteSection(String.Format("KeyBindings {0}", childns));
                kvp.Value.WriteIni(w, childns);
            }
        }

        public void ReadIni(IniReader r)
        {
            while (r.MoveToNextKey())
                keycommands.Add((Keys)Enum.Parse(typeof(Keys), r.Name), r.Value);
        }
        #endregion

        private Dictionary<Keys, String> keycommands = new Dictionary<Keys, string>();

        private Dictionary<String, ControlState> children = new Dictionary<string, ControlState>();
        private ControlState parent = null;
    }
}
