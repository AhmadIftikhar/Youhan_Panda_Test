using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.UI;
using UnityEngine.Profiling;
using System;
using NatSuite.Recorders;
using NatSuite.Recorders.Inputs;
using NatSuite.Recorders.Clocks;

public class RecordWebCam : MonoBehaviour
    {
        public RawImage Cameradisplay;
        public Button StartStopCamera;
    
        CameraInput input;
        MP4Recorder recorder;
        IClock clock;
        bool recording;
        Color32[] pixelBuffer;

    WebCamTexture cameraTexture;

    //start Camera
    async void Start()
        {
            // Start the webcam
            cameraTexture = new WebCamTexture();
            cameraTexture.Play();

        StartStopCamera.GetComponentInChildren<Text>().text = "Start";
        StartStopCamera.onClick.AddListener(RecordonNatCoder);
    
            Debug.Log(cameraTexture.width);
            Debug.Log(cameraTexture.height);

            //fix streatching
            float AR = (float)cameraTexture.width / (float)cameraTexture.height;
            Debug.Log(AR);
            Cameradisplay.GetComponent<AspectRatioFitter>().aspectRatio = AR;
            Cameradisplay.texture = cameraTexture;
        }

	private void Update()
	{
        if (recording && cameraTexture.didUpdateThisFrame)
        {
            cameraTexture.GetPixels32(pixelBuffer);
            recorder.CommitFrame(pixelBuffer, clock.timestamp);
        }
    }
	public void RecordonNatCoder()
        {

        StartStopCamera.onClick.RemoveListener(RecordonNatCoder);

        StartStopCamera.GetComponentInChildren<Text>().text = "Stop";

        StartStopCamera.onClick.AddListener(stopRecording);


         recorder  = new MP4Recorder(Cameradisplay.texture.width, Cameradisplay.texture.width, 30);
         clock = new RealtimeClock();
        pixelBuffer = cameraTexture.GetPixels32();
        recording = true;
     

    }

    public async void stopRecording()
    {
        StartStopCamera.onClick.RemoveListener(stopRecording);
        StartStopCamera.onClick.AddListener(RecordonNatCoder);
        StartStopCamera.GetComponentInChildren<Text>().text = "Record";
      
        recording = false;
        var path = await recorder.FinishWriting();
        // Playback recording
        Debug.Log($"Saved recording to: {path}");
        var prefix = Application.platform == RuntimePlatform.IPhonePlayer ? "file://" : "";
        Handheld.PlayFullScreenMovie($"{prefix}{path}");
    }


}
