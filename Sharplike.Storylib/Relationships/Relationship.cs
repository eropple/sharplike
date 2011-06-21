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
    public sealed class Relationship
    {
        internal Relationship(Person owner, Person other)
        {
            this.Owner = owner;
            this.Other = other;

            Random rnd = new Random();

            foreach (Trait t in Trait.All)
                Trait_NewTrait(t, new EventArgs());

            foreach (KeyValuePair<Trait, double> kvp in other.Qualities)
                Affect(kvp.Key, kvp.Value);

            Trait.NewTrait += new EventHandler<EventArgs>(Trait_NewTrait);
        }

        void Trait_NewTrait(object sender, EventArgs e)
        {
            impressions.Add((Trait)sender, 0);
            opinions.Add((Trait)sender, 0);
        }

        /// <summary>
        /// The person the relationship belongs to. This is a one-way ownership.
        /// </summary>
        /// <example>
        /// A moviegoer may be the owner of his or her relationship with a movie star.
        /// </example>
        public Person Owner
        {
            get;
            private set;
        }

        /// <summary>
        /// The target of the owner's relationship.
        /// </summary>
        /// <example>
        /// A movie star may be the target of a moviegoer's relationship.
        /// </example>
        public Person Other
        {
            get;
            private set;
        }

        internal void ApplyImpressionDecay(double change)
        {
            foreach (KeyValuePair<Trait, double> kvp in impressions)
            {
                impressions[kvp.Key] -= change * (kvp.Value / Ratio);
            }
        }

        /// <summary>
        /// Affect a particular aspect of a relationship.
        /// </summary>
        /// <param name="t">The trait to affect</param>
        /// <param name="change">
        /// How much to affect the trait. If the trait is negative, 
        /// a numerically positive change will have a harmful effect.
        /// </param>
        public void Affect(Trait t, double change)
        {
            double annoyancefactor = Owner.Annoyances[t];

            if (annoyancefactor >= 0)
                change *= annoyancefactor;
            else if (annoyancefactor < 0)
                change /= annoyancefactor;

            change = t.CalculateAmount(change);

            impressions[t] += change;
            opinions[t] += (change / Ratio);
        }

        /// <summary>
        /// The general quality of a relationship. The value of this depends
        /// entirely on the scale of previous effects.
        /// </summary>
        public double Quality
        {
            get
            {
                double quality = 0;
                int count = 0;

                foreach (KeyValuePair<Trait, double> kvp in impressions)
                {
                    ++count;
                    quality += GetTraitQuality(kvp.Key);
                }

                return quality / count;
            }
        }

        /// <summary>
        /// Gets the quality of a particular trait.
        /// </summary>
        /// <seealso cref="Relationship.Quality"/>
        /// <param name="t">The trait to query.</param>
        /// <returns>The trait quality. 
        /// Like relationship quality, this depends entirely on the scale of effects.</returns>
        public double GetTraitQuality(Trait t)
        {
            double shorttermquality = shorttermquality = impressions[t];
            double longtermquality = longtermquality = opinions[t];

            return (shorttermquality + (longtermquality * Ratio)) / (Ratio + 1.0);
        }

        /// <summary>
        /// All of the traits that play a part in affecting the overall relationship.
        /// </summary>
        public IList<Trait> Traits
        {
            get
            {
                return this.traits.AsReadOnly();
            }
        }

        private List<Trait> traits = new List<Trait>();

        /// <summary>
        /// Ratio describes how much of an effect short-term impressions have on a relationship
        /// vs. long-term opinions.
        /// </summary>
        /// <remarks>
        /// This value has a somewhat fuzzy meaning, and affects what percentage of
        /// trait change goes toward opinion, as well as what percentage of impression
        /// goes toward overall quality. This number also changes how quickly impression decays.
        /// As impression becomes more important, it decays faster.
        /// </remarks>
        public static double Ratio = 5.0;

        /// <summary>
        /// Impressions are short-term opinions. 
        /// They are more drastically affected by changes, but have less effect on relationship quality.
        /// Impressions decay over time.
        /// </summary>
        private Dictionary<Trait, double> impressions = new Dictionary<Trait, double>();
        /// <summary>
        /// Opinions are long-term levels of relationship.
        /// They are only slightly affected by changes, but have a great effect on relationship quality.
        /// Opinions do not decay over time.
        /// </summary>
        private Dictionary<Trait, double> opinions = new Dictionary<Trait, double>();
    }
}
