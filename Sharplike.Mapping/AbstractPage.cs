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
using System.Drawing;
using Sharplike.Core;
using Sharplike.Core.Scheduling;
using Sharplike.Mapping.Entities;

namespace Sharplike.Mapping
{
	public enum SleepMode
	{
		Active, 
		Slow, 
		Cached 
	}
	
	[Serializable]
	public abstract class AbstractPage : IScheduledTask
	{
		private Int64 lastCallTime;

		protected AbstractSquare[,,] map;
		protected Vector3 size;
		public Vector3 address;

		[NonSerialized]
		public AbstractMap parentMap;

        private List<AbstractEntity> ents = new List<AbstractEntity>();

		public Dictionary<Int64, List<PageCallbackInfo>> aiDispatchTable
		{
			get;
			private set;
		}

		[NonSerialized]
		public CachingMode cacheMode = CachingMode.Active;

		protected AbstractPage() {
			aiDispatchTable = new Dictionary<Int64, List<PageCallbackInfo>>();
			this.lastCallTime = Game.Time;
		}

		public AbstractPage(Vector3 pageSize) : this(pageSize.x, pageSize.y, pageSize.z)
		{
		}

        public AbstractPage(Int32 width, Int32 height, Int32 depth) : this()
		{
			this.size = new Vector3(width, height, depth);
			map = new AbstractSquare[size.x, size.y, size.x];
			for (int x = 0; x < size.x; ++x)
			{
				for (int y = 0; y < size.y; ++y)
				{
					for (int z = 0; z < size.z; ++z)
					{
						map[x, y, z] = new EmptySquare(x, y, z);
					}
				}
			}

			aiDispatchTable = new Dictionary<Int64, List<PageCallbackInfo>>();
			this.lastCallTime = Game.Time;
		}

		public Vector3 Size
		{
			get
			{
				return this.size;
			}
		}

        public List<AbstractEntity> Entities
        {
            get
            {
                return ents;
            }
        }
		
		public abstract void Build();
		
		public virtual AbstractSquare this[Int32 x, Int32 y]
		{
			get
			{
				return this.map[x, y, 0];
			}
			protected set
			{
				this.map[x, y, 0] = value;
			}
		}
		public virtual AbstractSquare this[Point p]
		{
			get
			{
				return this.map[p.X, p.Y, 0];
			}
			set
			{
				this.map[p.X, p.Y, 0] = value;
			}
		}
		public virtual AbstractSquare this[Int32 x, Int32 y, Int32 z]
		{
			get
			{
				return this.map[x, y, z];
			}
			protected set
			{
				this.map[x, y, z] = value;
			}
		}
		public virtual AbstractSquare this[Vector3 p]
		{
			get
			{
				return this.map[p.x, p.y, p.z];
			}
			set
			{
				this.map[p.x, p.y, p.z] = value;
			}
		}

		public void SetSquare(Int32 x, Int32 y, Int32 z, AbstractSquare sq) { this.map[x, y, z] = sq; }
        public AbstractSquare GetSquare(Int32 x, Int32 y, Int32 z) { return this.map[x, y, z]; }
		
		public AbstractSquare LocateNeighbor(AbstractSquare s, Direction d)
		{
			return LocateNeighbor(s.Position, d);
		}
		
		public AbstractSquare LocateNeighbor(Vector3 p, Direction d)
		{
			Vector3 offset = new Vector3(0,0,0);
			
			switch (d)
			{
				case Direction.Up:
					offset = new Vector3(0,0,-1);
					break;
				case Direction.Down:
					offset = new Vector3(0,0,1);
					break;
				case Direction.North:
					offset = new Vector3(0,-1,0);
					break;
				case Direction.South:
					offset = new Vector3(0,1,0);
					break;
				case Direction.East:
					offset = new Vector3(1,0,0);
					break;
				case Direction.West:
					offset = new Vector3(-1,0,0);
					break;
				case Direction.Northeast:
					offset = new Vector3(1,-1,0);
					break;
				case Direction.Northwest:
					offset = new Vector3(-1,-1,0);
					break;
				case Direction.Southeast:
					offset = new Vector3(1,1,0);
					break;
				case Direction.Southwest:
					offset = new Vector3(-1,1,0);
					break;
			}
			Vector3 neighborPosition = p + offset;
			if (neighborPosition.x >= 0 && neighborPosition.x <= map.GetUpperBound(0) &&
			    neighborPosition.y >= 0 && neighborPosition.y <= map.GetUpperBound(1) &&
				neighborPosition.z >= 0 && neighborPosition.z <= map.GetUpperBound(2)) 
			{
				return map[p.x, p.y, p.z];
			} else {
				return parentMap.GetSquare(this, neighborPosition);
			}
		}

