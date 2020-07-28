using Mapbox.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.UI;

public class SingleDeviceInfo : MonoBehaviour
{
	Dictionary<string, object> SingleObjectData;
	
	public ScrollRect scroll;
	Vector2 Sizeofsingle;



	public GameObject _singleKeyValueprefab;

	public ArrayList _DictionaryObjects;

	private void Start()
	{
		
	}
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
				go.GetComponent<PopulateDictionaryPrefab>().UpdateMykeyValue(key,value.ToString());
			}
		
		scroll.content.sizeDelta = new Vector2(Sizeofsingle.x, +Sizeofsingle.y * count);
	}

	public void UpdateInfoOfSingleDataObject(Dictionary<string, object> updatevalue) 
	{
		SingleObjectData = updatevalue;
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
