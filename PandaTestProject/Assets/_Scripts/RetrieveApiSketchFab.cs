using Mapbox.Json;
using Parse;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class RetrieveApiSketchFab : MonoBehaviour
{
	//public Sprite[] DragableImages;
	public string CurrentModelUId = null;

	public GameObject ItemPrefab;
	public GameObject map;
	public ScrollRect scroll;
	public Vector2 singleSize;
	public InputField SearchBaar;
	public IEnumerable<ParseObject> Results { get; private set; }
	public static RetrieveApiSketchFab instance;
	 void Start()
	{
		instance = this;
		singleSize = scroll.content.sizeDelta;
		Destroy(scroll.content.GetChild(0).gameObject);
		
	}


	public void Search() 
	{
		StartCoroutine(sendquerey());
	}
	IEnumerator sendquerey() 
	{
		UnityWebRequest www = UnityWebRequest.Get("https://api.sketchfab.com/v3/search?type=models&q=" + SearchBaar.text+"&downloadable=true");
		
		yield return www.SendWebRequest();

		if (www.isNetworkError || www.isHttpError)
		{
			Debug.Log(www.error);
		}
		else
		{
			Debug.Log(www.downloadHandler.text);

			JSonResponceObject responce = JsonConvert.DeserializeObject<JSonResponceObject>(www.downloadHandler.text);

			int c = 0;
			foreach (Result x in responce.results) 
			{
				System.Uri URl = new Uri(x.thumbnails.images[2].url);
				StartCoroutine(GenerateThumbnails(URl , x.name));
				CurrentModelUId = x.uid;
				c++;
			}


			scroll.content.sizeDelta = new Vector2(singleSize.x, +singleSize.y * c);

		}
/*
		foreach (ParseObject po in Results)
		{

			string pname = po.Get<string>("assetname");

			try
			{
				ParseFile pfile = po.Get<ParseFile>("thumbNail");
				
			}
			catch (KeyNotFoundException e)
			{
				Debug.Log("pictureFile entry not found on " + pname);
				GeneratenoThumbnails(pname);
			}


			
		}
	*/


	}





	private void GeneratenoThumbnails(string pname)
	{
		GameObject go = Instantiate(ItemPrefab, scroll.content);
		go.GetComponent<DropableItem>().SetSprite();
		go.GetComponent<DropableItem>().SetName(pname);
	}

	private IEnumerator GenerateThumbnails(System.Uri url, string description)
	{
		using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(url))
		{
			yield return www.SendWebRequest();

			if (www.isNetworkError || www.isHttpError)
			{
				Debug.Log("Downloaded Failed from " + url);
				Debug.Log(www.error);
			}
			else
			{
				Debug.Log("Downloaded sucessfully from " + url);
				Texture myTexture = DownloadHandlerTexture.GetContent(www);
				Texture2D tex2D = (Texture2D)myTexture;
				Sprite sprite = Sprite.Create(tex2D, new Rect(0.0f, 0.0f, tex2D.width, tex2D.height), new Vector2(0.5f, 0.5f), 100.0f);
				GameObject go = Instantiate(ItemPrefab, scroll.content);
				go.GetComponent<DropableItem>().SetSprite(sprite);
				go.GetComponent<DropableItem>().SetName(description);
			}
		}

	}





	public void DownloadModel() 
	{

		StartCoroutine(Getauthtokken());
	
	}

	public class TokkenClass
	{
		public string token_type { get; set; }
		public string access_token { get; set; }
		public int expires_in { get; set; }
		public string scope { get; set; }
		public string refresh_token { get; set; }
	}

	IEnumerator Getauthtokken()
	{
		WWWForm form = new WWWForm();
		form.AddField("grant_type", "password");
		form.AddField("username", "www.rockingpanda");
		form.AddField("password", "Rocking1182");

		UnityWebRequest www = UnityWebRequest.Post("https://sketchfab.com/oauth2/token/",form);
		www.SetRequestHeader("Authorization", "Basic " + "UktOQlAwUFdhMzh3clNLUjVPeDB6Rk92ejUxZ0VKT2ZuTGlwV1BXUDp1SVR1QXZJUVZ4bW05VG1UOUtQNHlobVhJUzZKYjdqYnRGZlM3MGVyTmVWeHMwRmRJNmk1d2xFTVBucnVqRkZXY2xNcjF4YjE5VU1Yc0FLdzVXVVBzQkltaWVtSkZJWE9OTGZBbExCU1JHN3ZFYXJydHZ0TFFlZjM0VE5NTm5KYQ==");
		



		yield return www.SendWebRequest();
		
		if (www.isNetworkError || www.isHttpError)
		{
			Debug.Log(www.error);
		}
		else
		{
			Debug.Log(www.downloadHandler.text);
			TokkenClass responce = JsonConvert.DeserializeObject<TokkenClass>(www.downloadHandler.text);

			StartCoroutine(DownloadModelOverTime(responce.access_token));
			
		}
	}






	public class Usdz
	{
		public int size { get; set; }
		public int expires { get; set; }
		public string url { get; set; }
	}

	public class Gltf
	{
		public int size { get; set; }
		public int expires { get; set; }
		public string url { get; set; }
	}

	public class RespnceDownloadGltf
	{
		public Usdz usdz { get; set; }
		public Gltf gltf { get; set; }
	}



	IEnumerator DownloadModelOverTime(string auth) 
	{
		if (CurrentModelUId == null)
		{
			yield return null;
		}
		else 
		{
			UnityWebRequest www = UnityWebRequest.Get("https://api.sketchfab.com/v3/models/"+ CurrentModelUId+"/download/");
			www.SetRequestHeader("Authorization", "Bearer " + auth);
			yield return www.SendWebRequest();

			if (www.isNetworkError || www.isHttpError)
			{
				Debug.Log(www.error);
			}
			else
			{
				Debug.Log(www.downloadHandler.text);

			}

			RespnceDownloadGltf responce = JsonConvert.DeserializeObject<RespnceDownloadGltf>(www.downloadHandler.text);

			StartCoroutine(DownloadModelNow(responce.gltf.url, CurrentModelUId+".zip"));

		}
	
	}
		IEnumerator DownloadModelNow(string url, string filename)
		{

		WebClient client = new WebClient();
		String path = "@" + Application.dataPath.ToString() + "/" + filename;
		Debug.Log(path);
		client.DownloadFile(url,filename);
		
		yield return null;
/*
		UnityWebRequest www = UnityWebRequest.Get(url);
		yield return www.SendWebRequest();
		if (www.isNetworkError || www.isHttpError)
		{
			Debug.Log(www.error);
		}
		else
		{
			Debug.Log(www.downloadHandler.text);

		}*/
	}
	}
