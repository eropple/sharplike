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
using System.IO;
using Mono.Addins;

namespace Sharplike.Editlike.MapTools
{
	public class MapToolExtensionNode : TypeExtensionNode
	{
		[NodeAttribute]
		string icon;

		[NodeAttribute]
		string tooltip;

		[NodeAttribute]
		string tag;

		public String Tooltip
		{
			get { return tooltip; }
		}

		public String Tag
		{
			get { return tag; }
		}

		public Image Icon
		{
			get
			{
				if (toolIcon == null && icon != null)
				{
					using (Stream s = this.Type.Assembly.GetManifestResourceStream(icon))
					{
						toolIcon = Image.FromStream(s);
					}
				}

				return toolIcon;
			}
		}
		private Image toolIcon = null;

		public IMapTool Tool
		{
			get
			{
				if (instance == null)
					instance = (IMapTool)this.CreateInstance();
				return instance;
			}
		}
		private IMapTool instance;
	}
}
