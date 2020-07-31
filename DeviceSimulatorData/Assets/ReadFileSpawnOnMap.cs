using Mapbox.Examples;
using Mapbox.Platform;
using Mapbox.Unity.Map;
using Mapbox.Unity.MeshGeneration.Data;
using Mapbox.Unity.MeshGeneration.Modifiers;
using Mapbox.Unity.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ReadFileSpawnOnMap : MonoBehaviour
{
	public string SimulateDataFilePath = "";
	public Text Currenttimedisplay;

	public CustomSpawnOnMap SpawnOnMapObject;
	
	[SerializeField]
	AbstractMap _map;

	[SerializeField]
	MeshModifier[] MeshModifiers;
	[SerializeField]
	Material _material;

	string Rawstreamdata;
	Dictionary<string, object>[] RawData;

	public Vector2 singleSize;

	public ScrollRect scroll;

	DateTime startdatetime;
	DateTime currentdatetime;

	GameObject _directionsGO;
	private int _counter;

	ArrayList deviceids; //ids for Devices to update 
	public List<GameObject> _devicesDisplay; //actual objects in prefab on screen instance 

	[SerializeField]
	[Range(1, 10)]
	private float UpdateFrequency = 2;

	public bool containtime = false;
	public bool containcoordinates = false;
	public bool containsGeometry = false;
	public bool containsStartTime = false;
	bool line = false;
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




	void UpdateLine()
	{/*
		var count = _waypoints.Length;
		var wp = new Vector2d[count];
		for (int i = 0; i < count; i++)
		{
			wp[i] = _waypoints[i].GetGeoPosition(_map.CenterMercator, _map.WorldRelativeScale);
		}
		var _directionResource = new DirectionResource(wp, RoutingProfile.Driving);
		_directionResource.Steps = true;
		_directions.Query(_directionResource, HandleDirectionsResponse);*/
	}
	public void ReadString()
	{
		string path = SimulateDataFilePath;
		StreamReader reader = new StreamReader(path);
		Rawstreamdata = reader.ReadToEnd();
		reader.Close();
	}

	public class Geometry
	{
		public string type { get; set; }
		public List<List<double>> coordinates { get; set; }

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

		foreach (KeyValuePair<string, object> keyValue in RawData[0])
		{
			string key = keyValue.Key;
			object value = keyValue.Value;
			if (key == "time")
				containtime = true;
			if (key == "coords")
				containcoordinates = true;
			if (key == "geometry")
				containsGeometry = true;
			if (key == "start_time")
				containsStartTime = true;
		}


		if (containsGeometry)
			_map.OnUpdated += UpdateLine;

		if (containtime)
		{
			DateTime.TryParse(RawData[0].Single(s => s.Key == "time").Value as string, out startdatetime);
		}

		if (containsStartTime)
		{
			DateTime.TryParse(RawData[0].Single(s => s.Key == "start_time").Value as string, out startdatetime);
		}
		currentdatetime = startdatetime;
		InvokeRepeating("fetchcurrenttimedevicedata", 1, 1);
		Debug.Log("datetime parsed" + startdatetime.ToString());


	}



	private void fetchcurrenttimedevicedata()
	{
		
		Currenttimedisplay.text = currentdatetime.ToLongTimeString();
		foreach (Dictionary<string, object> x in RawData)
		{
			DateTime devicetime = new DateTime(); ;
			if (containtime)
				DateTime.TryParse(x.Single(s => s.Key == "time").Value as string, out devicetime);
			if (containsStartTime)
			{
				DateTime.TryParse(RawData[0].Single(s => s.Key == "start_time").Value as string, out devicetime);
			}
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
					if (containcoordinates) //
					{
						object temp = x.Single(s => s.Key == "coords").Value;
						Coords myCoords = JsonConvert.DeserializeObject<Coords>(temp.ToString()); ;
						SpawnOnMapObject.AddLocationstring("" + myCoords.latitude, "" + myCoords.longitude, x);
					}









				}
			}
			if (containsGeometry)  /// if the data has geo points to place objects 
			{
			
				if (!line) /// if the data has geometry to draw on map 
				{
					if (x.Single(s => s.Key == "geometry").Value != null)
					{
						object Newtemp = x.Single(s => s.Key == "geometry").Value;
						Geometry geo = JsonConvert.DeserializeObject<Geometry>(Newtemp.ToString());
						Debug.Log(Newtemp.ToString());
						DrawRespondingLine(geo);
						line = true;
					}
				}


			}
		}
		if(containcoordinates)
			SpawnOnMapObject.SpawnItemsOnMap();
		if(containtime)
		currentdatetime = currentdatetime.AddSeconds(1);
	}

	public void AddDeviceData(GameObject y)
	{
		_devicesDisplay.Add(y);
	}


	void DrawRespondingLine (ReadFileSpawnOnMap.Geometry response)
	{
		

		var meshData = new MeshData();
		var dat = new List<Vector3>();
		foreach (List<double> point in response.coordinates )
		{
			Debug.Log("Adding ponts ");
			dat.Add(Conversions.GeoToWorldPosition(point[1], point[0], _map.CenterMercator, _map.WorldRelativeScale).ToVector3xz());
		}

		var feat = new VectorFeatureUnity();
		feat.Points.Add(dat);

		foreach (MeshModifier mod in MeshModifiers.Where(x => x.Active))
		{
			mod.Run(feat, meshData, _map.WorldRelativeScale);
		}

		CreateGameObject(meshData);
	}

	GameObject CreateGameObject(MeshData data)
	{
		Debug.Log("Create Gameobject called");
		if (_directionsGO != null)
		{
			_directionsGO.Destroy();
		}
		_directionsGO = new GameObject("SingleLine Geometry Draw Attempt " + " entity");
		var mesh = _directionsGO.AddComponent<MeshFilter>().mesh;
		mesh.subMeshCount = data.Triangles.Count;

		mesh.SetVertices(data.Vertices);
		_counter = data.Triangles.Count;
		for (int i = 0; i < _counter; i++)
		{
			var triangle = data.Triangles[i];
			mesh.SetTriangles(triangle, i);
		}

		_counter = data.UV.Count;
		for (int i = 0; i < _counter; i++)
		{
			var uv = data.UV[i];
			mesh.SetUVs(i, uv);
		}

		mesh.RecalculateNormals();
		_directionsGO.AddComponent<MeshRenderer>().material = _material;
		return _directionsGO;
	}




}