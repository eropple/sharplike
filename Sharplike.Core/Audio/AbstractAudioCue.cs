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
using System.IO;

namespace Sharplike.Core.Audio
{
	public abstract class AbstractAudioCue : IDisposable
	{
		protected Double volume = 0.5;
		protected Double balance = 0.0;
		protected Double fade = 0.0;

        public abstract void Play();
        public abstract void Stop();
        public abstract void Pause();
		
		/// <summary>
		/// Volume of the audio cue. Normalized: values are between 0.0 and 1.0, inclusive.
		/// </summary>
		public Double Volume
		{
			get
			{
				return volume;
			}
			set
			{
				if (value > 1 || value < 0)
					throw new ArgumentOutOfRangeException("Volume must be between 0.0 and 1.0.");
                volume = value;

                if (VolumeChanged != null)
                    VolumeChanged(this, new EventArgs());
			}
		}

		/// <summary>
		/// Balance (left/right panning) of the audio cue. Normalized: valid values
		/// are between -1.0 and 1.0, inclusive.
		/// </summary>
		public Double Balance
		{
			get
			{
				return balance;
			}
			set
			{
				if (value > 1 || value < -1)
					throw new ArgumentOutOfRangeException("Balance must be between -1.0 and 1.0.");
                balance = value;

                if (BalanceChanged != null)
                    BalanceChanged(this, new EventArgs());
			}
		}

		/// <summary>
		/// Fade (forward/backward panning) of the audio cue. Normalized: valid values
		/// are between -1.0 and 1.0, inclusive. Note that this only works if users have
		/// surround sound setups; consider leaving this at 0 unless you're sure.
		/// </summary>
		public Double Fade
		{
			get
			{
				return fade;
			}
			set
			{
				if (value > 1 || value < -1)
					throw new ArgumentOutOfRangeException("Fade must be between -1.0 and 1.0.");
				fade = value;

                if (FadeChanged != null)
                    FadeChanged(this, new EventArgs());
			}
		}

        public abstract void Dispose();


        public delegate void FadeChangedEventHandler(object sender, EventArgs e);
        /// <summary>
        /// Raised when the fade (front-rear pan) value of the cue is changed.
        /// </summary>
        public event FadeChangedEventHandler FadeChanged;

        public delegate void BalanceChangedEventHandler(object sender, EventArgs e);
        /// <summary>
        /// Raised when the balance (left-right pan) of the cue is changed.
        /// </summary>
        public event BalanceChangedEventHandler BalanceChanged;

        public delegate void VolumeChangedEventHandler(object sender, EventArgs e);
        /// <summary>
        /// Raised when the volume of the cue is changed.
        /// </summary>
        public event VolumeChangedEventHandler VolumeChanged;
    }
}
