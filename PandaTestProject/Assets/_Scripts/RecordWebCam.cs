using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.UI;
using UnityEngine.Profiling;
using System;


using NatCorder;
using NatCorder.Inputs;
using NatCorder.Clocks;
using NatMic;
using UnityEngine.Events;
using UnityEngine.Android;

public class RecordWebCam : MonoBehaviour, IAudioProcessor
    {

    [Header("Recording")]
    public int videoWidth;
    public int videoHeight;
    float startTime;
    float endTime;

    public static  float RECORDEDTIME;
    public  RenderTexture replayRenderTexture;

    private IAudioDevice audioDevice;
    private MP4Recorder videoRecorder;
    private IClock recordingClock;
    private CameraInput cameraInput;
    private AudioInput audioInput;
    public UnityEvent OnDoneRecording;



    public RawImage Cameradisplay;
    public Button StartStopCamera;
    public static RecordWebCam instance;
    WebCamTexture cameraTexture;

    //start Camera
    async void Start()
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


	public void RecordonNatCoder()
        {
        //  replayRenderTexture.width = videoWidth;
        //   replayRenderTexture.height = videoHeight;


         startTime = Time.time;

        StartStopCamera.onClick.RemoveListener(RecordonNatCoder);
        StartStopCamera.GetComponentInChildren<Text>().text = "Stop";
        StartStopCamera.onClick.AddListener(stopRecording);

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
   
        cameraInput = new CameraInput(videoRecorder, recordingClock, Camera.main);
        audioDevice = AudioDevice.Devices[0];
        audioDevice.StartRecording(sampleRate, channelCount, this);


    }

    public void OnSampleBuffer(float[] sampleBuffer, int sampleRate, int channelCount, long timestamp)
    {
        // Send sample buffers directly to the video recorder for recording
        videoRecorder.CommitSamples(sampleBuffer, recordingClock.Timestamp);
    }

    private void OnReplay(string path)
    {
        Debug.Log("Saved recording to: " + path);
        Handheld.Vibrate();

        FirebaseCustomStorageManager.recordpath = path;
        RetreivAndPlayVideo.instance.Setpath(path);
        OnDoneRecording.Invoke();
    }





	public async void stopRecording()
    {

        endTime = Time.time;

        RECORDEDTIME=endTime=startTime;
        Debug.Log("Recording stopped after time" +RECORDEDTIME);

        // Stop the recording inputs
        audioDevice.StopRecording();
        cameraInput.Dispose();
        // Stop recording
        videoRecorder.Dispose();
        
        StartStopCamera.onClick.AddListener(RecordonNatCoder);
        StartStopCamera.GetComponentInChildren<Text>().text = "Record";
        StartStopCamera.onClick.RemoveListener(stopRecording);

    }


}
