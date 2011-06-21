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
using System.IO;

namespace Sharplike.Core.Rendering
{
	public class GlyphPalette
	{
		/// <summary>
		/// The original source image for all glyphs.
		/// </summary>
		public readonly Bitmap SourceBitmap;

		/// <summary>
		/// The number of rows in the source bitmap.
		/// </summary>
		public readonly Int32 RowCount;
		/// <summary>
		/// The number of columns in the source bitmap.
		/// </summary>
		public readonly Int32 ColumnCount;
		/// <summary>
		/// The total number of glyphs in the glyph palette.
		/// </summary>
		public readonly Int32 GlyphCount;

		/// <summary>
		/// The dimensions of a single glyph. Each tile, then, is the same size
		/// in pixel terms.
		/// </summary>
		public readonly Size GlyphDimensions;

        /// <summary>
        /// Stream Constructor.
        /// </summary>
        /// <param name="filename">Filename of the glyph bitmap.</param>
        /// <param name="numRows">Number of rows in the glyph bitmap.</param>
        /// <param name="numCols">Number of columns in the glyph bitmap.</param>
        public GlyphPalette(Stream filedata, Int32 numRows, Int32 numCols)
        {
            this.SourceBitmap = new Bitmap(filedata);
            this.RowCount = numRows;
            this.ColumnCount = numCols;
            this.GlyphCount = numRows * numCols;

            this.GlyphDimensions = new Size(this.SourceBitmap.Width / numCols,
                                            this.SourceBitmap.Height / numRows);

            Glyph.GlyphCount = this.GlyphCount;
        }

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="filename">Filename of the glyph bitmap.</param>
		/// <param name="numRows">Number of rows in the glyph bitmap.</param>
		/// <param name="numCols">Number of columns in the glyph bitmap.</param>
		public GlyphPalette(String filename, Int32 numRows, Int32 numCols)
		{
			this.SourceBitmap = new Bitmap(filename);
			this.RowCount = numRows;
			this.ColumnCount = numCols;
			this.GlyphCount = numRows * numCols;

			this.GlyphDimensions = new Size(this.SourceBitmap.Width / numCols,
											this.SourceBitmap.Height / numRows);

			Glyph.GlyphCount = this.GlyphCount;
		}

		/// <summary>
		/// Generates a rectangle that fully contains the glyph at the specified
		/// index.
		/// </summary>
		/// <param name="glyphIndex">The glyph to select.</param>
		/// <returns>A rectangle that fully contains the specified glyph.</returns>
		public Rectangle ComputeGlyphRectangle(Int32 glyphIndex)
		{
			Int32 glyphCol = glyphIndex % this.ColumnCount;
			Int32 glyphRow = glyphIndex / this.ColumnCount;

			Point p = new Point(glyphCol * this.GlyphDimensions.Width, glyphRow * this.GlyphDimensions.Height);
			Rectangle r = new Rectangle(p, this.GlyphDimensions);

			return r;
		}
	}
}
