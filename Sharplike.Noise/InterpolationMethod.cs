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
using System.Text;

namespace Sharplike.Noise
{
	public enum InterpolationMethod
	{
		None,
		Linear,
		Cosine,
		Cubic
	}
	
	public abstract class Interpolator
	{
		public abstract Double Interpolate(Double a, Double b, Double t);
	}
	
	public class LinearInterpolator : Interpolator
	{
		public override Double Interpolate(Double a, Double b, Double t)
		{
			return (a * (1-t) + b * t);
		}
	}
	
	public class ClosestInterpolator : Interpolator
	{
		public override Double Interpolate(Double a, Double b, Double t)
		{
			return a;
		}
	}
}
