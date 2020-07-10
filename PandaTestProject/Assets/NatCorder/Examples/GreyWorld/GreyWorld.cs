/* 
*   NatCorder
*   Copyright (c) 2019 Yusuf Olokoba
*/

namespace NatCorder.Examples {

    using UnityEngine;
    using UnityEngine.UI;
    using System.Collections;
    using Clocks;

    public class GreyWorld : MonoBehaviour {

        /**
        * GreyWorld Example
        * ------------------
        * This example records a WebCamTexture
        * The WebCamTexture is recorded with a filter applied, using a shader/material
        * In `Update`, we blit the WebCamTexture to encoder surfaces (NatCorder.AcquireFrame) with the greyscale material/shader
        * When the user stops pressing the screen, we revert the greyness and stop recording
        */

        public RawImage rawImage;
        public CameraPreview cameraPreview;

        private MP4Recorder videoRecorder;
        private IClock clock;
        private float greyness;
        private const float GreySpeed = 3f;

        void Update () {
            // Animate the greyness
            if (cameraPreview.cameraTexture && rawImage.texture == cameraPreview.cameraTexture) {
                var currentGreyness = rawImage.material.GetFloat("_Greyness");
                var targetGreyness = Mathf.Lerp(currentGreyness, greyness, GreySpeed * Time.deltaTime);
                rawImage.material.SetFloat("_Greyness", targetGreyness);
            }
            // Record frames
            if (videoRecorder != null && cameraPreview.cameraTexture.didUpdateThisFrame) {
                var frame = videoRecorder.AcquireFrame();
                Graphics.Blit(cameraPreview.cameraTexture, frame, rawImage.material);
                videoRecorder.CommitFrame(frame, clock.Timestamp);
            }
        }

        public void StartRecording () {
            // Become grey
            greyness = 1f;            
            // Start recording
            clock = new RealtimeClock();
            videoRecorder = new MP4Recorder(cameraPreview.width, cameraPreview.height, 30, 0, 0, OnRecording);
        }

        public void StopRecording () {
            // Revert to normal color
            greyness = 0f;
            // Stop recording
            videoRecorder.Dispose();
            videoRecorder = null;
        }

        void OnRecording (string path) {
            Debug.Log("Saved recording to: "+path);
            // Playback the video
            #if UNITY_IOS
            Handheld.PlayFullScreenMovie("file://" + path);
            #elif UNITY_ANDROID
            Handheld.PlayFullScreenMovie(path);
            #endif
        }
    }
}