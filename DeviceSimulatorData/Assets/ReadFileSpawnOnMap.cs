using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System;
using Mapbox.Examples;

public class ReadFileSpawnOnMap : MonoBehaviour
{
	public string SimulateDataFilePath = "";
	public Text Currenttimedisplay;

	public CustomSpawnOnMap SpawnOnMapObject;

	string Rawstreamdata;
	Dictionary<string, object>[] RawData;

	public Vector2 singleSize;

	public ScrollRect scroll;

	DateTime startdatetime;
	DateTime currentdatetime;


	ArrayList deviceids; //ids for Devices to update 
	public List<GameObject> _devicesDisplay; //actual objects in prefab on screen instance 



	public ArrayList Objectkeys;
	private void Start()
	{
		Objectkeys = new ArrayList();
		deviceids = new ArrayList();
		ReadString();
		Parsestring();

		singleSize = scroll.content.sizeDelta;
		Destroy(scroll.content.GetChild(0).gameObject);
		Invoke("StartSimulation", 5f);
	}
	public void ReadString()
	{
		string path = SimulateDataFilePath;
		StreamReader reader = new StreamReader(path);
		Rawstreamdata = reader.ReadToEnd();
		reader.Close();
	}


	public class Coords
	{
		public double latitude { get; set; }
		public double longitude { get; set; }

	}
	public void Parsestring()
	{
		RawData = JsonConvert.DeserializeObject<Dictionary<string, object>[]>(Rawstreamdata);

		//Prepare prefab to spawn based on 1 Object

		foreach (KeyValuePair<string, object> keyValue in RawData[0])
		{
			string key = keyValue.Key;
			object value = keyValue.Value;

			Objectkeys.Add(key);
			Debug.Log("loop KEY " + key);
			Debug.Log("loop OBJECT " + value.ToString());
		}

		object Obj = RawData[0].Single(s => s.Key == "deviceid").Value;
		Debug.Log(Obj.ToString());


		object Obj1 = RawData[0].Single(s => s.Key == "type").Value;

		//object Obj2 = RawData[0].Single( s.key );

	}



	void StartSimulation()
	{
		DateTime.TryParse(RawData[0].Single(s => s.Key == "time").Value as string, out startdatetime);
		currentdatetime = startdatetime;
		InvokeRepeating("fetchcurrenttimedevicedata", 1, 1);
		Debug.Log("datetime parsed" + startdatetime.ToString());


	}



	private void fetchcurrenttimedevicedata()
	{
		Currenttimedisplay.text = currentdatetime.ToLongTimeString();
			foreach (Dictionary<string,object> x in RawData)
			{
				DateTime devicetime = new DateTime(); ;
				DateTime.TryParse(x.Single(s => s.Key == "time").Value as string, out devicetime);



				if (devicetime == currentdatetime)
				{
					if (deviceids.Contains(x.Single(s => s.Key == "deviceid").Value as string))
					{
						Debug.Log("Update");

						foreach (GameObject device in _devicesDisplay) 
						{


						Debug.Log(device.name);
						device.GetComponent<SingleDeviceInfo>().UpdateRealtime(x);
						// TO do update Device data 
						}
					}
					else
					{
						Debug.Log("Add");

						deviceids.Add(x.Single(s => s.Key == "deviceid").Value as string);
						object temp =  x.Single(s => s.Key == "coords").Value;
						Debug.Log(temp.ToString());
						Coords myCoords = JsonConvert.DeserializeObject<Coords>(temp.ToString()); ;
						SpawnOnMapObject.AddLocationstring(""+myCoords.latitude,""+ myCoords.longitude ,x);
					}
				}

			}
				SpawnOnMapObject.SpawnItemsOnMap(); 
				currentdatetime = currentdatetime.AddSeconds(1);
	}

	public void AddDeviceData(GameObject y) 
	{
		_devicesDisplay.Add(y);
	}



}