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
using Sharplike.Mapping.Entities;

using Sharplike.Core.Rendering;

namespace Sharplike.Mapping
{
	[Serializable]
	public abstract class AbstractSquare : IGlyphProvider
	{
		public virtual Boolean IsPassable(Direction fromDirection) { return false; }
		public virtual Boolean Teleport(Direction fromDirection, AbstractEntity newLocation) { return false; }
		public virtual Color BackgroundColor { get { return Color.Black; } }

		private static Glyph[] glyphs = { };
		public virtual Glyph[] Glyphs
		{
			get
			{
				return AbstractSquare.glyphs;
			}
		}

        public virtual bool Dirty
        {
			get { return false; }
        }

		public virtual AbstractMap Map
		{
			get { return null; }
			set { }
		}

		public virtual Vector3 Location
		{
			get { return Vector3.Zero; }
			set { }
		}
    }
}
