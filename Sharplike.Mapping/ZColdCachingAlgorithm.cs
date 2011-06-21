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

namespace Sharplike.Mapping
{
	public class ZColdCachingAlgorithm : AbstractCachingAlgorithm
	{
		public Int32 ActiveLevel;

		public ZColdCachingAlgorithm(AbstractMap map)
			: base(map)
		{
			ActiveLevel = 0;
		}

		public override void AssessCache()
		{
			List<AbstractPage> pages = new List<AbstractPage>();

			foreach (AbstractPage page in m_map.Pages.Values)
			{
				pages.Add(page);
			}

			foreach (AbstractPage page in pages)
			{
				if (page.address.z == this.ActiveLevel)
				{
					this.SetPageMode(page, CachingMode.Active);
				}
				else
				{
					this.SetPageMode(page, CachingMode.Cold);
				}
			}

		}
	}
}
