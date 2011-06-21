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
using Sharplike.Core;
using System.IO;
using Sharplike.Core.Rendering;
using Sharplike.Core.Messaging;
using Sharplike.Core.Scheduling;
using Sharplike.Mapping.Entities;
using System.Runtime.Serialization.Formatters.Binary;

namespace Sharplike.Mapping
{
	[ChannelSubscriber("Maps")]
	[Serializable]
	public abstract class AbstractMap : AbstractRegion, IMessageReceiver
	{
		Vector3 pageSize;
		public readonly Dictionary<Vector3, AbstractPage> Pages;
		Vector3 view;
		public readonly string Name;

		private IScheduler scheduler;

		public AbstractMap(Size displayRegionSize, Vector3 pageSize, string Name)
            : base(displayRegionSize, new Point(0,0))
		{
			this.pageSize = pageSize;
			this.Name = Name;

			Game.Scheduler = new SimpleThreadPoolScheduler();

			Pages = new Dictionary<Vector3, AbstractPage>();

			MessageHandler.SetHandler("LookAt", Message_LookAt);
			MessageHandler.SetHandler("ViewFrom", Message_ViewFrom);
			MessageHandler.SetHandler("RepositionEntity", Message_RepositionEntity);
		}

		#region Message Handler Functions
		[MessageArgument(0, typeof(AbstractEntity))]
		void Message_LookAt(Message m)
		{
			ViewFrom((AbstractEntity)m.Args[0]);
		}

		[MessageArgument(0, typeof(Vector3))]
		void Message_ViewFrom(Message m)
		{
			ViewFrom((Vector3)m.Args[0]);
		}

		[MessageArgument(0, typeof(AbstractEntity))]
		[MessageArgument(1, typeof(Vector3))]
		[MessageArgument(2, typeof(Vector3))]
		void Message_RepositionEntity(Message m)
		{
			RepositionEntity((AbstractEntity)m.Args[0], (Vector3)m.Args[1], (Vector3)m.Args[2]);
		}
		#endregion


		public static T Deserialize<T>(Stream file) where T : AbstractMap
		{
			BinaryFormatter f = new BinaryFormatter();
			return (T)f.Deserialize(file);
		}

		public void Serialize(Stream file)
		{
			BinaryFormatter f = new BinaryFormatter();
			f.Serialize(file, this);
		}

		/// <summary>
		/// Get operations are thread safe. Set operations are not.
		/// Gets or sets the square to be used at a particular map coordinate.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		/// <returns>The square at the specified coordinate.</returns>
		public virtual AbstractSquare this[Int32 x, Int32 y, Int32 z]
		{
			get
			{
				return this.GetSafeSquare(new Vector3(x,y,z));
			}
			protected set
			{

				SetSquare(new Vector3(x, y, z), value);
			}
		}

		/// <summary>
		/// Thread safe. Gets the size of pages that the map uses.
		/// </summary>
		public Vector3 PageSize
		{
			get
			{
				return pageSize;
			}
		}

		/// <summary>
		/// Thread safe. Gets the current position and size of the viewport.
		/// </summary>
		public Rectangle Viewport
		{
			get
			{
				return new Rectangle(new Point(view.x, view.y), this.Size);
			}
		}

		public void RemovePage(AbstractPage page)
		{
			List<Vector3> keys = new List<Vector3>();
			foreach (KeyValuePair<Vector3, AbstractPage> pair in Pages)
			{
				if (pair.Value == page) keys.Add(pair.Key);
			}
			foreach (Vector3 key in keys)
			{
				Pages.Remove(key);
			}
		}
		
		public void AddPage(AbstractPage newPage, Vector3 pageLocation)
		{
			if (newPage.Size.Equals(this.pageSize) == false)
				throw new ArgumentException("All pages must be the size specified to the Map object.");

			if (Pages.ContainsKey(pageLocation)) throw new ArgumentException("Page already exists at " + pageLocation.ToString() + ".");
			/*
			Vector3 n = new Vector3(here.x, here.y - 1, here.z);
			Vector3 s = new Vector3(here.x, here.y + 1, here.z);
			Vector3 e = new Vector3(here.x + 1, here.y, here.z);
			Vector3 w = new Vector3(here.x - 1, here.y, here.z);
			Vector3 u = new Vector3(here.x, here.y, here.z - 1);
			Vector3 d = new Vector3(here.x, here.y, here.z + 1);
			bool valid = pages.Count == 0 ||
				(false &&
				(pages.ContainsKey(n) ||
				 pages.ContainsKey(s) ||
				 pages.ContainsKey(e) ||
				 pages.ContainsKey(w) ||
				 pages.ContainsKey(u) ||
				 pages.ContainsKey(d)));
			if (!valid)
			{
				throw new ArgumentException("Provided coordinates for new page are not contiguous with existing pages.");
			}
			 */
			newPage.parentMap = this;
			newPage.address = pageLocation;

			Game.Scheduler.AddTask(newPage);

			Pages.Add(pageLocation, newPage);
            newPage.Build();
		}

