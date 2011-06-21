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
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Sharplike.Core.Rendering;
using Sharplike.UI;
using Sharplike.UI.Controls;
using Sharplike.Mapping;

namespace Sharplike.Editlike.MapTools
{
	public class LineTool : IMapTool
	{
		Main form = null;
		EmptyControl region;

		Point oldend = new Point(0, 0);
		Point start = new Point(0, 0);

		SquareChange change;

		public void SetActive(Main screen, String tag)
		{
			form = screen;
		}

		public void SetInactive()
		{
		}

		public void Start(Point tile)
		{
			region = new EmptyControl(form.Map.Size, new Point(0,0));
			change = new SquareChange(form.Map);

			form.Map.AddRegion(region);

			start = oldend = tile;
		}

		public void End(Point tile)
		{
			if (region == null)
				return;

			EditorExtensionNode node = form.SelectedSquareType();

			if (node != null)
			{
				foreach (Point p in Line(start, tile))
				{
					Vector3 loc = new Vector3(p.X + form.Map.View.x,
						p.Y + form.Map.View.y, form.Map.View.z);
					AbstractSquare sq = (AbstractSquare)node.CreateInstance();
					change.AddOperation(form.Map.GetSafeSquare(loc), sq, loc);
					form.Map.SetSquare(loc,	sq);
				}
			}
			if (change.Count > 0)
				form.UndoRedo.AddChange(change);

			change = null;

			region.InvalidateTiles();

			region.Dispose();
			region = null;

			form.Map.ViewFrom(form.Map.View, true);
		}

		public void Run(Point tile)
		{
			if (tile != oldend)
			{
				region.Clear();

				Rectangle regionExtents = new Rectangle(new Point(0, 0), region.Size);
				foreach (Point p in Line(start, oldend))
				{
					if (regionExtents.Contains(p))
						region.RegionTiles[p.X, p.Y].ClearGlyphs();
				}

				foreach (Point p in Line(start, tile))
				{
					if (regionExtents.Contains(p))
						region.RegionTiles[p.X, p.Y].AddGlyph((int)'*', Color.Green, Color.Transparent);
				}

				oldend = tile;
				region.InvalidateTiles();
			}
		}

		// Algorithm source: http://www.gamedev.net/reference/articles/article1275.asp
		public static IEnumerable<Point> Line(Point start, Point end)
		{
			int deltax = Math.Abs(end.X - start.X);
			int deltay = Math.Abs(end.Y - start.Y);

			int xinc1, xinc2, yinc1, yinc2;

			int x = start.X;
			int y = start.Y;

			if (end.X >= start.X)
				xinc1 = xinc2 = 1;
			else
				xinc1 = xinc2 = -1;

			if (end.Y >= start.Y)
				yinc1 = yinc2 = 1;
			else
				yinc1 = yinc2 = -1;

			int den, num, numadd, numpixels;

			if (deltax >= deltay)
			{
				xinc1 = 0;
				yinc2 = 0;
				den = deltax;
				num = deltax / 2;
				numadd = deltay;
				numpixels = deltax;
			}
			else
			{
				xinc2 = 0;
				yinc1 = 0;
				den = deltay;
				num = deltay / 2;
				numadd = deltax;
				numpixels = deltay;
			}

			for (int curpixel = 0; curpixel <= numpixels; ++curpixel)
			{
				yield return new Point(x, y);

				num += numadd;
				if (num >= den)
				{
					num -= den;
					x += xinc1;
					y += yinc1;
				}
				x += xinc2;
				y += yinc2;
			}
		}
	}
}
