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
using Mono.Addins;

namespace Sharplike.Editlike
{
	/// <summary>
	/// A factory class for squares and entities. New factories are defined in the .addin file
	/// for your project.
	/// </summary>
	public class EditorExtensionNode : TypeExtensionNode
	{
		[NodeAttribute]
		string editor;

		[NodeAttribute]
		int gid;

		[NodeAttribute]
		string tooltip;

		/// <summary>
		/// The tooltip for this particular editor extension.
		/// </summary>
		public String TooltipText
		{
			get { return tooltip; }
		}

		/// <summary>
		/// The index of the glyph that will represent this factory in the editor.
		/// </summary>
		public int GlyphID
		{
			get
			{
				return gid;
			}
		}
	}
}
