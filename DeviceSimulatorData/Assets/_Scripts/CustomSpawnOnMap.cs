namespace Mapbox.Examples
{
	using UnityEngine;
	using Mapbox.Utils;
	using Mapbox.Unity.Map;
	using Mapbox.Unity.MeshGeneration.Factories;
	using Mapbox.Unity.Utilities;
	using System.Collections.Generic;
	using System.Collections;

	public class CustomSpawnOnMap : MonoBehaviour
	{
		[SerializeField]
		AbstractMap _map;
		public ReadFileSpawnOnMap rfsom;
		[SerializeField]
		[Geocode]
		string[] _locationStrings;
		Vector2d[] _locations;

		[SerializeField]
		float _spawnScale = 100f;

		[SerializeField]
		GameObject _markerPrefab;

		List<GameObject> _spawnedObjects;
		public ArrayList CustomLatLongs;

		public ArrayList Dictionaries;
		public bool Started ;
		void Start() 
		{
			CustomLatLongs = new ArrayList();
			Dictionaries = new ArrayList();
				Started = false;
		}

		public void AddLocationstring(string lat,string  Long,Dictionary<string,object> dict) 
		{
			CustomLatLongs.Add(lat + "," + Long);
			Dictionaries.Add(dict);

		}


		public void SpawnItemsOnMap()
		{
			if (!Started)
			{
				_locationStrings = new string[CustomLatLongs.Count];

				for (int i=0;i< CustomLatLongs.Count;i++) 
				{
					_locationStrings[i] = CustomLatLongs[i].ToString();
				}

				Started = true;
				_locations = new Vector2d[_locationStrings.Length];
				_spawnedObjects = new List<GameObject>();
				
				for (int i = 0; i < _locationStrings.Length; i++)
				{
					var locationString = _locationStrings[i];
					_locations[i] = Conversions.StringToLatLon(locationString);
					var instance = Instantiate(_markerPrefab);
					instance.transform.localPosition = _map.GeoToWorldPosition(_locations[i], true);
					instance.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
					_spawnedObjects.Add(instance);
					rfsom.AddDeviceData(instance.gameObject);

					instance.GetComponent<SingleDeviceInfo>().UpdateInfoOfSingleDataObject(Dictionaries[i] as Dictionary<string,object>);
					instance.GetComponent<SingleDeviceInfo>().Initialize();
				}
			}

		}

		private void Update()
		{	if (!Started)
				return;
			int count = _spawnedObjects.Count;
			for (int i = 0; i < count; i++)
			{
				var spawnedObject = _spawnedObjects[i];
				var location = _locations[i];
				spawnedObject.transform.localPosition = _map.GeoToWorldPosition(location, true);
				spawnedObject.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
			}
		}
	}
}