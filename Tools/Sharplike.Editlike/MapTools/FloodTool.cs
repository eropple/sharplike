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
using Sharplike.UI;
using Sharplike.UI.Controls;

namespace Sharplike.Editlike.MapTools
{
	public class FloodTool : IMapTool
	{
		Main form = null;
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
			change = new SquareChange(form.Map);
		}

		public void End(Point tile)
		{
			Vector3 start = new Vector3(tile.X + form.Map.View.x,
				tile.Y + form.Map.View.y, form.Map.View.z);
			EditorExtensionNode node = form.SelectedSquareType();
			AbstractSquare sq = form.Map.GetSafeSquare(start);
			if (sq != null && node != null && !sq.GetType().Equals(node.Type))
			{
				foreach (Vector3 v in Flood(form.Map, start))
				{
					AbstractSquare square = (AbstractSquare)node.CreateInstance();
					change.AddOperation(form.Map.GetSafeSquare(v), square, v);
					form.Map.SetSquare(v, square);
				}
			}
			form.Map.ViewFrom(form.Map.View, true);
			if (change.Count > 0)
				form.UndoRedo.AddChange(change);
			change = null;
		}

		public void Run(Point tile)
		{
		}

		public static IEnumerable<Vector3> Flood(AbstractMap map, Vector3 location)
		{
			Type targetType = map.GetSafeSquare(location).GetType();
			Queue<Vector3> candidates = new Queue<Vector3>();
			candidates.Enqueue(location);
			while (candidates.Count > 0)
			{
				Vector3 n = candidates.Dequeue();
				Vector3 delta = Vector3.West;
				Vector3 w = n;
				for (int x = 0; x < 2; ++x)
				{
					bool ok = false;
					do
					{
						AbstractSquare sq = map.GetSafeSquare(w);
						ok = (sq != null && sq.GetType().Equals(targetType));
						if (ok)
						{
							yield return w;

							candidates.Enqueue(w + Vector3.North);
							candidates.Enqueue(w + Vector3.South);
						}
						w = w + delta;
					} while (ok);

					delta = Vector3.East;
					w = n + delta;
				}
			}
		}
	}
}