		public AbstractPage GetPage(Vector3 addr)
		{
			if (!Pages.ContainsKey(addr)) return null;
			return this.Pages[addr];
		}

		public AbstractSquare GetSquare(AbstractPage p, Vector3 offset)
		{
			Vector3 npos = new Vector3(p.address.x * pageSize.x + offset.x,
										   p.address.y * pageSize.y + offset.y,
										   p.address.z * pageSize.z + offset.z);
			return GetSquare(npos);
		}

		public AbstractSquare GetSquare(Vector3 location)
		{
			Vector3 addr;
			Vector3 newoff;
			Vector3.Divide(location, this.pageSize, out addr, out newoff);
			if (!Pages.ContainsKey(addr)) throw new ArgumentException("Specified square not in map.");
			return Pages[addr].GetSquare(newoff.x, newoff.y, newoff.z);
		}

		public void SetSquare(Vector3 location, AbstractSquare square)
		{
			Vector3 addr;
			Vector3 newoff;
			Vector3.Divide(location, this.pageSize, out addr, out newoff);
			if (!Pages.ContainsKey(addr))
			{
				AddPage(new BasicPage(pageSize), addr);
			}

			Pages[addr].SetSquare(newoff.x, newoff.y, newoff.z, square);

			if (location.z == view.z)
			{
				if (Viewport.Contains(new Point(location.x, location.y)))
				{
					InvalidateTiles(new Rectangle(location.x - view.x, location.y - view.y, 1, 1));
				}
			}

			if (square != null)
			{
				square.Map = this;
				square.Location = location;
			}
		}

		/// <summary>
		/// Thread safe. Get the square at the specified map location.
		/// </summary>
		/// <param name="location">The coordinates on the map to fetch the square from.</param>
		/// <returns>The square if one exists, or null if no square has been assigned there.</returns>
		public AbstractSquare GetSafeSquare(Vector3 location)
		{
			Vector3 addr;
			Vector3 newoff;

			lock (this)
			{
				Vector3.Divide(location, this.pageSize, out addr, out newoff);

				AbstractPage p;
				if (!Pages.TryGetValue(addr, out p))
					return null;
				return p.GetSquare(newoff.x, newoff.y, newoff.z);
			}
		}


		public Vector3 GetVisibleSquareLocation(AbstractSquare square)
		{
			lock (this)
			{
				foreach (AbstractPage page in this.GetPagesInRange(this.View, 
					new Vector3(this.Viewport.Width, this.Viewport.Height, 1)))
				{
					for (int y = 0; y < pageSize.y; ++y)
					{
						for (int x = 0; x < pageSize.x; ++x)
						{
							Vector3 location = new Vector3(x, y, this.View.z);
							if (page[location].Equals(square))
								return location;
						}
					}
				}
			}
			return Vector3.Zero;
		}

		public Vector3 View
		{
			get { return view; }
			set { ViewFrom(value); }
		}

		/// <summary>
		/// Sets the viewport to look at a specific entity on the map.
		/// This will center the entity on the screen.
		/// </summary>
		/// <param name="ent">The entity to view from.</param>
		public void ViewFrom(AbstractEntity ent)
        {
            int x = ent.Location.x - (Size.Width / 2);
            int y = ent.Location.y - (Size.Height / 2);
            int z = ent.Location.z;
            ViewFrom(new Vector3(x, y, z));
        }

		/// <summary>
		/// Sets the viewport to a specific location on the map.
		/// </summary>
		/// <param name="nView">The map coordinates of the top-left most square to view from.</param>
		public void ViewFrom(Vector3 nView) { this.ViewFrom(nView, false); }

