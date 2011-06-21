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

namespace Sharplike.Storylib.Relationships
{
	[Serializable]
    public class Person : IDisposable
    {
        public Person(String name)
        {
            this.Name = name;

            foreach (Trait t in Trait.All)
                Trait_NewTrait(t, new EventArgs());

			ev = new EventHandler<EventArgs>(Trait_NewTrait);
            Trait.NewTrait += ev;
        }

		public void Dispose()
		{
			Trait.NewTrait -= ev;
		}

		private EventHandler<EventArgs> ev;

        void Trait_NewTrait(object sender, EventArgs e)
        {
            Qualities[(Trait)sender] = 0;
            Annoyances[(Trait)sender] = 1;
        }

        /// <summary>
        /// The human-readable name of the person.
        /// </summary>
        public String Name
        {
            get;
            set;
        }

        /// <summary>
        /// "Introduces" the person to someone else, creating a new relationship
        /// and populating it with the other person's qualities.
        /// </summary>
        /// <param name="p">The person to introduce</param>
        /// <returns>A new relationship</returns>
        public Relationship Introduce(Person p)
        {
            Relationship r = new Relationship(this, p);
            relationships.Add(r);
            return r;
        }

        /// <summary>
        /// The person observes a passage of time, which decays all short term impressions.
        /// </summary>
        /// <remarks>
        /// Impressions do not decay linearly. The farther an impression is from indifference,
        /// the faster it will decay. Likewise, decay will slow as the impression approaches
        /// zero (indifferent).
        /// </remarks>
        /// <param name="shorttermchange">How much impressions should decay.</param>
        public void PassTime(double shorttermchange)
        {
            foreach (Relationship r in relationships)
            {
                r.ApplyImpressionDecay(shorttermchange);
            }
        }

        /// <summary>
        /// All of the people that this person has been introduced to. Note: Relationships are NOT bidirectional.
        /// </summary>
        public IList<Relationship> Relationships
        {
            get
            {
                return relationships.AsReadOnly();
            }
        }

        /// <summary>
        /// Multipliers for traits that this person finds annoying. Default is 1.0 or -1.0. No effect is 0.0.
        /// Negative annoyances cause trait changes to be generally more benefitial. Positives cause trait changes
        /// to be weighted harmfully.
        /// </summary>
        public Dictionary<Trait, double> Annoyances
        {
            get
            {
                return annoyances;
            }
        }

        /// <summary>
        /// Qualities of this person. This serves as a first impression for all introductions.
        /// </summary>
        public Dictionary<Trait, double> Qualities
        {
            get
            {
                return qualities;
            }
        }

        private Dictionary<Trait, double> qualities = new Dictionary<Trait, double>();
        private Dictionary<Trait, double> annoyances = new Dictionary<Trait, double>();
        internal List<Relationship> relationships = new List<Relationship>();
	}
}
