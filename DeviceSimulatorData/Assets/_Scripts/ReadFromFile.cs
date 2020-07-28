using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.IO;
using Newtonsoft.Json;
using System;
using UnityEngine.Experimental.UIElements;
using System.Data;
using System.Linq;

public class ReadFromFile : MonoBehaviour
{

	[Serializable]
	public enum FileType
	{
		airquality_fire_data,
		fire_traffic_closure,
		fire_traffic_conditions,
		firehotspots_generated_fire_data,
		firstrespondervitals_fire,
		objects_generated_fire_data,
		people_generated_fire_data,
		wind_generated_fire_data
	}




	public FileType typeoffile;

	public string SimulateDataFilePath="";
	public GameObject _prefab;

	public ScrollRect scroll;
	public Vector2 singleSize;

//	List se

	Dictionary<string ,object>[] RawData;
	public List<GameObject> _devicesDisplay;

	ArrayList deviceids;
	DateTime startdatetime;
	DateTime currentdatetime;
	public Text Currenttimedisplay;
	string Rawstreamdata;


    public void ReadString()
    {
        string path = SimulateDataFilePath;
		StreamReader reader = new StreamReader(path);
		Rawstreamdata =reader.ReadToEnd();
        reader.Close();
    }


	public void Parsestring()
	{
		RawData = JsonConvert.DeserializeObject<Dictionary<string, object>[]>(Rawstreamdata);
		Debug.Log(RawData[0].Keys.Count);
		//		Debug.Log(RawData[0].);

		/*foreach (Dictionary<string, object> item in RawData)
		{


			
//			foreach (return  string x as item.Keys.AsEnumerable);

			Debug.Log("loop");
		}
		*/
		foreach (KeyValuePair<string, object> keyValue in RawData[0])
		{
			string key = keyValue.Key;
			object value = keyValue.Value;

			Debug.Log("loop KEY " + key);
			Debug.Log("loop OBJECT " + value.ToString());
		}

		object Obj = RawData[0].Single(    s => s.Key == "deviceid").Value;
		Debug.Log(Obj.ToString());
		
		
		object Obj1 = RawData[0].Single(s => s.Key == "type").Value;

		object Obj2 = RawData[0].Single(s => s.Key == "type").Value;
		object Obj3 = RawData[0].Single(s => s.Key == "type").Value;

		//object Obj2 = RawData[0].Single( s.key );




		


	}

	private void Start()
	{
		deviceids = new ArrayList();
		ReadString();
		Parsestring();

		singleSize = scroll.content.sizeDelta;
		Destroy(scroll.content.GetChild(0).gameObject);

		//scroll.contentView.getch

		Invoke("StartSimulation",5f);
	}

	void StartSimulation() 
	{

	//	DateTime.TryParse(RawData[0].time,out startdatetime);
		currentdatetime = startdatetime;
		InvokeRepeating("fetchcurrenttimedevicedata",1,1);
		Debug.Log("datetime parsed"+ startdatetime.ToString() );


	}

	private void fetchcurrenttimedevicedata()
	{
		Currenttimedisplay.text = currentdatetime.ToLongTimeString();
		/*	foreach (AerialThermalDetector x in RawData)
			{
				DateTime devicetime = new DateTime(); ;
				DateTime.TryParse(x.time, out devicetime);
				if (devicetime == currentdatetime)
				{
					if (deviceids.Contains(x.deviceid))
					{
						Debug.Log("Update");

						foreach (GameObject device in _devicesDisplay) 
						{
							if (device.GetComponent<FIrescenarioDataView>().deviceid.text == x.deviceid)
							{
								device.GetComponent<FIrescenarioDataView>().UpdateThisView(x.deviceid, x.type, x.time, x.o2.value.ToString(), x.o2.units.ToString(), x.o2.max_threshold.ToString(), x.o2.min_threshold.ToString(), x.co.value.ToString(), x.co.units, x.co.max_threshold.ToString(), x.h2s.value.ToString(), x.h2s.units, x.h2s.max_threshold.ToString(), x.hcn.value.ToString(), x.hcn.units, x.hcn.max_threshold.ToString(), x.lel.value.ToString(), x.lel.units, x.lel.max_threshold.ToString(), x.particulate.value.ToString(), x.particulate.units, x.particulate.max_threshold.ToString(), x.coords.latitude.ToString(), x.coords.longitude.ToString());
							}
						}

						//updateDevice;
					}
					else
					{
						Debug.Log("Add");
						deviceids.Add(x.deviceid);

						GameObject y = Instantiate(_prefab, scroll.content.transform);
						y.GetComponent<FIrescenarioDataView>().UpdateThisView(x.deviceid, x.type, x.time, x.o2.value.ToString(), x.o2.units, x.o2.max_threshold.ToString(), x.o2.min_threshold.ToString(), x.co.value.ToString(), x.co.units, x.co.max_threshold.ToString(), x.h2s.value.ToString(), x.h2s.units, x.h2s.max_threshold.ToString(), x.hcn.value.ToString(), x.hcn.units, x.hcn.max_threshold.ToString(), x.lel.value.ToString(), x.lel.units, x.lel.max_threshold.ToString(), x.particulate.value.ToString(), x.particulate.units, x.particulate.max_threshold.ToString(), x.coords.latitude.ToString(), x.coords.longitude.ToString());
						_devicesDisplay.Add(y);
						scroll.content.sizeDelta = new Vector2(singleSize.x, +singleSize.y * deviceids.Count);
					}
				}

			}
			currentdatetime= currentdatetime.AddSeconds(1);
			;*/
	}
}
