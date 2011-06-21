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
using OpenTK.Audio;

namespace Sharplike.Audio.TK
{
    public class OpenTKAudioEngine : AbstractAudioEngine
    {
        public OpenTKAudioEngine()
        {
            ac = new AudioContext();
            ac.CheckErrors();
        }
        public override AbstractAudioCue BuildAudioCue(Stream audioData)
        {
            return new OpenTKAudioCue(audioData, ac);
        }

        public override void Process()
        {
            ac.Process();
            ac.CheckErrors();
        }

        public override void Dispose()
        {
            ac.Dispose();
        }

        AudioContext ac;
    }
}
