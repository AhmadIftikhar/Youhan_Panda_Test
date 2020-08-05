using Mapbox.Examples;
using Mapbox.Unity.Map;
using Mapbox.Unity.MeshGeneration.Data;
using Mapbox.Unity.MeshGeneration.Modifiers;
using Mapbox.Unity.Utilities;
using Mapbox.Utils;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.UI;

public class SingleDeviceInfo : MonoBehaviour
{
	Dictionary<string, object> SingleObjectData;
	private int _counter;
	public 	GameObject _directionsGO;
	
	public ScrollRect scroll;
	Vector2 Sizeofsingle;
	public string DeviceID;


	public GameObject _singleKeyValueprefab;

	public ArrayList _DictionaryObjects;
	bool containsGeometry;//flag to drawLine;
	private void Start()
	{
		containsGeometry = false;
	}
	/// <summary>
	/// update the single Device for First Time 
	/// </summary>
	/// <param name="updatevalue"></param>
	public void UpdateInfoOfSingleDataObject(Dictionary<string, object> updatevalue)
	{
		SingleObjectData = updatevalue;
	}


	/// <summary>
	/// Initialize the device on map 
	/// </summary>
	public void Initialize()
	{
		_DictionaryObjects = new ArrayList();
		Sizeofsingle = scroll.content.sizeDelta;
		Destroy(scroll.content.GetChild(0).gameObject);
		int count = SingleObjectData.Count;
		

			
			foreach (KeyValuePair<string, object> keyValue in SingleObjectData)
			{GameObject go = Instantiate(_singleKeyValueprefab, scroll.content.transform);
			_DictionaryObjects.Add(go);
				string key = keyValue.Key;
				object value = keyValue.Value;
			if (key == "deviceid")
			{
				DeviceID = value.ToString();

			}

			if (key == "geometry")
			{
				containsGeometry = true;
				object Newtemp =value;
				ReadFileSpawnOnMap.Geometry geo = JsonConvert.DeserializeObject<ReadFileSpawnOnMap.Geometry>(Newtemp.ToString());
				Debug.Log(Newtemp.ToString());
				DrawRespondingLine(geo);
			}

			go.GetComponent<PopulateDictionaryPrefab>().UpdateMykeyValue(key,value.ToString());
			}
	
		scroll.content.sizeDelta = new Vector2(Sizeofsingle.x, +Sizeofsingle.y * count);
	}

	void DrawRespondingLine(ReadFileSpawnOnMap.Geometry response)
	{

AbstractMap _map = CustomSpawnOnMap.instance._map;

		//_map.OnUpdated += UpdateLine;
		var meshData = new MeshData();
		var dat = new List<Vector3>();
		foreach (List<double> point in response.coordinates)
		{
			Debug.Log("Adding ponts ");
			
			dat.Add(Conversions.GeoToWorldPosition(point[1], point[0],_map.CenterMercator, _map.WorldRelativeScale).ToVector3xz());
		}

		var feat = new VectorFeatureUnity();
		feat.Points.Add(dat);

		foreach (MeshModifier mod in CustomSpawnOnMap.instance.MeshModifiers.Where(x => x.Active))
		{
			mod.Run(feat, meshData, _map.WorldRelativeScale);
		}

		GameObject go = 	CreateGameObject(meshData);
		go.transform.SetParent(this.gameObject.transform);
	}
	/*
	void UpdateRespondingLine(ReadFileSpawnOnMap.Geometry response) 
	{
		AbstractMap _map = CustomSpawnOnMap.instance._map;
		var meshData = new MeshData();
		var dat = new List<Vector3>();
		foreach (List<double> point in response.coordinates)
		{
			Debug.Log("Adding ponts ");

			dat.Add(Conversions.GeoToWorldPosition(point[1], point[0], _map.CenterMercator, _map.WorldRelativeScale).ToVector3xz());
		}

		var feat = new VectorFeatureUnity();
		feat.Points.Add(dat);

		foreach (MeshModifier mod in CustomSpawnOnMap.instance.MeshModifiers.Where(x => x.Active))
		{
			mod.Run(feat, meshData, _map.WorldRelativeScale);
		}
		MeshData data = meshData;
		var mesh = _directionsGO.GetComponent<MeshFilter>().mesh;
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


	}
	private void UpdateLine()
	{
		Debug.Log("Updating size");

		foreach (KeyValuePair<string, object> keyValue in SingleObjectData)
		{
			string key = keyValue.Key;
			object value = keyValue.Value;
			if (key == "geometry")
			{
			
				object Newtemp = value;
				ReadFileSpawnOnMap.Geometry geo = JsonConvert.DeserializeObject<ReadFileSpawnOnMap.Geometry>(Newtemp.ToString());
				UpdateRespondingLine(geo);
			}
		}
	}
	*/
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
		_directionsGO.AddComponent<MeshRenderer>().material = CustomSpawnOnMap.instance._material;
		return _directionsGO;
	}




	public void UpdateRealtime(Dictionary<string, object> ObjectData) 
	{ 
		 SingleObjectData=ObjectData;

		int count = 0;
		foreach (KeyValuePair<string, object> keyValue in SingleObjectData)
		{
			string key = keyValue.Key;
			object value = keyValue.Value;
			GameObject x = _DictionaryObjects[count] as GameObject;
			x.GetComponent<PopulateDictionaryPrefab>().UpdateMykeyValue(key, value.ToString());
			count++;
		}
	}



	
}
