using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class RetrieveMyCesiumAssetsApi : MonoBehaviour
{

	public class Item
	{
		public int id { get; set; }
		public string type { get; set; }
		public string name { get; set; }
		public string description { get; set; }
		public long bytes { get; set; }
		public string attribution { get; set; }
		public DateTime dateAdded { get; set; }
		public string status { get; set; }
		public int percentComplete { get; set; }
	}

	public class MyCesiumAssets
	{
		public IList<Item> items { get; set; }
	}

	public IEnumerator DownloadCesiumAset()
	{
	

		UnityWebRequest www = UnityWebRequest.Get("https://api.cesium.com/v1/assets");
		www.SetRequestHeader("Authorization", "Basic " + "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJqdGkiOiIxMmQ0YmY1OS03MzkwLTQzM2QtYTQ1Mi03Njg3NGQ0NzA3NmIiLCJpZCI6Mjk5MDYsInNjb3BlcyI6WyJhc2wiLCJhc3IiLCJhc3ciLCJnYyIsInByIl0sImlhdCI6MTU5MjkyOTc0MH0.YvkjuzE7Zq5qWJCLTBM-XXlBGKGEH_K_W5dJRZUaj_8");




		yield return www.SendWebRequest();

		if (www.isNetworkError || www.isHttpError)
		{
			Debug.Log(www.error);
		}
		else
		{
			Debug.Log(www.downloadHandler.text);
			MyCesiumAssets responce = JsonConvert.DeserializeObject<MyCesiumAssets>(www.downloadHandler.text);

			StartCoroutine(DownloadModelOverTime(responce.access_token));

		}
	}
}