		/// <summary>
		/// Sets the viewport to a specific location on the map.
		/// </summary>
		/// <param name="nView">The coordinates of the top-left most square to view from.</param>
		/// <param name="forceRender">Whether or not to force a redraw of the viewport.
		/// This only has an effect when the viewport doesn't change.</param>
		public void ViewFrom(Vector3 nView, Boolean forceRender)
		{
			if (nView.Equals(view) == false || forceRender == true)
			{
                this.Clear();
				
				for (int x = 0; x < this.Size.Width; x++)
				{
					int gx = x + nView.x;
					for (int y = 0; y < this.Size.Height; y++)
					{
						int gy = y + nView.y;
                        this.RegionTiles[x, y].Reset();
						
						AbstractSquare sq = this.GetSafeSquare(new Vector3(gx, gy, nView.z));
						if (sq != null)
						{
                            this.RegionTiles[x, y].AddGlyphProvider(sq);
						}
						else
						{
                            //Draw a nice red error tile if we're looking out of bounds in debug mode,
                            //but just black if we're in release mode.
#if DEBUG
							int n = 249; // circle
							if (gx % 2 == 0 && gy % 2 == 0) n = 197; // +
							if (gx % 2 != 0 && gy % 2 == 0) n = 196; // -
							if (gx % 2 == 0 && gy % 2 != 0) n = 179; // |

                            this.RegionTiles[x, y].AddGlyphProvider(new ErrorSquare(n));
#else
                            this.RegionTiles[x, y].AddGlyphProvider(new EmptySquare());
#endif
						}

					}
				}

				view = nView;

                Vector3 addr;
                Vector3 newoff;
                Vector3.Divide(nView, this.pageSize, out addr, out newoff);

				foreach (AbstractPage p in GetPagesInRange(nView, new Vector3(this.Size.Width, this.Size.Height, 1)))
				{
					foreach (AbstractEntity ent in p.Entities)
					{
						if (ent.Location.IntersectsWith(this.Viewport) && ent.Location.z == view.z)
						{
							this.RegionTiles[ent.Location.x - view.x,
								ent.Location.y - view.y].AddGlyphProvider(ent);
						}
					}
				}
				
			}
		}

		AbstractPage[] GetPagesInRange(Vector3 start, Vector3 extents)
		{
			Vector3 addr;
			Vector3 newoff;
			Vector3.Divide(start, this.pageSize, out addr, out newoff);

			List<AbstractPage> ret = new List<AbstractPage>();

			for (int x = addr.x; x <= addr.x + Math.Ceiling((double)extents.x / pageSize.x); ++x)
			{
				for (int y = addr.y; y <= addr.y + Math.Ceiling((double)extents.y / pageSize.y); ++y)
				{
					for (int z = addr.z; z <= addr.z + Math.Ceiling((double)extents.z / pageSize.z); ++z)
					{
						AbstractPage p;
						if (Pages.TryGetValue(new Vector3(x, y, z), out p))
							ret.Add(p);
					}
				}
			}

			return ret.ToArray();
		}

		bool IsPageVisible(AbstractPage p)
		{
			Vector3 worldloc = new Vector3(
				p.address.x * pageSize.x,
				p.address.y * pageSize.y,
				p.address.z * pageSize.z);
			Rectangle pagerect = new Rectangle(new Point(worldloc.x, worldloc.y), new Size(p.Size.x, p.Size.y));
			return pagerect.IntersectsWith(this.Viewport) && view.z >= worldloc.z && view.z < worldloc.z + pageSize.z;
		}

		#region Entity Operations
		internal void AddEntity(AbstractEntity ent)
		{
			Vector3 addr;
			Vector3 newoff;
			Vector3.Divide(ent.Location, this.pageSize, out addr, out newoff);

			if (ent.Location.IntersectsWith(this.Viewport) && ent.Location.z == view.z)
				this.RegionTiles[ent.Location.x - view.x, ent.Location.y - view.y].AddGlyphProvider(ent);

			AbstractPage p = Pages[addr];
			if (p != null)
				p.Entities.Add(ent);
		}

		internal void RemoveEntity(AbstractEntity ent)
		{
			Vector3 addr;
			Vector3 newoff;
			Vector3.Divide(ent.Location, this.pageSize, out addr, out newoff);

			if (ent.Location.IntersectsWith(this.Viewport) && ent.Location.z == view.z)
			{
				this.RegionTiles[ent.Location.x - view.x, ent.Location.y - view.y].RemoveGlyphProvider(ent);
			}

			AbstractPage p = Pages[addr];
			if (p != null)
				p.Entities.Remove(ent);
		}

