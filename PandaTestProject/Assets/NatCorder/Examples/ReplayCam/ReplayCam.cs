/* 
*   NatCorder
*   Copyright (c) 2019 Yusuf Olokoba
*/

namespace NatCorder.Examples {

    #if UNITY_EDITOR
	using UnityEditor;
	#endif
    using UnityEngine;
    using UnityEngine.UI;
    using System.Collections;
    using Clocks;
    using Inputs;
    using NatShareU;
    using UnityEngine.Android;
    using NatMic;

    public class ReplayCam : MonoBehaviour ,IAudioProcessor
    {

        /**
        * ReplayCam Example
        * -----------------
        * This example records the screen using a `CameraRecorder`.
        * When we want mic audio, we play the mic to an AudioSource and record the audio source using an `AudioRecorder`
        * -----------------
        * Note that UI canvases in Overlay mode cannot be recorded, so we use a different mode (this is a Unity issue)
        */

        [Header("Recording")]
        public int videoWidth = 1280;
        public int videoHeight = 720;


        private IAudioDevice audioDevice;
        private MP4Recorder videoRecorder;
        private IClock recordingClock;
        private CameraInput cameraInput;
        private AudioInput audioInput;
        public Canvas canvas;
        private void Start()
        {
                #if PLATFORM_ANDROID
                      if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
                      {
                            Permission.RequestUserPermission(Permission.ExternalStorageWrite);
                      }
                      if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
                      {
                            Permission.RequestUserPermission(Permission.Microphone);
                      }
                #endif
        }

        public void StartRecording () {
            // Start recording
            var sampleRate = 44100;
            var channelCount = 2;
            Debug.Log("Recording Started");
            recordingClock = new RealtimeClock();
            videoRecorder = new MP4Recorder(
                videoWidth,
                videoHeight,
                30,
                sampleRate,
                channelCount,
                OnReplay
            );
            // Create recording inputs
            cameraInput = new CameraInput(videoRecorder, recordingClock, Camera.main);
            audioDevice = AudioDevice.Devices[0];
            audioDevice.StartRecording(sampleRate, channelCount,this);
        }

        public void OnSampleBuffer(float[] sampleBuffer, int sampleRate, int channelCount, long timestamp)
        {
            // Send sample buffers directly to the video recorder for recording
            videoRecorder.CommitSamples(sampleBuffer, recordingClock.Timestamp);
        }

        public void StopRecording () {
            Debug.Log("Recording Stopped");
            // Stop the recording inputs
            audioDevice.StopRecording();
            cameraInput.Dispose();
            // Stop recording
            videoRecorder.Dispose();
        }

       

        private void OnReplay (string path) {
            Debug.Log("Saved recording to: "+path);
            Handheld.Vibrate();
            NatShare.SaveToCameraRoll(path,"Haiti AR");

        }

        public void TakeScreenShot()
        {
            canvas.enabled = false;
            Debug.Log("Screen Shot Taken");
            StartCoroutine(TakeScreenshotAndSave());
        }
        private IEnumerator TakeScreenshotAndSave()
        {
            yield return new WaitForEndOfFrame();

            Texture2D ss = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
            ss.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
            ss.Apply();

            NatShare.SaveToCameraRoll(ss,"Haiti AR");
            // To avoid memory leaks
            Destroy(ss);
            canvas.enabled = true;
        }



    }
}