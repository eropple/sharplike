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
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Sharplike.Core.Rendering
{
	/// <summary>
	/// A single tile in the render window. Should never be created by
	/// an end user (but there may be edge cases that make it necessary).
	/// </summary>
	public class DisplayTile
	{
        private Boolean isRenderDirty = true;
        private Boolean isStackDirty = true;

        private List<RegionTile> regionTiles = new List<RegionTile>();

        private Point location;

        GlyphPalette palette;
        AbstractRegion rootregion;
        

		/// <summary>
		/// Constructor.
		/// </summary>
        /// <param name="p">The GlyphPalette to use.</param>
		internal DisplayTile(GlyphPalette p, AbstractRegion root, Point loc)
		{
            palette = p;
            rootregion = root;
            location = loc;
		}

        public IList<RegionTile> RegionTiles
        {
            get
            {
                return regionTiles.AsReadOnly();
            }
        }

		/// <summary>
		/// Used in some renderers to determine if repainting is necessary.
		/// </summary>
		public Boolean IsRenderDirty
		{
			get
			{
				return this.isRenderDirty;
			}
		}

        public Boolean IsStackDirty
        {
            get
            {
                return this.isStackDirty;
            }
        }

		/// <summary>
		/// Signals to the tile that it shall be considered non-dirty until the tile
		/// is again updated.
		/// </summary>
		/// <remarks>
		/// Should only be called by the renderer, but maybe there's a case for an end
		/// user to call it, I don't know.
		/// </remarks>
		public void MarkRenderClean()
		{
			this.isRenderDirty = false;
		}
		/// <summary>
		/// Signals to the tile that it should be repainted even if it has not changed
		/// since the last repaint.
		/// </summary>
		/// <remarks>
		/// Generally should only be used through Window.Invalidate().
		/// </remarks>
		public void MakeRenderDirty()
		{
			this.isRenderDirty = true;
        }

        /// <summary>
        /// Signals to the tile that it's stack is now clean.
        /// </summary>
        public void MarkStackClean()
        {
            this.isStackDirty = false;
        }
        /// <summary>
        /// Signals to the tile that it should rebuild it's RegionTile stack.
        /// </summary>
        /// <remarks>
        /// Generally should only be used through Window.Invalidate().
        /// </remarks>
        public void MakeStackDirty()
        {
            this.isStackDirty = true;
        }

        /// <summary>
        /// Rebuilds our cached list of regiontiles that affect this displaytile
        /// </summary>
        internal void RebuildRegionTiles()
        {
            foreach (RegionTile r in regionTiles)
                r.displaytile = null;

            regionTiles.Clear();
            rootregion.PopulateRegionTiles(regionTiles, location);

            foreach (RegionTile r in regionTiles)
                r.displaytile = this;

            MarkStackClean();
        }
	}
}
