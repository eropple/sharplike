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
using System.IO;
using Sharplike.Core;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;

namespace Sharplike.Mapping
{
	[Serializable]
	public enum CachingMode
	{
		Active,
		Warm,
		Cold
	}

	public abstract class AbstractCachingAlgorithm
	{
		protected AbstractMap m_map;

		public AbstractCachingAlgorithm(AbstractMap map)
		{
			m_map = map;
		}

		public void SetPageMode(AbstractPage page, CachingMode mode)
		{
			if (page.cacheMode == mode) return;
			SetPageMode(page.address, mode);
		}

		public void SetPageMode(Vector3 addr, CachingMode mode)
		{
			if (mode == CachingMode.Cold)
			{
				AbstractPage page = m_map.GetPage(addr);
				if (page == null) throw new ArgumentException("No uncached page at this address.");
				Cache(page);
			}
			else
			{
				try
				{
					AbstractPage coldpage = m_map.GetPage(addr);
					AbstractPage page = Wake(addr);
					page.cacheMode = mode;
					if (coldpage != null)
					{
						m_map.RemovePage(coldpage);
					}
					m_map.AddPage(page, addr);
				}
				catch (System.IO.FileNotFoundException)
				{
					throw new ArgumentException("No cached page at this address.");
				}
			}


		}

		public void Cache(AbstractPage page)
		{
			Console.WriteLine("Caching " + page.address.ToString());
			FileStream file = new FileStream(Game.PathTo("cache/" + CacheName(page.address) + ".dat"), FileMode.Create);
			BinaryFormatter f = new BinaryFormatter();
			f.Serialize(file, page);
			file.Close();
			m_map.RemovePage(page);
			m_map.AddPage(new ColdPage(m_map.PageSize), page.address);
		}

		public AbstractPage Wake(Vector3 addr)
		{
			Console.WriteLine("Waking " + addr.ToString());
			string path = Game.PathTo("cache/" + CacheName(addr) + ".dat");
			FileStream file = new FileStream(path, FileMode.Open);
			BinaryFormatter f = new BinaryFormatter();
			AbstractPage page = (AbstractPage)f.Deserialize(file);
			file.Close();
			File.Delete(path);
			return page;
		}

		public string CacheName(Vector3 addr)
		{
			byte[] data = UnicodeEncoding.Unicode.GetBytes(addr.ToString() + m_map.Name);
			byte[] hash = new SHA1CryptoServiceProvider().ComputeHash(data);
			StringBuilder hex = new StringBuilder(hash.Length);
			foreach (byte b in data)
			{
				hex.Append(b.ToString("X2"));
			}
			return hex.ToString();
		}


		public abstract void AssessCache();

	}
}
