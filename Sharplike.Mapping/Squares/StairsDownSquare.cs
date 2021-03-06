﻿///////////////////////////////////////////////////////////////////////////////
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
using Sharplike.Mapping.Entities;

namespace Sharplike.Mapping.Squares
{
	[Serializable]
	public class StairsDownSquare : AbstractSquare
	{
		public static Glyph FloorGlyph = new Glyph(0x19, Color.Gray);

		public StairsDownSquare()
		{
		}

		public override bool Teleport(Direction enterFromDirection, AbstractEntity ent)
		{
			if (enterFromDirection != Direction.Down)
			{
				ent.Location = ent.Location + Vector3.Down;
				return true;
			}
			return false;
		}

		public override bool IsPassable(Direction fromDirection)
		{
			return true;
		}

		public override Glyph[] Glyphs
		{
			get
			{
				return new Glyph[] { FloorGlyph };
			}
		}

		public override bool Dirty
		{
			get
			{
				return false;
			}
		}
	}
}
