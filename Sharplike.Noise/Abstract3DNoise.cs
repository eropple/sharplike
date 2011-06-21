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

namespace Sharplike.Noise
{
	/// <summary>
	/// Serves to both provide a blueprint for three-dimensional noise generators
	/// and a jumping-off point for four-dimensional noise generators.
	/// </summary>
	public abstract class Abstract3DNoise : Abstract2DNoise
	{
		protected Int32 depth = 1;

		/// <summary>
		/// Defines the depth (Z coordinate) of the noise generator's array.
		/// </summary>
		public Int32 Depth
		{
			get
			{
				return depth;
			}
			set
			{
				if (this.Generated == true)
					throw new InvalidOperationException(AlreadyGenError);
				depth = value;
			}
		}

		/// <summary>
		/// Accessor. Look up an integral position in the noise field.
		/// </summary>
		/// <param name="x">The X-coordinate in the noise field.</param>
		/// <param name="y">The Y-coordinate in the noise field.</param>
		/// <param name="z">The Z-coordinate in the noise field.</param>
		/// <returns>The value at the specified coordinate.</returns>
		public Double this[Int32 x, Int32 y, Int32 z]
		{
			get
			{
				if (this.Generated == false)
					throw new InvalidOperationException(MustGenError);
				return this.GetNoiseValue(x, y, z);
			}
		}
		/// <summary>
		/// Accessor. Look up a non-integral position in the noise field
		/// and, if InterpolationMethod is set to anything other than
		/// InterpolationMethod.None, return the interpolated value (else
		/// return the nearest neighbor).
		/// </summary>
		/// <param name="x">The X-coordinate in the noise field.</param>
		/// <param name="y">The Y-coordinate in the noise field.</param>
		/// <param name="z">The Z-coordinate in the noise field.</param>
		/// <returns>The value at the specified coordinate.</returns>
		public Double this[Double x, Double y, Double z]
		{
			get
			{
				if (this.Generated == false)
					throw new InvalidOperationException(MustGenError);
				return this.GetNoiseValue(x, y, z);
			}
		}


		/// <summary>
		/// Accessor. Look up an integral position in the noise field.
		/// </summary>
		/// <param name="x">The X-coordinate in the noise field.</param>
		/// <param name="y">The Y-coordinate in the noise field.</param>
		/// <param name="z">The Z-coordinate in the noise field.</param>
		/// <returns>The value at the specified coordinate.</returns>
		public abstract Double GetNoiseValue(Int32 x, Int32 y, Int32 z);
		/// <summary>
		/// Accessor. Look up a non-integral position in the noise field
		/// and, if InterpolationMethod is set to anything other than
		/// InterpolationMethod.None, return the interpolated value (else
		/// return the nearest neighbor).
		/// </summary>
		/// <param name="x">The X-coordinate in the noise field.</param>
		/// <param name="y">The Y-coordinate in the noise field.</param>
		/// <param name="z">The Z-coordinate in the noise field.</param>
		/// <returns>The value at the specified coordinate.</returns>
		public abstract Double GetNoiseValue(Double x, Double y, Double z);
	}
}
