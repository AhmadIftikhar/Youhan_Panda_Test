using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HITAPITEst : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
		StartCoroutine(TestAPiNow());    }

    IEnumerator TestAPiNow()
	{
		UnityWebRequest www = UnityWebRequest.Get("https://assets.cesium.com/121900/tileset.json?v=1");

		//		Debug.Log("Access Tokken is  " + eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJqdGkiOiJjOWZkYzA4OS05MjUyLTQxOTItYjJlZC0yN2Q2NjM2MGNlZDkiLCJpZCI6MzAzNzMsImFzc2V0cyI6eyIxMjE5MDAiOnsidHlwZSI6IjNEVElMRVMifX0sInNyYyI6IjMxYjJmY2UxLTVkOTgtNDljNS05NTgwLWFmY2Y0MjcyZjQxMyIsImlhdCI6MTU5NDM4MzQ0NywiZXhwIjoxNTk0Mzg3MDQ3fQ.O97BQXjn3iY0uLZXXUr8uWvZ7FPFpORj7md14ECXgcY);
		www.SetRequestHeader("Content-type", "application/json");

		www.SetRequestHeader("Authorization", "Bearer " + "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJqdGkiOiJjOWZkYzA4OS05MjUyLTQxOTItYjJlZC0yN2Q2NjM2MGNlZDkiLCJpZCI6MzAzNzMsImFzc2V0cyI6eyIxMjE5MDAiOnsidHlwZSI6IjNEVElMRVMifX0sInNyYyI6IjMxYjJmY2UxLTVkOTgtNDljNS05NTgwLWFmY2Y0MjcyZjQxMyIsImlhdCI6MTU5NDM4MzQ0NywiZXhwIjoxNTk0Mzg3MDQ3fQ.O97BQXjn3iY0uLZXXUr8uWvZ7FPFpORj7md14ECXgcY");
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
			Debug.Log(www.downloadHandler.GetType());
			Debug.Log(www.downloadHandler.text
				);
			;

		//	Test.Root tileset = JsonConvert.DeserializeObject<Test.Root>(www.downloadHandler.text);
	//		Debug.Log(tileset.asset.version);
		}
	}
}