		public void SwapEntityCallbackOwner(AbstractEntity ent, AbstractPage oldpage, AbstractPage newpage)
		{
			List<PageCallbackInfo> pci = oldpage.RemoveAllAIDelegates(ent);
			foreach (PageCallbackInfo callback in pci)
			{
				newpage.RegisterAIDelegate(callback);
			}
		}

		internal bool RepositionEntity(AbstractEntity ent, Vector3 oldloc, Vector3 newloc)
		{
			Vector3 oldpageaddr, newpageaddr;
			Vector3 oldpageloc, newpageloc;
			Vector3.Divide(oldloc, this.pageSize, out oldpageaddr, out oldpageloc);
			Vector3.Divide(newloc, this.pageSize, out newpageaddr, out newpageloc);

			AbstractPage oldpage;
			AbstractPage newpage;

			if (Pages.TryGetValue(newpageaddr, out newpage))
			{
				if (Pages.TryGetValue(oldpageaddr, out oldpage))
				{
					if (oldloc.IntersectsWith(this.Viewport) && oldloc.z == view.z)
					{
						int x = oldloc.x - view.x;
						int y = oldloc.y - view.y;
						this.RegionTiles[x, y].RemoveGlyphProvider(ent);
					}
					if (!oldpageaddr.Equals(newpageaddr)) 
					{
						SwapEntityCallbackOwner(ent, oldpage, newpage);
						oldpage.Entities.Remove(ent);
					}
				}

				if (newloc.IntersectsWith(this.Viewport) && ent.Location.z == view.z)
					this.RegionTiles[newloc.x - view.x, newloc.y - view.y].AddGlyphProvider(ent);
				if (!oldpageaddr.Equals(newpageaddr))
				{
					newpage.Entities.Add(ent);
				}

				return true;
			}

			return false;
		}

		/// <summary>
		/// Thread safe. Gets all entities within a specified distance from a specified point.
		/// </summary>
		/// <param name="location">The center of the extents to search.</param>
		/// <param name="range">The ellipsoid extents to search.</param>
		/// <returns>An array of all entities found within the specified extents.</returns>
		public AbstractEntity[] EntitiesInElipticalRange(Vector3 location, Vector3 range)
		{
			List<AbstractEntity> ret = new List<AbstractEntity>();
			lock (this)
			{
				Vector3 topleft = location - range;
				Vector3 extents = (range * 2);
				foreach (AbstractPage p in GetPagesInRange(topleft, extents))
				{
					foreach (AbstractEntity ent in p.Entities)
					{
						if (ent.Location.IntersectsWithEllipse(location, range))
						{
							ret.Add(ent);
						}
					}
				}
			}
			return ret.ToArray();
		}

		/// <summary>
		/// Thread safe. Gets all entities within a specified distance from a specified point.
		/// </summary>
		/// <param name="location">The center of the extents to search.</param>
		/// <param name="range">The rectangular extents to search.</param>
		/// <returns>An array of all entities found within the specified extents.</returns>
		public AbstractEntity[] EntitiesInRectangularRange(Vector3 location, Vector3 range)
		{
			List<AbstractEntity> ret = new List<AbstractEntity>();
			lock (this)
			{
				Vector3 topleft = location - range;
				Vector3 extents = (range * 2);
				foreach (AbstractPage p in GetPagesInRange(topleft, extents))
				{
					foreach (AbstractEntity ent in p.Entities)
					{
						if (ent.Location.IntersectsWithExtents(location, range))
						{
							ret.Add(ent);
						}
					}
				}
			}
			return ret.ToArray();
		}

		/// <summary>
		/// Thread Safe. Broadcasts a messages to all entities within a given range from the specified point.
		/// </summary>
		/// <param name="location">The center of the ellipsoid to broadcast to.</param>
		/// <param name="range">The extents of the ellipsoid to broadcast within.</param>
		/// <param name="message">The message to broadcast.</param>
		/// <returns>All entities that received the message.</returns>
		public AbstractEntity[] BroadcastMessage(Vector3 location, Vector3 range, String message)
		{
			return BroadcastMessage(location, range, message, new Object[0]);
		}

