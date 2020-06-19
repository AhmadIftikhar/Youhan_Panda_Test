/* 
*   NatMic
*   Copyright (c) 2019 Yusuf Olokoba
*/

namespace NatMic.DSP {

    using UnityEngine;
    using System;
    using System.Linq; // Yikes
    using System.Threading;
    using Recorders;
    using Docs;

    /// <summary>
    /// Virtual device that mixes audio from multiple devices into one stream
    /// </summary>
    [Doc(@"MixerDevice")]
    public sealed class MixerDevice : IAudioDevice {

        #region --Client API--
        /// <summary>
        /// Source audio devices
        /// </summary>
        [Doc(@"MixerDeviceSources")]
        public readonly IAudioDevice[] Sources;

        /// <summary>
        /// Create a mixer device that mixes audio from multiple audio devices
        /// </summary>
        /// <param name="sources">Source audio devices to mix audio from</param>
        [Doc(@"MixerDeviceCtor")]
        public MixerDevice (params IAudioDevice[] sources) : base() {
            this.Sources = sources;
        }
        #endregion


        #region --IAudioDevice--
        /// <summary>
        /// Is the device currently recording?
        /// </summary>
        [Doc(@"IsRecording")]
        public bool IsRecording {
            get { return processor != null; }
        }

        /// <summary>
        /// Start recording from the audio device.
        /// All source devices MUST support the requested sample rate and channel count.
        /// </summary>
        /// <param name="requestedSampleRate">Requested sample rate</param>
        /// <param name="requestedChannelCount">Requested channel count</param>
        /// <param name="processor">Delegate to receive audio sample buffers</param>
        [Doc(@"StartRecording", @"MixerDeviceStartRecordingDescription")]
        public void StartRecording (int requestedSampleRate, int requestedChannelCount, IAudioProcessor processor) { 
            this.sampleRate = requestedSampleRate;
            this.channelCount = requestedChannelCount;
            this.processor = processor;
            new Thread(MixerLoop).Start();
        }

        /// <summary>
        /// Stop recording from the audio device
        /// </summary>
        [Doc(@"StopRecording")]
        public void StopRecording () {
            this.processor = null;
        }
        #endregion


        #region --Operations--

        private volatile IAudioProcessor processor;
        private int sampleRate, channelCount;
        private const int BufferSize = 1024; // In frames

        private void MixerLoop () {
            // Start devices
            var sleepTimeMs = (int)(1e+3 * BufferSize / sampleRate);
            var mixedBuffer = new float[BufferSize * channelCount];
            var sourceBuffer = new float[mixedBuffer.Length];
            var sourceStreams = Sources.Select(device => {
                var stream = new AudioStream(AudioDevice.CurrentTimestamp);
                device.StartRecording(sampleRate, channelCount, stream);
                return stream;
            }).ToArray();
            // Mix
            while (true) {
                Thread.Sleep(sleepTimeMs / 2);
                if (!sourceStreams.All(stream => stream.Length >= BufferSize))
                    continue;
                foreach (var stream in sourceStreams) {
                    stream.Read(sourceBuffer);
                    Mix(mixedBuffer, sourceBuffer, mixedBuffer);
                }
                var localProcessor = processor;
                if (localProcessor == null)
                    break;
                try {
                    localProcessor.OnSampleBuffer(mixedBuffer, sampleRate, channelCount, AudioDevice.CurrentTimestamp);
                } catch (Exception ex) {
                    Debug.LogError("NatMic Error: MixerDevice processor raised exception: "+ex);
                }
                Array.Clear(mixedBuffer, 0, mixedBuffer.Length);
            }
            // Stop devices
            foreach (var device in Sources)
                device.StopRecording();
            foreach (var stream in sourceStreams)
                stream.Dispose();
        }

        private static void Mix (float[] srcA, float[] srcB, float[] dst) {
            for (int i = 0; i < srcA.Length; i++) {
                var sum = srcA[i] + srcB[i];
                var product = srcA[i] * srcB[i];
                var mult = product > 0 ? srcA[i] > 0 ? -1 : 1 : 0;
                dst[i] = sum + mult * product;
            }
        }
        #endregion
    }
}