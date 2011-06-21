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

namespace Sharplike.UI
{
	/// <summary>
	/// A map for quick access to the RenderSystem's Glyphs by other
	/// names.
	/// </summary>
	public partial class GlyphMap
	{
		private Dictionary<Object, Int32> map;

		public GlyphMap()
		{
			map = new Dictionary<Object, Int32>();
			GlyphMap.LoadDefaults(this);
		}

		public void Register(Object key, Int32 glyphIndex)
		{
			if (map.ContainsKey(key) == true)
				map.Remove(key);

			map.Add(key, glyphIndex);
		}
		public Int32 Get(Object key)
		{
			if (map.ContainsKey(key) == false)
				throw new ArgumentOutOfRangeException("Key '" + key.ToString() + "' not found.");
			return map[key];
		}
		public void Clear()
		{
			map.Clear();
		}

		/// <summary>
		/// Gets or sets the numeric glyph index associated with a given
		/// object key. This key is intentionally typeless, but a standard
		/// GlyphMap comes preloaded with all glyphs in the GlyphDefaults
		/// enumeration, as well as most English glyphs as Char types.
		/// </summary>
		/// <param name="key">The object key for searching.</param>
		/// <returns>The glyph index corresponding to the specified object.</returns>
		public Int32 this[Object key]
		{
			get
			{
				return this.Get(key);
			}
			set
			{
				this.Register(key, value);
			}
		}
	}
}
