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

namespace Sharplike.Storylib.Relationships
{
	[Serializable]
    public class Trait
    {
        public Trait(String name, TraitType effectType)
            : this(name)
        {
            Effect = effectType;
        }

        public Trait(String name)
            : this()
        {
            this.Name = name;
        }

        public Trait()
        {
            Effect = TraitType.Positive;
            traits.Add(this);
            if (NewTrait != null)
                NewTrait(this, new EventArgs());
        }

        public String Name
        {
            get;
            set;
        }

        public String Description
        {
            get;
            set;
        }

        public TraitType Effect
        {
            get;
            set;
        }

        public double CalculateAmount(double amount)
        {
            return (Effect == TraitType.Positive) ? amount : -amount;
        }

        public static IList<Trait> All
        {
            get
            {
                return traits.AsReadOnly();
            }
        }

        public static event EventHandler<EventArgs> NewTrait;

        private static List<Trait> traits = new List<Trait>();
    }

    public enum TraitType
    {
        Positive,
        Negative
    }
}
