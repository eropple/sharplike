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
using Sharplike.Mapping.Entities;

namespace Sharplike.Editlike.MapTools
{
	public class CursorTool : IMapTool
	{
		Main form;
		AbstractEntity ent;
		public void SetActive(Main screen, String tag)
		{
			form = screen;
		}

		public void End(Point tile)
		{
			form.EntityProperties.SelectedObject = ent;
			ent = null;
		}

		public void SetInactive()
		{
			form = null;
		}

		public void Start(Point tile)
		{
			Vector3 location = new Vector3(tile.X + form.Map.View.x,
				tile.Y + form.Map.View.y, form.Map.View.z);
			Vector3 extents = new Vector3(1, 1, 1);
			AbstractEntity[] ents = form.Map.EntitiesInRectangularRange(location, extents);

			if (ents.Length > 0)
				ent = ents[0];


			form.EntityProperties.SelectedObject = ent;
		}

		public void Run(Point tile)
		{
			if (ent != null)
			{
				Vector3 location = new Vector3(tile.X + form.Map.View.x,
					tile.Y + form.Map.View.y, form.Map.View.z);
				ent.Location = location;
			}
		}
	}
}
