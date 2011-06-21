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

namespace Sharplike.Core.Rendering
{
    public abstract class AbstractRenderSystem : IDisposable
    {
		public abstract AbstractWindow CreateWindow(Size displayDimensions, GlyphPalette palette, Object context);

        public AbstractWindow CreateWindow(Size displayDimensions, GlyphPalette palette)
		{
			return CreateWindow(displayDimensions, palette, null);
		}
        public abstract AbstractWindow Window
        {
            get;
        }
        public abstract void Process();

        public abstract void Dispose();
    }
}
