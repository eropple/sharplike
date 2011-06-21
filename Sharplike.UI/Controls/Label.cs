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
using Sharplike.Core.Rendering;
using System.Drawing;

namespace Sharplike.UI.Controls
{
    public class Label : AbstractRegion
    {
        public Label(Size extents, Point location)
            : base(extents, location)
        {
        }

        public void SetText(String text)
        {
            this.Clear();
            int y = 0;
            foreach (String line in Wrap(text))
            {
                if (y > this.Size.Height - 1)
                    break;

                int x = 0;
                foreach (char c in line)
                {
                    if (x > this.Size.Width - 1)
                        break;
                    RegionTile tile = this.RegionTiles[x, y];
                    tile.ClearGlyphs();
                    tile.AddGlyph((int)c, Color, Background);
                    ++x;
                }
                ++y;
            }
        }

        private String[] Wrap(String text)
        {
            return text.Split(new String[] { "\n", "\r\n" }, StringSplitOptions.None);
        }

        public Color Background
        {
            get
            {
                return bg;
            }
            set
            {
                bg = value;
                SetText(this.text);
            }
        }
        private Color bg = Color.Black;

        public Color Color
        {
            get 
            { 
                return fg; 
            }
            set 
            { 
                fg = value; SetText(this.text); 
            }
        }
        private Color fg = Color.White;

        public String Text
        {
            get
            {
                return text;
            }

            set 
            {
                text = value;
                SetText(text);
            }
        }
        private String text = "";
    }
}
