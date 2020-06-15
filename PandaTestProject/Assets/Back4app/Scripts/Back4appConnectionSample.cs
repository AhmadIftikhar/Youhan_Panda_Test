using UnityEngine;
using System.Collections;
using Parse;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using UnityEngine.Networking;

public class Back4appConnectionSample : MonoBehaviour {
	public IEnumerable<ParseObject> Results { get; private set; }

	async void Start()
	{
		
	/*
		//Methode 1
		ParseQuery<ParseObject> query = ParseObject.GetQuery("CloudAssets");
		await query.GetAsync("DhobA5STFv").ContinueWith(t =>
		 {
			 if (t.IsCompleted)
			 {
				 Result = t.Result;
				string myCustomKey4 = Result.Get<string>("url");
				Debug.Log(myCustomKey4+ " " + Result.Get<float>("realWorldSize"));
			 }
		 });

		//Methode 2 
		ParseQuery<ParseObject> query1 = ParseObject.GetQuery("CloudAssets");
		ParseObject result = await query1.GetAsync("DhobA5STFv");

		string myCustomKey1 = result.Get<string>("url");
		float myCustomKey2 = result.Get<float>("realWorldSize");
		ParseFile x= result.Get<ParseFile>("thumbNail");

	//	x.Url
	*/

		var query = ParseObject.GetQuery("CloudAssets").OrderBy("createdAt");
		
		await query.FindAsync().ContinueWith(t =>
		{
			Results = t.Result;
		});

		foreach (ParseObject po in Results)
		{
			ParseFile pfile =	po.Get<ParseFile>("thumbNail");
			StartCoroutine(GenerateThumbnails(pfile.Url.ToString()));
		}
	}

	private IEnumerator GenerateThumbnails(string url)
	{
		UnityWebRequest www = UnityWebRequest.Get(url);
		yield return www.SendWebRequest();

		if (www.isNetworkError || www.isHttpError)
		{
			Debug.Log(www.error);
		}
		else
		{
			Texture2D myTexture = DownloadHandlerTexture.GetContent(www);
			Sprite sprite = Sprite.Create(myTexture, new Rect(0.0f, 0.0f, myTexture.width, myTexture.height), new Vector2(0.5f, 0.5f), 100.0f);
		}
	}
}
