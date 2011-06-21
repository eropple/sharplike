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
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Sharplike.Core.Messaging;

namespace Sharplike.Core.Rendering
{
    [ChannelSubscriber("Regions")]
	[Serializable]
    public abstract class AbstractRegion : IComparable<AbstractRegion>, IDeserializationCallback, IDisposable
    {
		[NonSerialized]
        private RegionTile[,] regionTiles = new RegionTile[0, 0];
        public RegionTile[,] RegionTiles
        {
            get { return regionTiles; }
        }

        private Size size;
        public Size Size
        {
            get
            {
                return size;
            }
            set
            {
                this.ResizeRegion(value);
                size = value;
                if (this.Resize != null)
                    this.Resize();
            }
        }

		[NonSerialized]
        private Point location;
        public Point Location
        {
            get
            {
                return this.location;
            }
            set
            {
                location = value;
                if (this.Move != null)
                    this.Move();
            }
        }

        public Rectangle Rectangle
        {
            get { return new Rectangle(location, size); }
        }

		[NonSerialized]
        private Int32 zOrder;
        public Int32 ZOrder
        {
            get { return zOrder; }
            set
            {
                zOrder = value;
                if (this.ZOrderChange != null)
                    this.ZOrderChange();
            }
        }

		[NonSerialized]
        private AbstractRegion parent;
        public AbstractRegion Parent
        {
            get { return this.parent; }
        }

		[NonSerialized]
        private SortedDictionary<Int32, AbstractRegion> childRegions;

        public AbstractRegion(Size size, Point location)
        {
            this.ZOrder = 0;
            this.Size = size;
            this.Location = location;
            childRegions = new SortedDictionary<Int32, AbstractRegion>();
        }


        /// <summary>
        /// Change the size of the region.
        /// </summary>
        /// <param name="dimensions">The new size.</param>
        private void ResizeRegion(Size dimensions)
        {
            if (parent != null)
            {
                parent.InvalidateTiles(new Rectangle(this.location,
                    new Size(Math.Max(this.Size.Width, dimensions.Width),
                        Math.Max(this.Size.Height, dimensions.Height))));
            }

            RegionTile[,] newRegion = new RegionTile[dimensions.Width, dimensions.Height];

			for (Int32 y = 0; y < dimensions.Height; y++)
			{
				for (Int32 x = 0; x < dimensions.Width; x++)
				{
					if (x < regionTiles.GetLength(0) && y < regionTiles.GetLength(1))
					{
						newRegion[x, y] = regionTiles[x, y];
					}
					else
					{
						newRegion[x, y] = new RegionTile();
					}
				}
			}

            regionTiles = newRegion;
        }

        /// <summary>
        /// Changes the position of the region in parent region space.
        /// </summary>
        /// <param name="newLocation">The new location of the region.</param>
        private void RelocateRegion(Point newLocation)
        {
            InvalidateTiles();
            this.location = newLocation;
            parent.InvalidateTiles(new Rectangle(this.location, this.size));
        }

        /// <summary>
        /// Invalidates all tiles within the region.
        /// </summary>
        public void InvalidateTiles()
        {
            InvalidateTiles(new Rectangle(new Point(0, 0), this.Size));
        }

        /// <summary>
        /// Invalidates the tiles in a given subset of the region.
        /// </summary>
        /// <param name="region">
        /// The rectangle to invalidate. 
        /// It is safe for this rectangle to exceed the region bounds.
        /// </param>
        public void InvalidateTiles(Rectangle region)
        {
            for (Int32 y = 0; y < region.Height; ++y)
			{
				if (y < 0 || y >= region.Height) continue;
				for (Int32 x = 0; x < region.Width; ++x)
				{
					if (x < 0 || x >= region.Width) continue;

					if (this.regionTiles[x,y].displaytile != null)
						this.regionTiles[x, y].displaytile.MakeStackDirty();
				}
			}
        }

		/// <summary>
		/// Returns a list of all child regions. Regions are sorted by Z-order.
		/// This is not a static list; all references refer to the actual child
		/// regions and not to copies.
		/// </summary>
		public IList<AbstractRegion> ChildRegions
        {
            get
            {
                return new List<AbstractRegion>(this.childRegions.Values).AsReadOnly();
            }
        }

        /// <summary>
        /// Adds the specified region as a child of this region.
        /// </summary>
        /// <param name="childRegion">The child region to add.</param>
        public void AddRegion(AbstractRegion childRegion)
        {
            this.AddRegion(childRegion, this.ChildRegions.Count + 1);
        }
        /// <summary>
        /// Adds the specified region as a child of this region. Enables
        /// you to specify the ZOrder for your regions.
        /// </summary>
        /// <param name="childRegion">The child region to add.</param>
        /// <param name="zOrder">The ZOrder of the new region.</param>
        public void AddRegion(AbstractRegion childRegion, Int32 zOrder)
        {
            AbstractRegion oldP = this.parent;
            if (childRegion.Parent != null)
                childRegion.Parent.RemoveRegion(childRegion);

            this.childRegions.Add(zOrder, childRegion);
            if (this.ChildRegionAdded != null)
                this.ChildRegionAdded(childRegion);

            childRegion.parent = this;

            if (this.Reparent != null)
                this.Reparent(oldP, this.parent);

            InvalidateTiles(new Rectangle(childRegion.Location, childRegion.Size));
        }

