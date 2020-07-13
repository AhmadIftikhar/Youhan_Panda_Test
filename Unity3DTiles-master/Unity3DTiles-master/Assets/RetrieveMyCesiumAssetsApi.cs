﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System.IO;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.GZip;
using JetBrains.Annotations;

public class RetrieveMyCesiumAssetsApi : MonoBehaviour
{
	public string JsonPath;
	public ArrayList asseturls = new ArrayList();
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

		//	StartCoroutine(ReadTilesetfromFile());

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
		UnityWebRequest www = UnityWebRequest.Get(responce.url);

		Debug.Log("Access Tokken is  " + responce.accessToken);


		www.SetRequestHeader("Authorization", "Bearer " + responce.accessToken);
		www.SetRequestHeader("Accept-Encoding", "gzip, deflate, br");
		//www.SetRequestHeader("Connection", "keep-alive");
		www.SetRequestHeader("Accept", "*/*");
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
			
			string savepath = Application.dataPath;


#if UNITY_EDITOR
			savepath = savepath.Replace("/Assets", "");
			savepath = savepath + "/Downloaded";
#endif
			savepath = savepath + "/Tileset.gz";

			File.WriteAllBytes(savepath, results);

#if UNITY_EDITOR
			string p1 = Application.dataPath;
			p1 = p1.Replace("/Assets", "");
			p1 = p1 + "/Downloaded";
			// p1 is path to project folder...
#endif
			p1 = p1 + "/Tileset.gz";
			Debug.Log(p1);

			string p2 = Application.dataPath;


#if UNITY_EDITOR
			p2 = p2.Replace("/Assets", "");
			p2 = p2 + "/Downloaded";
			// p1 is path to project folder...
#endif
			ExtractGZipFile(p1, p2);
			//	Test.Root tileset = JsonConvert.DeserializeObject<Test.Root>(www.downloadHandler.text);

			string path = p2+"/Tileset";
			//Read the text from directly from the test.txt file
			StreamReader reader = new StreamReader(path);

			string data = reader.ReadToEnd().ToString();
			Debug.Log(data);
			Test.Root myDeserializedClass = JsonConvert.DeserializeObject<Test.Root>(data);
		
			reader.Close();
		
	///First Base Object
			asseturls.Add(myDeserializedClass.root.content.uri);
			ParseChildren(myDeserializedClass.root.children);

			Debug.Log(asseturls.ToString());

			string Url = responce.url;
			

			Download3dmFiles(Url, responce.accessToken);
		}

	}
	public void Download3dmFiles(string Url,string accesstokken ) 
	{
		foreach (string x in asseturls) 
		{
			string tempurl = Url.Replace("tileset.json", x);
			StartCoroutine(DownnloadTileset3dmodels(tempurl, accesstokken, x));
		}
	}

	private IEnumerator DownnloadTileset3dmodels(string url, string accesstokken, string filename)
	{
		UnityWebRequest www = UnityWebRequest.Get(url);
		www.SetRequestHeader("Authorization", "Bearer " + accesstokken);
		yield return www.SendWebRequest();

		if (www.isNetworkError || www.isHttpError)
		{
			Debug.Log(www.error);
		}
		else
		{
			Byte[] results = www.downloadHandler.data;
			string p1 = Application.dataPath;

#if UNITY_EDITOR
			p1 = p1.Replace("/Assets", "");
			p1 = p1 + "/Downloaded";
			// p1 is path to project folder...
#endif

			string gzippath = p1 + "/"+ filename + ".gz";

			File.WriteAllBytes(gzippath, results);
			string extractpath = p1 ;
			ExtractGZipFile(gzippath, extractpath);
		}
	}

	public void ParseChildren(List<Test.Child> Children) 
	{
		foreach (Test.Child X in Children) 
		{
			asseturls.Add(X.content.uri);
			Debug.Log("Added Object" + X.content.uri+"  Current Count is "+  asseturls.Count);
			if (X.children != null)
			{ 
				ParseChildren(X.children); 
			}
		}
	
	}


	public void ExtractGZipFile(string gzipFileName, string targetDir)
	{
		// Use a 4K buffer. Any larger is a waste.    
		byte[] dataBuffer = new byte[4096];

		using (System.IO.Stream fs = new FileStream(gzipFileName, FileMode.Open, FileAccess.Read))
		{
			using (GZipInputStream gzipStream = new GZipInputStream(fs))
			{
				// Change this to your needs
				string fnOut = Path.Combine(targetDir, Path.GetFileNameWithoutExtension(gzipFileName));

				using (FileStream fsOut = File.Create(fnOut))
				{
					StreamUtils.Copy(gzipStream, fsOut, dataBuffer);
				}
			}
		}
	}




















}


