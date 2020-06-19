/* 
*   NatMic
*   Copyright (c) 2019 Yusuf Olokoba
*/

namespace NatMic.Recorders {

    using System;
    using Docs;

    /// <summary>
    /// An audio processor that records incoming audio data
    /// </summary>
    public interface IAudioRecorder : IAudioProcessor, IDisposable { }
}