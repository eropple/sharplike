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

namespace Sharplike.Core.Rendering
{
	/// <summary>
	/// Abstract class to encapsulate a front-end display for Sharplike roguelikes.
	/// </summary>
	/// <remarks>
	/// Allows developers to pretty easily swap out frontends and minimize game
	/// logic changes (for platform changes--MonoDroid is a primary target for
	/// Sharplike).
	/// </remarks>
	abstract public class AbstractWindow : AbstractRegion, IDisposable
	{
		/// <summary>
		/// The dimensions of the render screen, in pixels.
		/// </summary>
		public Size WindowSize
		{
			get { return displayDimensions; }
			set
			{
				displayDimensions = value;

				if (this.GlyphPalette == null)
					return;

				Int32 tileCols = this.WindowSize.Width / this.GlyphPalette.GlyphDimensions.Width;
				Int32 tileRows = this.WindowSize.Height / this.GlyphPalette.GlyphDimensions.Height;

				Size = new Size(tileCols, tileRows);
			}
		}
		private Size displayDimensions;
		/// <summary>
		/// The GlyphPalette for the Window, containing details of the available tiles.
		/// </summary>
		public readonly GlyphPalette GlyphPalette;


		protected String windowTitle;
		protected DisplayTile[,] tiles;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="displayDimensions">
		/// The dimensions of the render screen, in pixels.
		/// </param>
		/// <param name="palette">
		/// The GlyphPalette for the Window.
		/// </param>
		public AbstractWindow(Size displayDimensions, GlyphPalette palette)
            : base(new Size(displayDimensions.Width / palette.GlyphDimensions.Width, 
                displayDimensions.Height / palette.GlyphDimensions.Height), new Point(0, 0))
		{
			this.GlyphPalette = palette;
			this.WindowSize = displayDimensions;

			this.tiles = new DisplayTile[this.Size.Width, this.Size.Height];
			for (Int32 x = 0; x < this.Size.Width; x++)
			{
				for (Int32 y = 0; y < this.Size.Height; y++)
				{
					this.tiles[x, y] = new DisplayTile(this.GlyphPalette, this, new Point(x, y));
                    this.tiles[x, y].MakeStackDirty();
				}
			}
		}

		/// <summary>
		/// Accessor for the Window's tiles. Allows developer to index tiles
		/// without worry about accidentally replacing a tile; they are
		/// manipulable but not overwritable.
		/// </summary>
		/// <param name="x">X-coordinate of the tile to retrieve.</param>
		/// <param name="y">Y-coordinate of the tile to retrieve.</param>
		/// <returns>The tile at the specified location.</returns>
		public DisplayTile this[Int32 x, Int32 y]
		{
			get
			{
				return this.tiles[x, y];
			}
		}

		/// <summary>
		/// The Window's window title (in the title bar).
		/// </summary>
		public String WindowTitle
		{
			get
			{
				return this.windowTitle;
			}
			set
			{
				this.windowTitle = value;
				this.WindowTitleChange();
			}
		}
		/// <summary>
		/// Force a repaint of all tiles.
		/// </summary>
		public void Invalidate()
		{
			InvalidateTiles();
		}



		/// <summary>
		/// Update the Window (paint any changes to screen).
		/// </summary>
        public virtual void Update()
        {
            for (Int32 x = 0; x < this.Size.Width; x++)
            {
				for (Int32 y = 0; y < this.Size.Height; y++)
                {
                    if (tiles[x, y].IsStackDirty)
                        tiles[x, y].RebuildRegionTiles();
                }
            }
        }
		
		/// <summary>
		/// Invoked when the user changes the title (is called by the WindowTitle setter).
		/// </summary>
		protected abstract void WindowTitleChange();
	}
}
