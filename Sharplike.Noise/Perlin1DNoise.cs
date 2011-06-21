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

namespace Sharplike.Noise
{
	public class Perlin1DNoise : Abstract1DNoise
	{
		PerlinNoise noise;
		Int32 octaves;
		Double persistence;

		/// <summary>
		/// Defines the octaves for the noise generator.
		/// </summary>
		public Int32 Octaves
		{
			get
			{
				return octaves;
			}
			set
			{
				if (this.Generated == true)
					throw new InvalidOperationException(AlreadyGenError);
				octaves = value;
			}
		}

		/// <summary>
		/// Defines the persistence value for the noise generator.
		/// </summary>
		public Double Persistence
		{
			get
			{
				return persistence;
			}
			set
			{
				if (this.Generated == true)
					throw new InvalidOperationException(AlreadyGenError);
				persistence = value;
			}
		}

		public override void Generate()
		{
			Interpolator i = null;

			switch (this.InterpolationMethod)
			{
				case Noise.InterpolationMethod.None:
					{
						i = new ClosestInterpolator();
						break;
					}
				case Noise.InterpolationMethod.Linear:
					{
						i = new LinearInterpolator();
						break;
					}
				case Noise.InterpolationMethod.Cubic:
					{
						throw new NotImplementedException();
					}
				case Noise.InterpolationMethod.Cosine:
					{
						throw new NotImplementedException();
					}
			}

			noise = new PerlinNoise(octaves, persistence, i, this.Length, 1, 1, 1); // Use 1 for the unused dimension sizes.
			this.Generated = true;
		}

		public override Double GetNoiseValue(Int32 x)
		{
			if (this.Generated == false)
				throw new InvalidOperationException(MustGenError);
			return noise.GetValue(x, 0, 0, 0);
		}
		public override Double GetNoiseValue(Double x)
		{
			if (this.Generated == false)
				throw new InvalidOperationException(MustGenError);

			Double d = noise.interp.Interpolate(noise.GetValue((int)Math.Floor(x), 0, 0, 0),
												noise.GetValue((int)Math.Ceiling(x), 0, 0, 0),
												x - Math.Floor(x));

			return d;
		}
	}
}
