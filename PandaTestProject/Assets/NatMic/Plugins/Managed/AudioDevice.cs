/* 
*   NatMic
*   Copyright (c) 2019 Yusuf Olokoba
*/

namespace NatMic {

    using UnityEngine;
    using System;
    using Platforms;
    using Docs;

    /// <summary>
    /// An abstraction for a hardware audio input device
    /// </summary>
    [Doc(@"AudioDevice")]
    public abstract class AudioDevice : IAudioDevice, IEquatable<AudioDevice> {

        #region --Introspection--
        /// <summary>
        /// Currently available hardware audio input devices
        /// </summary>
        [Doc(@"Devices")]
        public static AudioDevice[] Devices {
            get {
                switch (Application.platform) {
                    case RuntimePlatform.Android: return AudioDeviceAndroid.Devices;
                    case RuntimePlatform.OSXEditor:
                    case RuntimePlatform.OSXPlayer:
                    case RuntimePlatform.WindowsEditor:
                    case RuntimePlatform.WindowsPlayer:
                    case RuntimePlatform.IPhonePlayer: return AudioDeviceiOS.Devices;
                    case RuntimePlatform.WebGLPlayer: // Soon enough :)
                    default: return new AudioDevice[0];
                }
            }
        }
        /// <summary>
        /// Currently timestamp on this device's high-resolution clock
        /// </summary>
        [Doc(@"CurrentTimestamp")]
        public static long CurrentTimestamp {
            get {
                switch (Application.platform) {
                    case RuntimePlatform.Android: return AudioDeviceAndroid.CurrentTimestamp;
                    case RuntimePlatform.OSXEditor:
                    case RuntimePlatform.OSXPlayer:
                    case RuntimePlatform.WindowsEditor:
                    case RuntimePlatform.WindowsPlayer:
                    case RuntimePlatform.IPhonePlayer: return AudioDeviceiOS.CurrentTimestamp;
                    case RuntimePlatform.WebGLPlayer:
                    default: return 0L;
                }
            }
        }
        #endregion


        #region --Properties--
        /// <summary>
        /// Display-friendly device name
        /// </summary>
        [Doc(@"Name")]
        public abstract string Name { get; }
        /// <summary>
        /// Device unique ID
        /// </summary>
        [Doc(@"UniqueID")]
        public abstract string UniqueID { get; }
        /// <summary>
        /// Does this device have Adaptive Echo Cancellation?
        /// </summary>
        [Doc(@"EchoCancellation")]
        public abstract bool EchoCancellation { get; }
        #endregion


        #region --IAudioDevice--
        /// <summary>
        /// Is the device currently recording?
        /// </summary>
        [Doc(@"IsRecording")]
        public abstract bool IsRecording { get; }
        /// <summary>
        /// Start recording from the audio device
        /// </summary>
        /// <param name="requestedSampleRate">Requested sample rate</param>
        /// <param name="requestedChannelCount">Requested channel count</param>
        /// <param name="processor">Delegate to receive audio sample buffers</param>
        [Doc(@"StartRecording")]
        public abstract void StartRecording (int requestedSampleRate, int requestedChannelCount, IAudioProcessor processor);
        /// <summary>
        /// Stop recording from the audio device
        /// </summary>
        [Doc(@"StopRecording")]
        public abstract void StopRecording ();
        #endregion


        #region --IEquatable--

        public bool Equals (AudioDevice other) {
            return other != null && other.UniqueID == UniqueID;
        }

        public override string ToString () {
            return Name + " (" + UniqueID + ")";
        }
        #endregion
    }
}