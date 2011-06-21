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
using Sharplike.Mapping;

namespace Sharplike.Editlike.MapTools
{
	public class ViewportTool : IMapTool
	{
		Main form = null;
		Point prevloc;

		public void SetActive(Main screen, String tag)
		{
			form = screen;
		}
		public void SetInactive()
		{
		}

		public void Start(Point tile)
		{
			prevloc = tile;
		}

		public void End(Point tile)
		{
		}

		public void Run(Point tile)
		{
			form.Map.View = form.Map.View + (new Vector3(prevloc.X - tile.X, prevloc.Y - tile.Y, 0));
			prevloc = tile;
		}
	}
}
