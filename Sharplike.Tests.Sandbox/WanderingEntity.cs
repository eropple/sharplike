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
using Sharplike.Core.Rendering;
using Sharplike.Mapping;
using Sharplike.Mapping.Entities;
using Sharplike.UI;

namespace Sharplike.Tests.Sandbox
{
	[Serializable]
    public class WanderingEntity : AbstractEntity
    {
        public override Core.Rendering.Glyph[] Glyphs
        {
            get
            {
                return new Glyph[] { new Glyph((int)GlyphDefault.At, Color.White) };
            }
        }

        public void Wander()
        {
            Random r = new Random();
            Vector3 newloc = Location + new Vector3(r.Next(-1, 2), r.Next(-1, 2), 0);

            AbstractSquare sq = Map.GetSafeSquare(newloc);
            if (sq != null && sq.IsPassable(Direction.Here))
                Location = newloc;
        }
    }
}
