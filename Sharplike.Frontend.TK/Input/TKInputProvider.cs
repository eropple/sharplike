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
using System.Drawing;
using System.Windows.Forms;
using OpenTK;
using Sharplike.Core;
using Sharplike.Core.Input;
using Sharplike.Frontend;
using Sharplike.Frontend.Rendering;

namespace Sharplike.Frontend.Input
{
    public class TKInputProvider : AbstractInputProvider
    {
        public TKInputProvider()
        {
            TKRenderSystem rsys = (TKRenderSystem)Game.RenderSystem;
            win = (TKWindow)rsys.Window;

            win.Control.MouseMove += new MouseEventHandler(Form_MouseMove);
			win.Control.KeyDown += new KeyEventHandler(Form_KeyDown);
			win.Control.KeyUp += new KeyEventHandler(Form_KeyUp);
			win.Control.MouseDown += new MouseEventHandler(Form_MouseDown);
			win.Control.MouseUp += new MouseEventHandler(Form_MouseUp);
			win.Control.MouseWheel += new MouseEventHandler(Form_MouseWheel);
        }

		void Form_MouseWheel(object sender, MouseEventArgs e)
		{
			Point screenCoords = e.Location;
			Point tileCoords = this.ScreenCoordsToTileCoords(screenCoords);
			Boolean shift = (Form.ModifierKeys & Keys.Shift) == Keys.Shift;
			Boolean control = (Form.ModifierKeys & Keys.Control) == Keys.Control;
			Boolean alt = (Form.ModifierKeys & Keys.Alt) == Keys.Alt;

			Keys k = Keys.None;
			if (e.Delta < 0)
				k = Keys.PageDown;
			else if (e.Delta > 0)
				k = Keys.PageUp;

			this.MouseWheel(k, screenCoords, tileCoords, shift, control, alt);
		}

        void Form_MouseUp(object sender, MouseEventArgs e)
        {
			Point screenCoords = e.Location;
			Point tileCoords = this.ScreenCoordsToTileCoords(screenCoords);
			Boolean shift = (Form.ModifierKeys & Keys.Shift) == Keys.Shift;
			Boolean control = (Form.ModifierKeys & Keys.Control) == Keys.Control;
			Boolean alt = (Form.ModifierKeys & Keys.Alt) == Keys.Alt;

			Keys k = Keys.None;
			switch (e.Button)
			{
				case MouseButtons.Left:
					k = Keys.LButton;
					break;
				case MouseButtons.Middle:
					k = Keys.MButton;
					break;
				case MouseButtons.Right:
					k = Keys.RButton;
					break;
				case MouseButtons.XButton1:
					k = Keys.XButton1;
					break;
				case MouseButtons.XButton2:
					k = Keys.XButton2;
					break;
			}

			this.MouseUp(k, screenCoords, tileCoords, shift, control, alt);
        }

        void Form_MouseDown(object sender, MouseEventArgs e)
        {
			Point screenCoords = e.Location;
			Point tileCoords = this.ScreenCoordsToTileCoords(screenCoords);
			Boolean shift = (Form.ModifierKeys & Keys.Shift) == Keys.Shift;
			Boolean control = (Form.ModifierKeys & Keys.Control) == Keys.Control;
			Boolean alt = (Form.ModifierKeys & Keys.Alt) == Keys.Alt;

			Keys k = Keys.None;
			switch (e.Button)
			{
				case MouseButtons.Left:
					k = Keys.LButton;
					break;
				case MouseButtons.Middle:
					k = Keys.MButton;
					break;
				case MouseButtons.Right:
					k = Keys.RButton;
					break;
				case MouseButtons.XButton1:
					k = Keys.XButton1;
					break;
				case MouseButtons.XButton2:
					k = Keys.XButton2;
					break;
			}

			this.MouseDown(k, screenCoords, tileCoords, shift, control, alt);
        }

        void Form_KeyUp(object sender, KeyEventArgs e)
        {
            this.KeyUp(e.KeyData);
        }

        void Form_KeyDown(object sender, KeyEventArgs e)
		{
            this.KeyDown(e.KeyData);
        }

        void Form_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            mouseloc = new Point(e.X, e.Y);
        }

        public override void Poll()
        {
        }

        public override void Dispose()
        {
        }

        public override Point GetMousePosition()
        {
            return mouseloc;
        }

		public Point ScreenCoordsToTileCoords(Point screenCoords)
		{
			Size tileDimensions = win.GlyphPalette.GlyphDimensions;

			return (new Point(screenCoords.X / tileDimensions.Width,
								screenCoords.Y / tileDimensions.Height));
		}

        Point mouseloc;
        TKWindow win;
    }
}
