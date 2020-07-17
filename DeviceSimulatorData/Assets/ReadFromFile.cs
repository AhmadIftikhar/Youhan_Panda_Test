using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;


using UnityEditor;
using System.IO;
using Newtonsoft.Json;
using System;

public class ReadFromFile : MonoBehaviour
{
  public string SimulateDataFilePath="";

	SimulateData[] RawData;

	ArrayList deviceids;
	DateTime startdatetime;
	DateTime currentdatetime;
	public Text Currenttimedisplay;
	string Rawstreamdata;
	[MenuItem("Tools/Read file")]
    public void ReadString()
    {
        string path = SimulateDataFilePath;
		StreamReader reader = new StreamReader(path);
		Rawstreamdata =reader.ReadToEnd();
        reader.Close();
    }


	public void Parsestring()
	{
			RawData= JsonConvert.DeserializeObject<SimulateData[]>(Rawstreamdata);
		Debug.Log(RawData[3].time);
	}

	private void Start()
	{
		deviceids = new ArrayList();
		ReadString();
		Parsestring();

		Invoke("StartSimulation",5f);
	}

	void StartSimulation() 
	{

		DateTime.TryParse(RawData[0].time,out startdatetime);
		currentdatetime = startdatetime;
		InvokeRepeating("fetchcurrenttimedevicedata",1,1);
		Debug.Log("datetime parsed"+ startdatetime.ToString() );


	}

	private void fetchcurrenttimedevicedata()
	{
		Currenttimedisplay.text = currentdatetime.ToLongTimeString();


		foreach (SimulateData x in RawData)
		{
			if (deviceids.Contains(x.deviceid))
			{
				Debug.Log("Update");
				//updateDevice;
			}
			else
			{
				Debug.Log("Add");
				deviceids.Add(x.deviceid);
				//	adddevice;
			}

		}
		currentdatetime= currentdatetime.AddSeconds(1);
		;
	}
}
