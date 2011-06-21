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
namespace Sharplike.Noise
{
	
	
	public class PerlinNoise
	{
		Double m_persistence;
		Int32 m_octaves, sx, sy, sz, sw;
		internal Interpolator interp;
		
		Double [,,,] data;
		
		public PerlinNoise(Int32 octaves, Double persistence, Interpolator i, Int32 x, Int32 y, Int32 z, Int32 w)
		{
			m_octaves = octaves;
			m_persistence = persistence;
			
			interp = i;
			sx = x;
			sy = y;
			sz = z;
			sw = w;
			
			data = new Double[x,y,z,w];
		}
		
		public Double GetValue(Int32 x, Int32 y, Int32 z, Int32 w)
		{
			return data[x,y,z,w];	
		}
		
		public void Generate()
		{
			Random rnd = new Random();
			
			for (Int32 o = 1; o <= m_octaves; o++) {
				Double[,,,] odata = new Double[sx,sy,sz,sw];
				for (Int32 dx = 0; dx < sx; dx+=o) {
					for (Int32 dy = 0; dy < sy; dy+=o) {
						for (Int32 dz = 0; dz < sz; dz+=o) {
							for (Int32 dw = 0; dw < sw; dw+=o) {
								odata[dx,dy,dz,dw] = rnd.NextDouble();
								
//								System.Console.WriteLine("odata: "+odata[dx,dy,dz,dw]);
							}
						}
					}
				}
				
				for (Int32 dx = 0; dx < sx; dx++) {
					Int32 ptx = dx / o;
					for (Int32 dy = 0; dy < sy; dy++) {
						Int32 pty = dy / o;
						for (Int32 dz = 0; dz < sz; dz++) {
							Int32 ptz = dz / o;
							for (Int32 dw = 0; dw < sw; dw++) {
								Int32 ptw = dw / o;
								
								Double fx = (Double)(dx - (ptx*o)) / (Double)o;
								Double fy = (Double)(dy - (pty*o)) / (Double)o;
								Double fz = (Double)(dz - (ptz*o)) / (Double)o;
								Double fw = (Double)(dw - (ptw*o)) / (Double)o;
								
								try {
									Double vx = interp.Interpolate(odata[ptx*o, pty*o, ptz*o, ptw*o], 
									                               odata[(ptx+1)*o, pty*o, ptz*o, ptw*o], fx);
									Double vy = interp.Interpolate(odata[ptx*o, pty*o, ptz*o, ptw*o], 
									                               odata[ptx*o, (pty+1)*o, ptz*o, ptw*o], fy);
									Double vz = interp.Interpolate(odata[ptx*o, pty*o, ptz*o, ptw*o], 
									                               odata[ptx*o, pty*o, (ptz+1)*o, ptw*o], fz);
									Double vw = interp.Interpolate(odata[ptx*o, pty*o, ptz*o, ptw*o], 
									                               odata[ptx*o, pty*o, ptz*o, (ptw+1)*o], fw);
							
									Double finalval = (vx + vy + vz + vw) / 4; // Not the proper way, but, it's late
								
									data[dx,dy,dz,dw] += finalval *  Math.Pow(m_persistence, m_octaves-o + 1);
									
								} catch (IndexOutOfRangeException) {
									
									data[dx,dy,dz,dw] += odata[ptx*o, pty*o, ptz*o, ptw*o] *  Math.Pow(m_persistence, m_octaves-o + 1);
								}
								
								
							}
						}
					}				
				}
			}
		}
	}
}
