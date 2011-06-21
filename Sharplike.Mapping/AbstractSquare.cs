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
using System.Drawing;

using Sharplike.Core.Rendering;

namespace Sharplike.Mapping
{
	[Serializable]
	public abstract class AbstractSquare : IGlyphProvider
	{
		public readonly Vector3 Position;

		public virtual Boolean IsPassable(Direction fromDirection) { return true; }
		public virtual Color BackgroundColor { get { return Color.Black; } }

		private static Glyph[] glyphs = { };
		public virtual Glyph[] Glyphs
		{
			get
			{
				return AbstractSquare.glyphs;
			}
		}

		public AbstractSquare(Int32 x, Int32 y)
		{
			Position = new Vector3(x, y, 0);
		}
		public AbstractSquare(Point p)
		{
			Position = new Vector3(p);
		}
		public AbstractSquare(Int32 x, Int32 y, Int32 z)
		{
			Position = new Vector3(x, y, z);
		}
		public AbstractSquare(Vector3 v)
		{
			Position = v;
		}


        public bool Dirty
        {
            get;
            private set;
        }
    }
}