        /// <summary>
        /// Removes a child region by ZOrder.
        /// </summary>
        /// <param name="zOrder">The ZOrder of the child region to remove.</param>
        /// <returns>Returns true if a region existed at that ZOrder.</returns>
        public Boolean RemoveRegion(Int32 zOrder)
        {
            AbstractRegion r = null;

            if (this.childRegions.ContainsKey(zOrder))
            {
                r = this.childRegions[zOrder];
                return RemoveRegion(r);
            }

            return false;
        }

        /// <summary>
        /// Changes the ZOrder of the specified child region.
        /// </summary>
        /// <param name="childRegion">The child region to change.</param>
        /// <param name="zOrder">The child region's new ZOrder.</param>
        public void ChangeChildZOrder(AbstractRegion childRegion, Int32 zOrder)
        {
            if (this.childRegions.ContainsValue(childRegion) == false)
                throw new ArgumentException("Child region does not exist in this region.");

            foreach (Int32 i in this.childRegions.Keys)
            {
                if (this.childRegions[i] == childRegion)
                {
                    this.childRegions.Remove(i);
                }
            }
            this.childRegions.Add(zOrder, childRegion);
            childRegion.ZOrder = zOrder;
        }

        /// <summary>
        /// Removes a child region.
        /// </summary>
        /// <param name="region">The child region to remove.</param>
        /// <returns>Returns true if a region was removed.</returns>
        public Boolean RemoveRegion(AbstractRegion region)
        {
            AbstractRegion r = region;

            if (this.childRegions.ContainsValue(r))
            {
                foreach (Int32 i in this.childRegions.Keys)
                {
                    if (this.childRegions[i] == r)
                    {
                        r.InvalidateTiles();
                        this.childRegions.Remove(i);
                        r.parent = null;
                        if (this.ChildRegionRemoved != null)
                            this.ChildRegionRemoved(r);
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Gets a child region by ZOrder.
        /// </summary>
        /// <param name="zOrder">The ZOrder of the child region.</param>
        /// <returns>The child region, if found.</returns>
        public AbstractRegion GetRegion(Int32 zOrder)
        {
            if (this.childRegions.ContainsKey(zOrder) == false)
                throw new ArgumentException("No region with that ZOrder was found.");

            return this.childRegions[zOrder];
        }

        /// <summary>
        /// Recursively populates a list of RegionTiles. Only DisplayTile should ever need this.
        /// </summary>
        /// <param name="tiles">The list of displaytiles to populate.</param>
        /// <param name="loc">The index of the tile array to pull from.</param>
        internal void PopulateRegionTiles(List<RegionTile> tiles, Point loc)
        {
            tiles.Add(this.regionTiles[loc.X, loc.Y]);
            foreach (KeyValuePair<Int32, AbstractRegion> r in this.childRegions)
            {
                if (r.Value.Location.X <= loc.X
                    && r.Value.Location.Y <= loc.Y
                    && r.Value.Location.X + r.Value.Size.Width > loc.X
                    && r.Value.Location.Y + r.Value.Size.Height > loc.Y)
                {
                    r.Value.PopulateRegionTiles(tiles,
                        new Point(loc.X - r.Value.Location.X, loc.Y - r.Value.Location.Y));
                }
            }
        }

        /// <summary>
        /// Clear all tiles.
        /// </summary>
        public void Clear()
        {
            for (Int32 x = 0; x < this.Size.Width; x++)
            {
                for (Int32 y = 0; y < this.Size.Height; y++)
                {
                    regionTiles[x, y].Reset();
                }
            }
        }


        /// <summary>
        /// Compares Z-Ordering of two regions. Only provides semantically
        /// valid data if both regions are children of the same parent.
        /// </summary>
        /// <param name="other">The other region to test against.</param>
        /// <returns>
        /// A value of less than zero indicates that this region has a lower
        /// Z-order than the other region. A value greater than zero indicates
        /// that this region has a higher Z-order than the other region. A
        /// value of zero indicates that the two are equal.
        /// </returns>
        public Int32 CompareTo(AbstractRegion other)
        {
            return this.ZOrder.CompareTo(other.ZOrder);
        }

        public delegate void EmptyDelegate();
        public delegate void ChildNodeDelegate(AbstractRegion childNode);
        public delegate void ReparentDelegate(AbstractRegion oldParent, AbstractRegion newParent);

        public event EmptyDelegate Resize;
        public event EmptyDelegate Move;
        public event EmptyDelegate ZOrderChange;
        public event ChildNodeDelegate ChildRegionAdded;
        public event ChildNodeDelegate ChildRegionRemoved;
        public event ReparentDelegate Reparent;

        public virtual void Dispose()
        {
            if (parent != null)
                parent.RemoveRegion(this);
            SortedDictionary<int, AbstractRegion> children = new SortedDictionary<int, AbstractRegion>(childRegions);
            foreach (KeyValuePair<int, AbstractRegion> r in children)
                RemoveRegion(r.Value);
        }

		public void OnDeserialization(object sender)
		{
			childRegions = new SortedDictionary<int, AbstractRegion>();

			regionTiles = new RegionTile[size.Width, size.Height];
			for (int x = 0; x < size.Width; ++x)
			{
				for (int y = 0; y < size.Height; ++y)
				{
					regionTiles[x, y] = new RegionTile();
				}
			}
		}
	}
}
