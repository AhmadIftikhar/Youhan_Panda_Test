using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
public class RetreivAndPlayVideo : MonoBehaviour
{
    public static RetreivAndPlayVideo instance;

    [SerializeField] RawImage outputdisplay;
    [SerializeField] string path;




    [SerializeField] VideoPlayer vPlayer;

    // Update is called once per frame
    private void Start()
	{
        instance = this;

    }
	public void Setpath(string x) 
    {
        path = "file://"+x;
    }


    public void PlayVideoFromPath() 
    {
        float AR = (float) outputdisplay.texture.width / (float)outputdisplay.texture.height;
        Debug.Log(AR);
        outputdisplay.GetComponent<AspectRatioFitter>().aspectRatio = AR;


        vPlayer.url = path;
        vPlayer.Play();

    }
    
}
