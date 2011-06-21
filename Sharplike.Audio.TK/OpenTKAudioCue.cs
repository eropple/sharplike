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
using Sharplike.Core.Audio;
using OpenTK;
using OpenTK.Audio;
//using OpenTK.Audio.OpenAL;

namespace Sharplike.Audio.TK
{
    public class OpenTKAudioCue : AbstractAudioCue
    {
        internal OpenTKAudioCue(Stream data, AudioContext ac)
        {
            this.ac = ac;

            buffer = AL.GenBuffer();
            ac.CheckErrors();

            source = AL.GenSource();
            ac.CheckErrors();

            AL.Source(source, ALSourcef.Gain, (float)this.Volume);
            ac.CheckErrors();

            using (AudioReader ar = new AudioReader(data))
            {
                SoundData d = ar.ReadToEnd();
                AL.BufferData(source, d);
                ac.CheckErrors();
            }

            AL.Source(source, ALSourcei.Buffer, buffer);
            ac.CheckErrors();

            this.VolumeChanged += new VolumeChangedEventHandler(OpenTKAudioCue_VolumeChanged);
            this.BalanceChanged += new BalanceChangedEventHandler(OpenTKAudioCue_BalanceChanged);
            this.FadeChanged += new FadeChangedEventHandler(OpenTKAudioCue_FadeChanged);
        }

        public override void Dispose()
        {
            Stop();

            AL.DeleteSource(source);
            ac.CheckErrors();

            AL.DeleteBuffer(buffer);
            ac.CheckErrors();
        }

        void OpenTKAudioCue_FadeChanged(object sender, EventArgs e)
        {
        }

        void OpenTKAudioCue_BalanceChanged(object sender, EventArgs e)
        {
        }

        void OpenTKAudioCue_VolumeChanged(object sender, EventArgs e)
        {
            AL.Source(source, ALSourcef.Gain, (float)this.Volume);
            ac.CheckErrors();
        }

        public override void Play()
        {
            AL.SourcePlay(source);
            ac.CheckErrors();
        }

        public override void Pause()
        {
            AL.SourcePause(source);
            ac.CheckErrors();
        }

        public override void Stop()
        {
            AL.SourceStop(source);
            ac.CheckErrors();
        }

        AudioContext ac;
        int source = 0;
        int buffer = 0;
    }
}
