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
using System.Drawing;

namespace Sharplike.Noise
{
	/// <summary>
	/// Abstract parent class for all noise field generators in Sharplike.Noise.
	/// Serves to both provide a blueprint for one-dimensional noise generators
	/// and a jumping-off point for two- and further-dimensional noise generators.
	/// </summary>
	public abstract class Abstract1DNoise
	{
		protected const String AlreadyGenError = "Cannot change values once the NoiseGenerator has been invoked.";
		protected const String MustGenError = "Must invoke the NoiseGenerator before reading from its data set.";

		
		protected Int32 length = 1;
		protected Double minClamp = 0;
		protected Double maxClamp = 0;
		protected InterpolationMethod interpolationMethod = InterpolationMethod.Linear;

		/// <summary>
		/// Defines the length (X coordinate) of the noise generator's array.
		/// </summary>
		public Int32 Length
		{
			get
			{
				return length;
			}
			set
			{
				if (this.Generated == true)
					throw new InvalidOperationException(AlreadyGenError);
				length = value;
			}
		}

		/// <summary>
		/// Defines the minimum value the noise generator will return.
		/// </summary>
		public Double MinimumClamp
		{
			get
			{
				return minClamp;
			}
			set
			{
				if (this.Generated == true)
					throw new InvalidOperationException(AlreadyGenError);
				minClamp = value;
			}
		}
		
		/// <summary>
		/// Defines the maximum value the noise generator will return.
		/// </summary>
		public Double MaximumClamp
		{
			get
			{
				return maxClamp;
			}
			set
			{
				if (this.Generated == true)
					throw new InvalidOperationException(AlreadyGenError);
				maxClamp = value;
			}
		}

		/// <summary>
		/// Defines the interpolation method that the noise generator will
		/// use if a non-integral point in the noise field is looked up.
		/// </summary>
		public InterpolationMethod InterpolationMethod
		{
			get
			{
				return interpolationMethod;
			}
			set
			{
				interpolationMethod = value;
			}
		}

		/// <summary>
		/// Accessor. Look up an integral position in the noise field.
		/// </summary>
		/// <param name="x">The X-coordinate in the noise field.</param>
		/// <returns>The value at the specified coordinate.</returns>
		public Double this[Int32 x]
		{
			get
			{
				if (this.Generated == false)
					throw new InvalidOperationException(MustGenError);
				return this.GetNoiseValue(x);
			}
		}
		/// <summary>
		/// Accessor. Look up a non-integral position in the noise field
		/// and, if InterpolationMethod is set to anything other than
		/// InterpolationMethod.None, return the interpolated value (else
		/// return the nearest neighbor).
		/// </summary>
		/// <param name="x">The X-coordinate in the noise field.</param>
		/// <returns>The value at the specified coordinate.</returns>
		public Double this[Double x]
		{
			get
			{
				if (this.Generated == false)
					throw new InvalidOperationException(MustGenError);
				return this.GetNoiseValue(x);
			}
		}

		/// <summary>
		/// Defines whether or not the noise field has been generated from
		/// this object.
		/// </summary>
		public Boolean Generated { get; protected set; }

		/// <summary>
		/// Generate the noise field for this object. Necessary before
		/// values may be looked up with this object's indexers.
		/// </summary>
		public abstract void Generate();

		/// <summary>
		/// Accessor. Look up an integral position in the noise field.
		/// </summary>
		/// <param name="x">The X-coordinate in the noise field.</param>
		/// <returns>The value at the specified coordinate.</returns>
		public abstract Double GetNoiseValue(Int32 x);
		/// <summary>
		/// Accessor. Look up a non-integral position in the noise field
		/// and, if InterpolationMethod is set to anything other than
		/// InterpolationMethod.None, return the interpolated value (else
		/// return the nearest neighbor).
		/// </summary>
		/// <param name="x">The X-coordinate in the noise field.</param>
		/// <returns>The value at the specified coordinate.</returns>
		public abstract Double GetNoiseValue(Double x);
	}
}