		/// <summary>
		/// Allows an IPageCallbacker (the page itself or an Entity) to register a
		/// callback for some future point in time. Designed for AI calls from entities,
		/// hence the name.
		/// </summary>
		/// <param name="timeToCall">The future tick to invoke the delegate at.</param>
		/// <param name="target">The object that requests this AI call; used for later culling.</param>
		/// <param name="callbackFunction">The delegate for the function to call.</param>
		public void RegisterAIDelegate(Int64 timeToCall, IPageCallbacker target, PageActionDelegate callbackFunction)
		{
			this.RegisterAIDelegate(new PageCallbackInfo(timeToCall, target, callbackFunction));
		}

		/// <summary>
		/// Allows an IPageCallbacker (the page itself or an Entity) to register a
		/// callback for some future point in time. Designed for AI calls from entities,
		/// hence the name.
		/// </summary>
		/// <param name="callbackInfo">The collected callback information.</param>
		public void RegisterAIDelegate(PageCallbackInfo callbackInfo)
		{
			List<PageCallbackInfo> foo = null;
			if (this.aiDispatchTable.TryGetValue(callbackInfo.CallTime, out foo) == true)
			{
				foo.Add(callbackInfo);
			}
			else
			{
				foo = new List<PageCallbackInfo>();
				foo.Add(callbackInfo);
				this.aiDispatchTable.Add(callbackInfo.CallTime, foo);
			}
		}

		/// <summary>
		/// Removes a single AI delegate from the page's schedule.
		/// </summary>
		/// <param name="time">The time at which the delegate to remove is running.</param>
		/// <param name="target">The object owning the delegate to remove.</param>
		/// <returns>The removed delegate, or null if not found.</returns>
		public PageCallbackInfo RemoveAIDelegate(Int64 time, IPageCallbacker target)
		{
			List<PageCallbackInfo> foo = null;
			if (this.aiDispatchTable.TryGetValue(time, out foo) == true)
			{
				List<PageCallbackInfo> remList = new List<PageCallbackInfo>();
				foreach (PageCallbackInfo p in foo)
				{
					if (p.Target == target)
						remList.Add(p);
				}
				foreach (PageCallbackInfo p in remList)
				{
					foo.Remove(p);
					return p;
				}
			}
			return null;
		}

		/// <summary>
		/// Removes all delegates from the specified IPageCallbacker from the
		/// page scheduler, and returns them. Used primarily to get a list of
		/// callbacks that must transition during a page move.
		/// </summary>
		/// <param name="target">The target whose callbacks to remove.</param>
		/// <returns>A list of all callbacks started by the specified IPageCallbacker.</returns>
		public List<PageCallbackInfo> RemoveAllAIDelegates(IPageCallbacker target)
		{
			return this.RemoveAllAIDelegates(new IPageCallbacker[] { target } );
		}

		/// <summary>
		/// Removes all delegates from the specified IPageCallbackers from the
		/// page scheduler, and returns them. Used primarily to get a list of
		/// callbacks that must transition during a page move.
		/// </summary>
		/// <param name="targets">The targets whose callbacks to remove.</param>
		/// <returns>A list of all callbacks started by the specified IPageCallbackers.</returns>
		public List<PageCallbackInfo> RemoveAllAIDelegates(IList<IPageCallbacker> targets)
		{
			List<PageCallbackInfo> allRemovedNodes = new List<PageCallbackInfo>();
			foreach (Int64 t in this.aiDispatchTable.Keys)
			{
				List<PageCallbackInfo> foo = this.aiDispatchTable[t];
				List<PageCallbackInfo> remList = new List<PageCallbackInfo>();
				foreach (PageCallbackInfo p in foo)
				{
					if (targets.Contains(p.Target))
					{
						remList.Add(p);
						allRemovedNodes.Add(p);
					}
				}
				foreach (PageCallbackInfo p in remList)
				{
					foo.Remove(p);
				}
			}

			return allRemovedNodes;
		}


		public virtual void ScheduledAction()
		{
			List<PageCallbackInfo> callList = null;
			for (Int64 i = this.lastCallTime; i <= Game.Time; i++)
			{
				if (this.aiDispatchTable.TryGetValue(i, out callList))
				{
					foreach (PageCallbackInfo p in callList)
					{
						p.Method(this);
					}
					this.aiDispatchTable.Remove(i);
				}
			}
		}
	}


	public delegate void PageActionDelegate(AbstractPage page);
}
