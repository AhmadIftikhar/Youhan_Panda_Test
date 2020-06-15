using Parse;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Threading.Tasks;
using System;

public class RetrieveApi : MonoBehaviour
{

	//public Sprite[] DragableImages;

	public GameObject ItemPrefab;
	public GameObject map;
	public ScrollRect scroll;
	public Vector2 singleSize;

	public IEnumerable<ParseObject> Results { get; private set; }

	async void Start()
	{

		singleSize = scroll.content.sizeDelta;
		Destroy(scroll.content.GetChild(0).gameObject);


		var query = ParseObject.GetQuery("CloudAssets").OrderBy("createdAt");

		await query.FindAsync().ContinueWith(t =>
		{
			Results = t.Result;
		});

		int c = 0;
		foreach (ParseObject po in Results)
		{
		
			string pname = po.Get<string>("assetname");
		
			try
			{
				ParseFile pfile = po.Get<ParseFile>("thumbNail");
				 StartCoroutine(GenerateThumbnails(pfile.Url, pname));
			}
			catch (KeyNotFoundException e)
			{
				Debug.Log("pictureFile entry not found on " + pname);
				GeneratenoThumbnails(pname);
			}
		
		
			c++;
		}
		scroll.content.sizeDelta = new Vector2(singleSize.x, +singleSize.y * c);
	}

	private void GeneratenoThumbnails(string pname)
	{
		GameObject go = Instantiate(ItemPrefab, scroll.content);
		go.GetComponent<DropableItem>().SetSprite();
		go.GetComponent<DropableItem>().SetName(pname);
	}

	private IEnumerator GenerateThumbnails(System.Uri url,string description)
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
	
	
		}
	
	

