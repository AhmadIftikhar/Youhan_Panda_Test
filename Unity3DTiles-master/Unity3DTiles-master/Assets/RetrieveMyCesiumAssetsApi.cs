using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System.IO;

public class RetrieveMyCesiumAssetsApi : MonoBehaviour
{
	public string JsonPath;
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
	private void Start()
	{
		StartCoroutine(DownloadCesiumAset());
	}
	public IEnumerator DownloadCesiumAset()
	{
	

		UnityWebRequest www = UnityWebRequest.Get("https://api.cesium.com/v1/assets");
		www.SetRequestHeader("Authorization", "Bearer " + "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJqdGkiOiIzMWIyZmNlMS01ZDk4LTQ5YzUtOTU4MC1hZmNmNDI3MmY0MTMiLCJpZCI6MzAzNzMsInNjb3BlcyI6WyJhc2wiLCJhc3IiLCJhc3ciLCJnYyIsInByIl0sImlhdCI6MTU5NDMxNzMwMX0.4UsZW9AtQBFQFzdFbQMf_TY-42ZyUK-7kAwJE8BUZLI");


		yield return www.SendWebRequest();

		if (www.isNetworkError || www.isHttpError)
		{
			Debug.Log(www.error);
		}
		else
		{
			Debug.Log(www.downloadHandler.text);
			MyCesiumAssets responce = JsonConvert.DeserializeObject<MyCesiumAssets>(www.downloadHandler.text);

			StartCoroutine(GetAssetTokken());
		}
	}



	public class Attribution
	{
		public string html { get; set; }
		public bool collapsible { get; set; }
	}

	public class AccessTokkenClass
	{
		public string type { get; set; }
		public string url { get; set; }
		public string accessToken { get; set; }
		public IList<Attribution> attributions { get; set; }
	}



	private IEnumerator GetAssetTokken()
	{
		UnityWebRequest www = UnityWebRequest.Get("https://api.cesium.com/v1/assets/121900/endpoint");
		www.SetRequestHeader("Authorization", "Bearer " + "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJqdGkiOiIzMWIyZmNlMS01ZDk4LTQ5YzUtOTU4MC1hZmNmNDI3MmY0MTMiLCJpZCI6MzAzNzMsInNjb3BlcyI6WyJhc2wiLCJhc3IiLCJhc3ciLCJnYyIsInByIl0sImlhdCI6MTU5NDMxNzMwMX0.4UsZW9AtQBFQFzdFbQMf_TY-42ZyUK-7kAwJE8BUZLI");
		yield return www.SendWebRequest();

		if (www.isNetworkError || www.isHttpError)
		{
			Debug.Log(www.error);
		}
		else
		{
			Debug.Log(www.downloadHandler.text);
			AccessTokkenClass responce = JsonConvert.DeserializeObject<AccessTokkenClass>(www.downloadHandler.text);

			StartCoroutine(ReadTilesetfromFile());

			StartCoroutine(DownnloadTileset(responce));
		}
	}




	private IEnumerator ReadTilesetfromFile() 
	{
	/*	using (StreamReader r = new StreamReader("file.json"))
		{
			string json = r.ReadToEnd();
			List<Item> items = JsonConvert.DeserializeObject<List<Item>>(json);


		}*/


		UnityWebRequest www = UnityWebRequest.Get(JsonPath);
		yield return www.SendWebRequest();
		if (www.isNetworkError || www.isHttpError)
		{
			Debug.Log(www.error);
		}
		else
		{

			//		Debug.Log(www.downloadHandler.data);
			Debug.Log(www.downloadHandler.text);
			Test.Root myDeserializedClass = JsonConvert.DeserializeObject<Test.Root>(www.downloadHandler.text);
			Test.Root tileset = JsonConvert.DeserializeObject<Test.Root>(www.downloadHandler.text);
			Debug.Log(myDeserializedClass.asset.version);
		}

	}








	private IEnumerator DownnloadTileset(AccessTokkenClass responce)
	{
		Debug.Log("Hitting APi "+responce.url);
		UnityWebRequest www = UnityWebRequest.Get("https://assets.cesium.com/121900/tileset.json?v=1");

		Debug.Log("Access Tokken is  " + responce.accessToken);

		www.SetRequestHeader("Authorization", "Bearer " + responce.accessToken);
		yield return www.SendWebRequest();

		if (www.isNetworkError || www.isHttpError)
		{
			Debug.Log(www.error);
		}
		else
		{
			if (www.isDone)
				//		Debug.Log(www.downloadHandler.data);
				Debug.Log(www.downloadProgress);
			Debug.Log(www.downloadHandler.data);

			Debug.Log(www.downloadHandler.text);

			Byte[] results = www.downloadHandler.data;
			
			File.WriteAllBytes("Tileset.json", results);
		//	using (var stream = new MemoryStream(results))
		/*	using (var binaryStream = new BinaryReader(stream))
			{
				DoStuffWithBinaryStream(binaryStream);
			}*/


			Test.Root tileset = JsonConvert.DeserializeObject<Test.Root>(www.downloadHandler.text);
		//	Debug.Log(tileset.asset.version);
		}

	}

























}


