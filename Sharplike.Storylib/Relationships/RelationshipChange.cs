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
using Sharplike.Storylib.DialogueTree;

namespace Sharplike.Storylib.Relationships
{
    /// <summary>
    /// A utility for batching relationship changes.
    /// Also useful for binding relationship changes to dialogue options.
	/// </summary>
	[Serializable]
    public class RelationshipChange
    {
        public RelationshipChange()
        {
        }

        /// <summary>
        /// Constructor which lets you provide a dialogue option and relationship to automatically bind to.
        /// </summary>
        /// <param name="option">
        /// The dialogue option which, when triggered by a visitor, will apply the batch.
        /// </param>
        /// <param name="relationship">The relationship that will be modified.</param>
        /// <param name="applyMultiple">
        /// Whether the batch may be applied more than once for the same dialogue option.
        /// </param>
        public RelationshipChange(DialogueOption option, Relationship relationship, bool applyMultiple = false)
        {
            rel = relationship;
            opt = option;
            applymulti = applyMultiple;
			chooseevent = new EventHandler<EventArgs>(opt_OptionChosen);
			opt.OptionChosen += chooseevent;
        }
		private EventHandler<EventArgs> chooseevent;

		void opt_OptionChosen(object sender, EventArgs e)
		{
			if (!autoapplied || applymulti)
				Apply(rel);
			autoapplied = true;

			if (!applymulti)
				opt.OptionChosen -= chooseevent;
		}

        /// <summary>
        /// Applies a batch.
        /// </summary>
        /// <param name="target">The relationship that the batch should be applied to.</param>
        public void Apply(Relationship target)
        {
            if (Applied != null)
                Applied(this, new RelationshipChangeEventArgs(target));
        }

        /// <summary>
        /// Adds a relationship trait change to the batch.
        /// </summary>
        /// <param name="t">The trait to change.</param>
        /// <param name="amount">How much the trait should change.</param>
        public void AddRelationshipChange(Trait t, double amount)
        {
            Applied += delegate(object sender, RelationshipChangeEventArgs r)
            {
                r.Relationship.Affect(t, amount);
            };
        }

        /// <summary>
        /// Adds a personality effect to the batch.
        /// Personality effects will change the first impression that new relationships get seeded with.
        /// </summary>
        /// <param name="target">The person whose personality should change.</param>
        /// <param name="t">The trait of the personality that should change.</param>
        /// <param name="amount">How much the specified trait should change.</param>
        public void AddPersonalityEffect(Person target, Trait t, double amount)
        {
            Applied += delegate(object sender, RelationshipChangeEventArgs r)
            {
                Dictionary<Trait, double> qual = r.Relationship.Other.Qualities;
                if (!qual.ContainsKey(t))
                    qual.Add(t, 0);
                qual[t] += amount;
            };
        }

        private DialogueOption opt = null;
        private Relationship rel = null;
        private bool autoapplied = false;
        private bool applymulti = false;

        public class RelationshipChangeEventArgs : EventArgs
        {
            public RelationshipChangeEventArgs(Relationship r)
            {
                this.Relationship = r;
            }

            public Relationship Relationship;
        }
        public event EventHandler<RelationshipChangeEventArgs> Applied;
    }
}
