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
using Sharplike.Core.Rendering;
using System.Drawing;

namespace Sharplike.Mapping
{
	public class ErrorSquare : AbstractSquare
	{

		public override Boolean IsPassable(Direction d) { return false; }
		public override Color BackgroundColor { get { return Color.Black; } }

		private Glyph[] glyphs;
		public override Glyph[] Glyphs
		{
			get
			{
				return glyphs;
			}
		}
		
		public ErrorSquare (Int32 x, Int32 y, Int32 g) : base(x,y)
		{
			glyphs = new Glyph[1];
			glyphs[0] = new Glyph(g, Color.Red);
		}
	}
}
