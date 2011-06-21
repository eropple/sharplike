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
using System.Drawing;
using System.Windows.Forms;

namespace Sharplike.Core.Input
{
	/// <summary>
	/// A command, as is given to the game loop for processing.
	/// </summary>
	public class CommandData
	{
		/// <summary>
		/// The invoked command.
		/// </summary>
		public readonly String Command;
		/// <summary>
		/// Set to True if the command is a valid mouse event. If False,
		/// ScreenCoordinates and TileCoordinates will not hold meaningful data.
		/// </summary>
		public readonly Boolean IsMouseEvent;
		/// <summary>
		/// The point on screen where the mouse command occurred.
		/// </summary>
        public Point ScreenCoordinates
        {
            get
            {
                return Game.InputSystem.GetMousePosition();
            }
        }
		/// <summary>
		/// The screen tile on which the mouse command occurred.
		/// </summary>
        public Point TileCoordinates
        {
            get
            {
                Point sc = ScreenCoordinates;
                int x = sc.X / Game.RenderSystem.Window.GlyphPalette.GlyphDimensions.Width;
				int y = sc.Y / Game.RenderSystem.Window.GlyphPalette.GlyphDimensions.Width;
                return new Point(x,y);
            }
        }

		public CommandData(String command)
		{
			this.Command = command;
			this.IsMouseEvent = false;
		}
		public CommandData(String command, Boolean isMouseEvent)
		{
			this.Command = command;
			this.IsMouseEvent = isMouseEvent;
		}

		public override string ToString()
		{
			String str = "CommandData: { " + this.Command;
			if (this.IsMouseEvent)
			{
				str += " (mouse) ";
				str += "TileXY: " + this.TileCoordinates.ToString() + " ";
				str += "ScreenXY: " + this.ScreenCoordinates.ToString();
			}
			str += " }";
			return str;
		}
	}
}