		/// <summary>
		/// Thread Safe. Broadcasts a messages to all entities within a given range from the specified point.
		/// </summary>
		/// <param name="location">The center of the ellipsoid to broadcast to.</param>
		/// <param name="range">The extents of the ellipsoid to broadcast within.</param>
		/// <param name="message">The message to broadcast.</param>
		/// <param name="args">Additional arguments that this message requires.</param>
		/// <returns>All entities that received the message.</returns>
		public AbstractEntity[] BroadcastMessage(Vector3 location, Vector3 range, String message, params Object[] args)
		{
			AbstractEntity[] ents = EntitiesInElipticalRange(location, range);

			foreach (AbstractEntity ent in ents)
			{
				Game.SendMessage(ent, message, args);
			}

			return ents;
		}

		/// <summary>
		/// Broadcasts a message only to the single nearest entity.
		/// </summary>
		/// <param name="location">The center of the ellipsoid to broadcast to.</param>
		/// <param name="maxrange">The extents of the ellipsoid to broadcast within.</param>
		/// <param name="message">The message to broadcast.</param>
		/// <returns>The entity that received the message, or null if no other entity existed within the range.</returns>
		public AbstractEntity BroadcastToNearest(Vector3 location, Vector3 maxrange, String message)
		{
			return BroadcastToNearest(location, maxrange, message, new Object[0]);
		}

		/// <summary>
		/// Broadcasts a message only to the single nearest entity.
		/// </summary>
		/// <param name="location">The center of the ellipsoid to broadcast to.</param>
		/// <param name="maxrange">The extents of the ellipsoid to broadcast within.</param>
		/// <param name="message">The message to broadcast.</param>
		/// <param name="args">Additional arguments for the message.</param>
		/// <returns>The entity that received the message, or null if no other entity existed within the range.</returns>
		public AbstractEntity BroadcastToNearest(Vector3 location, Vector3 maxrange, String message, params Object[] args)
		{
			List<AbstractEntity> ents = new List<AbstractEntity>(EntitiesInElipticalRange(location, maxrange));
			ents.Sort(delegate(AbstractEntity a, AbstractEntity b)
			{
				return a.Location.SquaredDistanceTo(location) > b.Location.SquaredDistanceTo(location) ? 1 : -1;
			});
			if (ents.Count > 0)
				return ents[0];
			return null;
		}

		/// <summary>
		/// Broadcasts a message only to the single nearest entity.
		/// </summary>
		/// <param name="location">The center of the ellipsoid to broadcast to.</param>
		/// <param name="maxrange">The extents of the ellipsoid to broadcast within.</param>
		/// <param name="exclude">One entity to exclude from the broadcast.</param>
		/// <param name="message">The message to broadcast.</param>
		/// <returns>The entity that received the message, or null if no other entity existed within the range.</returns>
		public AbstractEntity BroadcastToNearest(Vector3 location, Vector3 maxrange, AbstractEntity exclude, String message)
		{
			return BroadcastToNearest(location, maxrange, exclude, message, new Object[0]);
		}

		/// <summary>
		/// Broadcasts a message only to the single nearest entity.
		/// </summary>
		/// <param name="location">The center of the ellipsoid to broadcast to.</param>
		/// <param name="maxrange">The extents of the ellipsoid to broadcast within.</param>
		/// <param name="exclude">One entity to exclude from the broadcast.</param>
		/// <param name="message">The message to broadcast.</param>
		/// <param name="args">Additional arguments for the message.</param>
		/// <returns>The entity that received the message, or null if no other entity existed within the range.</returns>
		public AbstractEntity BroadcastToNearest(Vector3 location, Vector3 maxrange, AbstractEntity exclude, String message, params Object[] args)
		{
			List<AbstractEntity> ents = new List<AbstractEntity>(EntitiesInElipticalRange(location, maxrange));
			ents.Remove(exclude);
			ents.Sort(delegate(AbstractEntity a, AbstractEntity b)
			{
				return a.Location.SquaredDistanceTo(location) > b.Location.SquaredDistanceTo(location) ? 1 : -1;
			});
			if (ents.Count > 0)
				return ents[0];
			return null;
		}
		#endregion

		public void OnMessage(Message msg)
		{
			MessageHandler.HandleMessage(msg);
		}

		public void AssertArgumentTypes(Message msg)
		{
			MessageHandler.AssertArgumentTypes(msg);
		}
		public readonly MessageHandler MessageHandler = new MessageHandler();
	}
}