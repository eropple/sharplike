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

namespace Sharplike.Core.Rendering
{
    public class RegionTile
    {
        internal DisplayTile displaytile;

        /// <summary>
        /// A list of all glyphs in the tile.
        /// </summary>
        /// <remarks>
        /// This should be treated as if it were read-only; manipulation of this list
        /// WILL NOT set the IsDirty flag and so repaints may act unexpectedly. Would
        /// have been internal/protected internal, but the renderers are in different
        /// namespaces so that wouldn't work.
        /// </remarks>
        public List<IGlyphProvider> GlyphProviders
        {
            get { return this.glyphList; }
        }
        private List<IGlyphProvider> glyphList = new List<IGlyphProvider>();

        /// <summary>
        /// Adds a glyph to the tile.
        /// </summary>
        /// <remarks>
        /// Many glyphs can be in a single tile, and they will just overlay each other.
        /// Make sure to invoke DisplayTile.ClearGlyphs() or DisplayTile.Reset() if
        /// you want to fully replace them.
        /// </remarks>
        /// <param name="glyphIndex">The index of the glyph to add.</param>
        /// <param name="glyphColor">The color of the specific glyph.</param>
        public void AddGlyph(Int32 glyphIndex, Color glyphColor, Color backColor)
        {
            this.glyphList.Add(new RawGlyphProvider(new Glyph(glyphIndex, glyphColor), backColor));
            if (displaytile != null)
                displaytile.MakeRenderDirty();
        }

        /// <summary>
        /// Adds a glyph to the tile.
        /// </summary>
        /// <remarks>
        /// Many glyphs can be in a single tile, and they will just overlay each other.
        /// Make sure to invoke DisplayTile.ClearGlyphs() or DisplayTile.Reset() if
        /// you want to fully replace them.
        /// </remarks>
        /// <param name="g">The pre-created glyph to add.</param>
        public void AddGlyphProvider(IGlyphProvider g)
        {
            this.glyphList.Add(g);
            if (displaytile != null)
                displaytile.MakeRenderDirty();
        }

		/// <summary>
		/// Removes a single glyph provider from the region tile.
		/// </summary>
		/// <param name="g">The glyph provider to remove</param>
		public void RemoveGlyphProvider(IGlyphProvider g)
		{
			this.glyphList.Remove(g);
			if (displaytile != null)
				displaytile.MakeRenderDirty();
		}

        /// <summary>
        /// Clears all glyphs from the tile. Does not reset the background color.
        /// </summary>
        public void ClearGlyphs()
        {
            this.glyphList.Clear();
            if (displaytile != null)
                displaytile.MakeRenderDirty();
        }

        /// <summary>
        /// Clears all glyphs and resets the background color to black.
        /// </summary>
        public void Reset()
        {
            this.ClearGlyphs();
        }
    }
}
