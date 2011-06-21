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
using System.Drawing;

namespace Sharplike.Mapping
{
	[Serializable]
	public struct Vector3 : IEquatable<Vector3>
	{
		public static readonly Vector3 Zero = new Vector3(0, 0, 0);

		public static readonly Vector3 North = new Vector3(0, -1, 0);
		public static readonly Vector3 South = new Vector3(0, 1, 0);
		public static readonly Vector3 East = new Vector3(1, 0, 0);
		public static readonly Vector3 West = new Vector3(-1, 0, 0);
		public static readonly Vector3 Up = new Vector3(0, 0, 1);
		public static readonly Vector3 Down = new Vector3(0, 0, -1);

		public Vector3(int x, int y, int z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}
		public Vector3(Point p)
		{
			this.x = p.X;
			this.y = p.Y;
			this.z = 0;
		}
		
		public override string ToString ()
		{
			return string.Format("(" + x + "," + y + "," + z + ")");
		}

		
		public static void Divide(Vector3 a, Vector3 b, out Vector3 q, out Vector3 r)
		{
			int x = (int)Math.Floor((double)a.x / (double)b.x);
			int y = (int)Math.Floor((double)a.y / (double)b.y);
			int z = (int)Math.Floor((double)a.z / (double)b.z);
			int xr = a.x - b.x * x;
			int yr = a.y - b.y * y;
			int zr = a.z - b.z * z;
			
			q = new Vector3(x,y,z);
			r = new Vector3(xr,yr,zr);
		}

		public static Vector3 Add(Vector3 a, Vector3 b)
		{
			return a + b;
		}
		public static Vector3 Add(Vector3 a, Point b)
		{
			return a + b;
		}

		public static Vector3 operator +(Vector3 a, Vector3 b)
		{
			return new Vector3(a.x + b.x,
			                   a.y + b.y,
			                   a.z + b.z);
		}
		public static Vector3 operator +(Vector3 a, Point b)
		{
			return new Vector3(a.x + b.X,
								a.y + b.Y,
								a.z);
		}

		public static Vector3 operator -(Vector3 a, Vector3 b)
		{
			return new Vector3(a.x - b.x,
							   a.y - b.y,
							   a.z - b.z);
		}

		public static Vector3 operator /(Vector3 a, Vector3 b)
		{
			return new Vector3(a.x / b.x,
								a.y / b.y,
								a.z / b.z);
		}

		public static Vector3 operator /(Vector3 a, int b)
		{
			return new Vector3(a.x / b,
								a.y / b,
								a.z / b);
		}

		public static Vector3 operator *(Vector3 a, int b)
		{
			return new Vector3(a.x * b,
								a.y * b,
								a.z * b);
		}

		public double SquaredDistanceTo(Vector3 target)
		{
			target = target - this;
			return (target.x * target.x) +
				(target.y * target.y) +
				(target.z * target.z);
		}
		
		
		public readonly int x, y, z;

		public override Boolean Equals(object obj)
		{
			if (obj is Vector3)
			{
				Vector3 v = (Vector3)obj;

				return this.x == v.x && this.y == v.y && this.z == v.z;
			}
			else
			{
				return false;
			}
		}

		bool IEquatable<Vector3>.Equals(Vector3 other)
		{
			return (this.x == other.x && this.y == other.y && this.z == other.z);
		}

		public bool IntersectsWith(Rectangle r)
		{
			return this.x >= r.Left && this.x < r.Right && this.y >= r.Top && this.y < r.Bottom;
		}

		public bool IntersectsWithEllipse(Vector3 location, Vector3 range)
		{
			Vector3 test = this - location;
			return
				((double)(test.x * test.x) / (range.x * range.x)) +
				((double)(test.y * test.y) / (range.y * range.y)) +
				((double)(test.z * test.z) / (range.z * range.z))
				<= 1;
		}

		public bool IntersectsWithExtents(Vector3 location, Vector3 range)
		{
			Vector3 test = this - location;
			return test.x < range.x && test.y < range.y && test.z < range.z && 
				test.x >= 0 && test.y >= 0 && test.z >= 0;
		}
	}
}
