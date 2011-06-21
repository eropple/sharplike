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

namespace Sharplike.Mapping
{
	public enum Direction
	{
		Northwest,
		North,
		Northeast,
		East,
		Southeast,
		South,
		Southwest,
		West,
		Up,
		Down,
		Here
	}

	public static class DirectionUtils
	{
		public static Direction OppositeDirection(Direction from)
		{
			switch (from)
			{
				case Direction.Northwest:
					return Direction.Southeast;
				case Direction.North:
					return Direction.South;
				case Direction.Northeast:
					return Direction.Southwest;

				case Direction.East:
					return Direction.West;
				case Direction.West:
					return Direction.East;

				case Direction.Southeast:
					return Direction.Northwest;
				case Direction.South:
					return Direction.North;
				case Direction.Southwest:
					return Direction.Northeast;

				case Direction.Up:
					return Direction.Down;
				case Direction.Down:
					return Direction.Up;
			}
			return Direction.Here;
		}
	}
}
