/* 
*   NatMic
*   Copyright (c) 2019 Yusuf Olokoba
*/

namespace NatMic.Platforms {

    using UnityEngine;
    using UnityEngine.Scripting;
    using System;

    public sealed class AudioDeviceAndroid : AudioDevice {

        #region --Intropection--

        public new static AudioDeviceAndroid[] Devices {
            get {
                AudioDevice = AudioDevice ?? new AndroidJavaClass(AudioDeviceID);
                using (var devicesArray = AudioDevice.CallStatic<AndroidJavaObject>(@"devices")) {
                    var devices = AndroidJNIHelper.ConvertFromJNIArray<AndroidJavaObject[]>(devicesArray.GetRawObject());
                    var result = new AudioDeviceAndroid[devices.Length];
                    for (var i = 0; i < devices.Length; i++)
                        result[i] = new AudioDeviceAndroid(devices[i]);
                    return result;
                }
            }
        }

        public new static long CurrentTimestamp {
            get {
                AudioDevice = AudioDevice ?? new AndroidJavaClass(AudioDeviceID);
                return AudioDevice.CallStatic<long>(@"currentTimestamp");
            }
        }
        #endregion


        #region --AudioDevice--

        public override string Name {
            get { return device.Call<string>(@"name"); }
        }

        public override string UniqueID {
            get { return device.Call<string>(@"uniqueID"); }
        }

        public override bool EchoCancellation {
            get { return device.Call<bool>(@"echoCancellation"); }
        }
        #endregion


        #region --IAudioDevice--

        public override bool IsRecording {
            get { return device.Call<bool>(@"isRecording"); }
        }

        public override void StartRecording (int sampleRate, int channelCount, IAudioProcessor processor) {
            this.processor = processor;
            device.Call(@"startRecording", sampleRate, channelCount, new SampleBufferDelegate(this));
        }

        public override void StopRecording () {
            this.processor = null;
            device.Call(@"stopRecording");
        }
        #endregion


        #region --Operations--
        
        private readonly AndroidJavaObject device;
        private volatile IAudioProcessor processor;
        private static AndroidJavaClass AudioDevice;
        private const string AudioDeviceID = @"com.yusufolokoba.natmic.AudioDevice";

        private AudioDeviceAndroid (AndroidJavaObject device) {
            this.device = device;
        }

        ~AudioDeviceAndroid () {
            device.Dispose();
        }

        private class SampleBufferDelegate : AndroidJavaProxy {

            private readonly AudioDeviceAndroid device;

            public SampleBufferDelegate (AudioDeviceAndroid device) : base(@"com.yusufolokoba.natmic.SampleBufferDelegate") {
                this.device = device;
            }

            [Preserve]
            private void onSampleBuffer (AndroidJavaObject frame, int sampleRate, int channelCount) {
                // Null checking
                if (device.processor == null)
                    return;
                // Marshal sample buffer
                var sampleBuffer = AndroidJNI.FromFloatArray(frame.Get<AndroidJavaObject>(@"sampleBuffer").GetRawObject());
                var timestamp = frame.Get<long>(@"timestamp");
                // Pass to processor
                try {
                    device.processor.OnSampleBuffer(sampleBuffer, sampleRate, channelCount, timestamp);
                } catch (Exception ex) {
                    Debug.LogError("NatMic Error: AudioDevice processor raised exception: "+ex);
                }
            }
        }
        #endregion
    }
}