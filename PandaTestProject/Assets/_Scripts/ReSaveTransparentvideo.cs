using NatCorder;
using NatCorder.Clocks;
using NatCorder.Inputs;
using NatMic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.Video;

public class ReSaveTransparentvideo : MonoBehaviour, IAudioProcessor
{
    Animator anim;
    public VideoPlayer video;

    [Header("Recording")]
    public int videoWidth;
    public int videoHeight;

    private IAudioDevice audioDevice;
    private MP4Recorder videoRecorder;
    private IClock recordingClock;
    private CameraInput cameraInput;
    private AudioInput audioInput;
    public UnityEvent OnDoneRecording;
    public Camera cam;

    public RawImage refrerenceimage;
    public RawImage Cameradisplay;

    public RawImage SourceImage;

	//to do aspect ratio fit 
	private void Update()
	{
        Cameradisplay.texture = SourceImage.texture;
    }
	public void RerecordWIthTransparency() 
    {

       
        Cameradisplay.GetComponent<AspectRatioFitter>().aspectRatio= refrerenceimage.GetComponent<AspectRatioFitter>().aspectRatio;
        video.Stop();
        video.Play();
      
        Invoke("stopRecording", RecordWebCam.RECORDEDTIME );
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

        cameraInput = new CameraInput(videoRecorder, recordingClock, cam);
        audioDevice = AudioDevice.Devices[0];
        audioDevice.StartRecording(sampleRate, channelCount, this);

    }





    public async void stopRecording()
    {
        Debug.Log("Recording stopped");
        video.Stop();

        // Stop the recording inputs
        audioDevice.StopRecording();
        cameraInput.Dispose();
        // Stop recording
        videoRecorder.Dispose();

     

    }




    public void OnSampleBuffer(float[] sampleBuffer, int sampleRate, int channelCount, long timestamp)
    {
        // Send sample buffers directly to the video recorder for recording
        videoRecorder.CommitSamples(sampleBuffer, recordingClock.Timestamp);
    }
    private void OnReplay(string path)
    {
        Debug.Log("Saved Transparent recording to: " + path);
        Handheld.Vibrate();  
        FirebaseCustomStorageManager.transpath = path;OnDoneRecording.Invoke();
       // RetreivAndPlayVideo.instance.Setpath(path);
      

    }
}
