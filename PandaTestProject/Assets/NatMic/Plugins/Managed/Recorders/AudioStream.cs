/* 
*   NatMic
*   Copyright (c) 2019 Yusuf Olokoba
*/

namespace NatMic.Recorders {

    using UnityEngine;
    using System;
    using System.Runtime.CompilerServices;

    public sealed class AudioStream : IAudioRecorder {

        #region --Client API--

        public readonly int Capacity;

        public int Length {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get { return Math.Max(writeIndex - readIndex, 0); }
        }

        public AudioStream (int capacity = 1 << 16) {
            this.Capacity = capacity;
        }

        public AudioStream (long baseTimestamp, int capacity = 1 << 16) : this(capacity) {
            this.baseTimestamp = baseTimestamp;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Dispose () {
            buffer = null;
            readIndex =
            writeIndex = 0;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Read (float[] dst) {
            if (buffer == null)
                return;
            if (readIndex < 0) {
                ReadBackward(dst);
                return;
            }
            var bufferIndex = readIndex % Capacity;
            var remaining = Capacity - bufferIndex;
            var copyCount = Mathf.Min(dst.Length, remaining);
            var residualCount = dst.Length - copyCount;
            Array.Copy(buffer, bufferIndex, dst, 0, copyCount);
            Array.Copy(buffer, 0, dst, copyCount, residualCount);
            readIndex += dst.Length;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Seek (int offset) {
            readIndex += offset;
        }
        #endregion


        #region --Operations--

        private readonly long baseTimestamp;
        private float[] buffer;
        private int writeIndex, readIndex;

        [MethodImpl(MethodImplOptions.Synchronized)]
        void IAudioProcessor.OnSampleBuffer (float[] sampleBuffer, int sampleRate, int channelCount, long timestamp) {
            // Create buffer
            if (buffer == null) {
                buffer = new float[Capacity];
                if (baseTimestamp != 0L) {
                    var timeDelta = (baseTimestamp - timestamp) / 1e+9;
                    var frameDelta = timeDelta * sampleRate;
                    var sampleDelta = frameDelta * channelCount;
                    Seek((int)sampleDelta);
                }
            }
            // Write to buffer
            var bufferIndex = writeIndex % Capacity;
            var remaining = Capacity - bufferIndex;
            var copyCount = Mathf.Min(sampleBuffer.Length, remaining);
            var residualCount = sampleBuffer.Length - copyCount;
            Array.Copy(sampleBuffer, 0, buffer, bufferIndex, copyCount);
            Array.Copy(sampleBuffer, copyCount, buffer, 0, residualCount);
            writeIndex += sampleBuffer.Length;
        }

        private void ReadBackward (float[] dst) {
            if (dst.Length < -readIndex) {
                Array.Clear(dst, 0, dst.Length);
                readIndex += dst.Length;
            }
            else {
                var readCount = -readIndex;
                var surrogate = new float[dst.Length - readCount];
                readIndex += readCount;
                Read(surrogate);
                Array.Clear(dst, 0, readCount);
                Array.Copy(surrogate, 0, dst, readCount, surrogate.Length);
            }
        }
        #endregion
    }
}