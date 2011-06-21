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
using System.Drawing;
using Sharplike.Core.Rendering;
using Sharplike.UI;

namespace Sharplike.UI.Controls
{
    public enum BorderStyle
    {
        None,
        Single,
        Double
    }

    public class Border : AbstractRegion
    {
        public Border(Size extents, Point location)
            : base(extents, location)
        {
            Draw();
        }

        void Draw()
        {
            this.Clear();

            int horizontals = 0;
            int verticals = 0;
            int topleft = 0;
            int topright = 0;
            int bottomleft = 0;
            int bottomright = 0;

            switch (Style)
            {
                case BorderStyle.Single:
                    horizontals = (int)GlyphDefault.BoxSingleHorizontal;
                    verticals = (int)GlyphDefault.BoxSingleVertical;
                    topleft = (int)GlyphDefault.BoxSingleDownSingleRight;
                    topright = (int)GlyphDefault.BoxSingleDownSingleLeft;
                    bottomleft = (int)GlyphDefault.BoxSingleUpSingleRight;
                    bottomright = (int)GlyphDefault.BoxSingleUpSingleLeft;
                    break;
                case BorderStyle.Double:
                    horizontals = (int)GlyphDefault.BoxDoubleHorizontal;
                    verticals = (int)GlyphDefault.BoxDoubleVertical;
                    topleft = (int)GlyphDefault.BoxDoubleDownDoubleRight;
                    topright = (int)GlyphDefault.BoxDoubleDownDoubleLeft;
                    bottomleft = (int)GlyphDefault.BoxDoubleUpDoubleRight;
                    bottomright = (int)GlyphDefault.BoxDoubleUpDoubleLeft;
                    break;
                case BorderStyle.None:
                default:
                    horizontals = verticals = topleft = topright = bottomleft = bottomright = (int)GlyphDefault.Blank;
                    break;
            }

            for (int side = 1; side < this.Size.Height - 1; ++side)
            {
                this.RegionTiles[0, side].AddGlyph(verticals, fg, bg);
                this.RegionTiles[this.Size.Width - 1, side].AddGlyph(verticals, fg, bg);
            }

            for (int end = 1; end < this.Size.Width - 1; ++end)
            {
                this.RegionTiles[end, 0].AddGlyph(horizontals, fg, bg);
                this.RegionTiles[end, this.Size.Height - 1].AddGlyph(horizontals, fg, bg);
            }

            //Left side
            this.RegionTiles[0, 0].AddGlyph(topleft, fg, bg);
            this.RegionTiles[0, this.Size.Height - 1].AddGlyph(bottomleft, fg, bg);

            //Right side
            this.RegionTiles[this.Size.Width - 1, 0].AddGlyph(topright, fg, bg);
            this.RegionTiles[this.Size.Width - 1, this.Size.Height - 1].AddGlyph(bottomright, fg, bg);

            for (int x = 1; x < this.Size.Width - 1; ++x)
            {
                for (int y = 1; y < this.Size.Height - 1; ++y)
                {
                    this.RegionTiles[x, y].AddGlyph(0, bg, bg);
                }
            }
        }

        public Color ForegroundColor
        {
            get { return fg; }
            set
            {
                fg = value;
                Draw();
            }
        }
        private Color fg = Color.White;

        public Color BackgroundColor
        {
            get { return bg; }
            set
            {
                bg = value;
                Draw();
            }
        }
        private Color bg = Color.Black;

        public BorderStyle Style
        {
            get { return drawstyle; }
            set
            {
                drawstyle = value;
                Draw();
            }
        }
        private BorderStyle drawstyle = BorderStyle.Single;
    }
}
