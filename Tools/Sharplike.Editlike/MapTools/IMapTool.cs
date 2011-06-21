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
using System.Drawing;
using Sharplike.Mapping;

namespace Sharplike.Editlike.MapTools
{
	public interface IMapTool
	{
		/// <summary>
		/// Called when the tool is selected from the toolbar.
		/// </summary>
		/// <param name="screen">The form that's selecting this tool.</param>
		/// <param name="tag">A user-defined string that was defined in the .addin file.</param>
		void SetActive(Main screen, String tag);

		/// <summary>
		/// Called when the tool is no longer the active tool in the editor.
		/// </summary>
		void SetInactive();

		/// <summary>
		/// Equivilent to a MouseDown event.
		/// </summary>
		/// <param name="tile">The tile where the event started.</param>
		void Start(Point tile);

		/// <summary>
		/// Equivilent to a MouseUp event.
		/// </summary>
		/// <param name="tile">The final location of the tool operation.</param>
		void End(Point tile);

		/// <summary>
		/// Equivilent to a MouseMove event.
		/// </summary>
		/// <param name="tile">The current mouse position. 
		/// This is where the tool operation should be performed.</param>
		void Run(Point tile);
	}

	/// <summary>
	/// SquareChange denotes an undo/redo operation involving the changing of map squares.
	/// </summary>
	/// <remarks>
	/// In general this class should be run as part of a map tool that operates on squares.
	/// It should be constructed in Start() and applied to the undo/redo stack during End().
	/// </remarks>
	public class SquareChange : UndoStack.Change
	{
		/// <summary>
		/// Creates a new change batch targeting the specified map.
		/// </summary>
		/// <param name="map">The map that we're changing squares on.</param>
		public SquareChange(AbstractMap map)
		{
			this.map = map;
			this.OnUndo += new EventHandler<EventArgs>(PenChange_OnUndo);
			this.OnRedo += new EventHandler<EventArgs>(PenChange_OnRedo);
		}

		void PenChange_OnRedo(object sender, EventArgs e)
		{
			foreach (KeyValuePair<Vector3, SquareChanged> kvp in this.changes)
			{
				map.SetSquare(kvp.Key, kvp.Value.newSquare);
			}
			map.ViewFrom(map.View, true);
		}

		void PenChange_OnUndo(object sender, EventArgs e)
		{
			foreach (KeyValuePair<Vector3, SquareChanged> kvp in this.changes)
			{
				map.SetSquare(kvp.Key, kvp.Value.oldSquare);
			}
			map.ViewFrom(map.View, true);
		}

		/// <summary>
		/// Adds a square change operation. This does not actually change the square, but rather stores
		/// the modification for handing in undo and redo.
		/// </summary>
		/// <param name="oldSquare">The square that was previously at the map location.</param>
		/// <param name="newSquare">The new square being placed at the map location.</param>
		/// <param name="location">The location on the map that this operation takes place.</param>
		public void AddOperation(AbstractSquare oldSquare, AbstractSquare newSquare, Vector3 location)
		{
			if (!changes.ContainsKey(location) && oldSquare != newSquare)
			{
				SquareChanged c = new SquareChanged();
				c.oldSquare = oldSquare;
				c.newSquare = newSquare;
				changes.Add(location, c);
			}
		}

		/// <summary>
		/// The number of operations in this change batch.
		/// </summary>
		public int Count
		{
			get { return changes.Count; }
		}

		struct SquareChanged
		{
			public AbstractSquare oldSquare;
			public AbstractSquare newSquare;
		}

		private AbstractMap map;
		private Dictionary<Vector3, SquareChanged> changes = new Dictionary<Vector3, SquareChanged>();
	}
}
