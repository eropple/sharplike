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

namespace Sharplike.Mapping.Squares
{
	[Serializable]
	public class WallSquare : AbstractSquare
	{
		public static Color WallColor = Color.Gray;

		public WallSquare()
		{
		}

		public override bool IsPassable(Direction fromDirection)
		{
			return false;
		}

		public override Glyph[] Glyphs
		{
			get
			{
				return new Glyph[] { CalculateAdjacencyGlyph(Location) };
			}
		}

		public override Vector3 Location
		{
			get;
			set;
		}

		private Glyph CalculateAdjacencyGlyph(Vector3 myLoc)
		{
			WallSquare north = Map.GetSafeSquare(myLoc + Vector3.North) as WallSquare;
			WallSquare south = Map.GetSafeSquare(myLoc + Vector3.South) as WallSquare;
			WallSquare east = Map.GetSafeSquare(myLoc + Vector3.East) as WallSquare;
			WallSquare west = Map.GetSafeSquare(myLoc + Vector3.West) as WallSquare;

			WallDirections hash = WallDirections.None;
			hash |= (north == null) ? WallDirections.None : WallDirections.North;
			hash |= (south == null) ? WallDirections.None : WallDirections.South;
			hash |= (east == null) ? WallDirections.None : WallDirections.East;
			hash |= (west == null) ? WallDirections.None : WallDirections.West;

			if (glyphs == null)
				SetupGlyphs();

			if (!glyphs.ContainsKey(hash))
				Console.WriteLine("WHAT!");
			return glyphs[hash];
		}

		private static Dictionary<WallDirections, Glyph> glyphs;
		private static List<WallSquare> walls = new List<WallSquare>();

		private static void SetupGlyphs()
		{
			glyphs = new Dictionary<WallDirections, Glyph>();
			glyphs[WallDirections.None] = new Glyph(0xF9, WallColor);

			//HEX values determined by http://en.wikipedia.org/wiki/Box_drawing_characters
			glyphs[WallDirections.North] = new Glyph(0xB3, WallColor);
			glyphs[WallDirections.South] = new Glyph(0xB3, WallColor);
			glyphs[WallDirections.North | WallDirections.South] = new Glyph(0xB3, WallColor);

			glyphs[WallDirections.East] = new Glyph(0xC4, WallColor);
			glyphs[WallDirections.West] = new Glyph(0xC4, WallColor);
			glyphs[WallDirections.East | WallDirections.West] = new Glyph(0xC4, WallColor);

			glyphs[WallDirections.South | WallDirections.West] = new Glyph(0xBF, WallColor);
			glyphs[WallDirections.North | WallDirections.East] = new Glyph(0xC0, WallColor);

			glyphs[WallDirections.North | WallDirections.West] = new Glyph(0xD9, WallColor);
			glyphs[WallDirections.South | WallDirections.East] = new Glyph(0xDA, WallColor);


			glyphs[WallDirections.North | WallDirections.South | WallDirections.West] = new Glyph(0xB4, WallColor);
			glyphs[WallDirections.North | WallDirections.East | WallDirections.West] = new Glyph(0xC1, WallColor);
			glyphs[WallDirections.South | WallDirections.East | WallDirections.West] = new Glyph(0xC2, WallColor);
			glyphs[WallDirections.North | WallDirections.South | WallDirections.East] = new Glyph(0xC3, WallColor);

			glyphs[WallDirections.All] = new Glyph(0xC5, WallColor);

		}

		public override bool Dirty
		{
			get
			{
				return false;
			}
		}

		public override AbstractMap Map
		{
			get;
			set;
		}

		[Flags]
		public enum WallDirections
		{
			None = 0,
			North = 1,
			South = 2,
			East = 4,
			West = 8,
			All = North | South | East | West
		}
	}
}
