using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FirebaseCustomStorageManager : MonoBehaviour
{
	[SerializeField]
	string StorageGsURL;
	Firebase.Storage.FirebaseStorage storage;
	public Firebase.Auth.FirebaseAuth auth;
	public Firebase.Auth.FirebaseUser user;
	public static FirebaseCustomStorageManager instance;

	public UnityEvent NotLoggedIN;
	public UnityEvent LoggedIN;


	public static string recordpath;
	public static string transpath;

	public bool UseUploadMethod;
	// Start is called before the first frame update


	void Awake()
	{
		InitializeFirebase();
	}

	void Start()
	{
		auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
		if (instance == null)
		{
			instance = this;
		}


		storage = Firebase.Storage.FirebaseStorage.DefaultInstance;
		if (StorageGsURL != null)
		{
			Firebase.Storage.StorageReference storage_ref = storage.GetReferenceFromUrl(StorageGsURL);
		}
	}

	void InitializeFirebase()
	{
		auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
		auth.StateChanged += AuthStateChanged;
		AuthStateChanged(this, null);
	}


	void AuthStateChanged(object sender, System.EventArgs eventArgs)
	{
		if (auth.CurrentUser != user)
		{
			bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;
			if (!signedIn && user != null)
			{
				Debug.Log("Signed out " + user.UserId);
				NotLoggedIN.Invoke();
			}
			user = auth.CurrentUser;
			if (signedIn)
			{
				LoggedIN.Invoke();
				Debug.Log("Signed in " + user.UserId);
			}
			else
			{
				NotLoggedIN.Invoke();
			}
		}
	}

	public void SignInanonymously() 
	{
		auth.SignInAnonymouslyAsync().ContinueWith(task => {
		if (task.IsCanceled)
		{
			Debug.LogError("SignInAnonymouslyAsync was canceled.");
			return;
		}
		if (task.IsFaulted)
		{
			Debug.LogError("SignInAnonymouslyAsync encountered an error: " + task.Exception);
			return;
		}

		Firebase.Auth.FirebaseUser newUser = task.Result;
		Debug.LogFormat("User signed in successfully: {0} ({1})",
			newUser.DisplayName, newUser.UserId);
		});
	}


	public void UploadFile(string path)
	{
		Debug.Log("UploadFilecalled");
		storage = Firebase.Storage.FirebaseStorage.DefaultInstance;
		// Create a root reference
		Firebase.Storage.StorageReference storage_rootref = storage.RootReference;
		// Create a reference to "mountains.jpg"	
		auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
		// While the file names are the same, the references point to different files
		//mountains_ref.Name == mountain_images_ref.Name; // true
		//mountains_ref.Path == mountain_images_ref.Path; // false

		// Create a reference to the file you want to upload
		string filename = Path.GetFileName(path);
		Firebase.Storage.StorageReference uploadPath = storage_rootref.Child("Users/" + user.UserId + "/SimpleRecorded/" + filename);

		StartCoroutine(uploadFileNow(path, uploadPath));

	}
	public void UploadTransFile(string path)
	{
		Debug.Log("UploadFilecalled");
		storage = Firebase.Storage.FirebaseStorage.DefaultInstance;
		// Create a root reference
		Firebase.Storage.StorageReference storage_rootref = storage.RootReference;
		// Create a reference to "mountains.jpg"	
		auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
		// While the file names are the same, the references point to different files
		//mountains_ref.Name == mountain_images_ref.Name; // true
		//mountains_ref.Path == mountain_images_ref.Path; // false

		// Create a reference to the file you want to upload
		string filename = Path.GetFileName(path);
		Firebase.Storage.StorageReference uploadPath = storage_rootref.Child("Users/" + user.UserId + "/Transparent/" + filename);

		StartCoroutine(uploadTransFileNow(path, uploadPath));

	}

	public void UploadToIrebaseSTorage() 
	{
		if (UseUploadMethod) 
		{
			UploadFile(recordpath);
		}
	}

	public void UploadTransparentToIrebaseSTorage()
	{
		if (UseUploadMethod)
		{
			UploadTransFile(transpath);
		}
	}



	IEnumerator uploadTransFileNow(string path, Firebase.Storage.StorageReference uploadPath)
	{

		string local_file = path;

		// Upload the file to the path "images/rivers.jpg"
		System.Threading.Tasks.Task<Firebase.Storage.StorageMetadata> uploadtask = uploadPath.PutFileAsync(local_file);
		yield return new YieldTask(uploadtask);
		if (uploadtask.IsCompleted)
		{


			Debug.Log("File UPLOADED SUCESSFULLY");

			System.Threading.Tasks.Task<System.Uri> geturl = uploadPath.GetDownloadUrlAsync();
			yield return new YieldTask(geturl);
			string DOWNLOADURL = "";
			if (!geturl.IsFaulted && !geturl.IsCanceled)
			{
				DOWNLOADURL = geturl.Result.ToString();
				Debug.Log("Download URL: " + geturl.Result.ToString());

				//	ScreenCapture.instance.ShareUrl = DOWNLOADURL;
			}
			else
			{
				DOWNLOADURL = auth.CurrentUser.PhotoUrl.ToString();
				Debug.Log("Download URL NOT FOUND");
			}
			Firebase.Storage.StorageMetadata metadata = uploadtask.Result;
			Debug.Log("Finished uploading...");

			//					Debug.Log("download url = " + download_url);
			DatabaseReference reference;
			reference = FirebaseDatabase.DefaultInstance.RootReference;
			FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://youhanpandatest.firebaseio.com/");


			reference.Child("Users").Child(user.UserId).Child("RecordedTransparentVideo").SetValueAsync(DOWNLOADURL);
			//	Profilemanagement.instance.UserImageurl = DOWNLOADURL;
		}
		else if (uploadtask.IsFaulted)
		{
			//	UIScreenManager.instance.TransitionFromscreen("Loading");

			Debug.Log("task Faulted Upload Failed PANIC");
		}
		else if (uploadtask.IsCanceled)
		{
			//		UIScreenManager.instance.TransitionFromscreen("Loading");

			Debug.Log("task cancelled Upload Failed PANIC");
		}
	}




	IEnumerator uploadFileNow(string path, Firebase.Storage.StorageReference uploadPath)
	{

		string local_file = path;

		// Upload the file to the path "images/rivers.jpg"
		System.Threading.Tasks.Task<Firebase.Storage.StorageMetadata> uploadtask = uploadPath.PutFileAsync(local_file);
		yield return new YieldTask(uploadtask);
		if (uploadtask.IsCompleted)
		{
		

			Debug.Log("File UPLOADED SUCESSFULLY");

			System.Threading.Tasks.Task<System.Uri> geturl = uploadPath.GetDownloadUrlAsync();
			yield return new YieldTask(geturl);
			string DOWNLOADURL = "";
			if (!geturl.IsFaulted && !geturl.IsCanceled)
			{
				DOWNLOADURL = geturl.Result.ToString();
				Debug.Log("Download URL: " + geturl.Result.ToString());

			//	ScreenCapture.instance.ShareUrl = DOWNLOADURL;
			}
			else
			{
				DOWNLOADURL = auth.CurrentUser.PhotoUrl.ToString();
				Debug.Log("Download URL NOT FOUND");
			}
			Firebase.Storage.StorageMetadata metadata = uploadtask.Result;
			Debug.Log("Finished uploading...");

			//					Debug.Log("download url = " + download_url);
			DatabaseReference reference;
			reference = FirebaseDatabase.DefaultInstance.RootReference;
			FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://youhanpandatest.firebaseio.com/");

			
			reference.Child("Users").Child(user.UserId).Child("RecordedVideo").SetValueAsync(DOWNLOADURL);
			//	Profilemanagement.instance.UserImageurl = DOWNLOADURL;
		}
		else if (uploadtask.IsFaulted)
		{
		//	UIScreenManager.instance.TransitionFromscreen("Loading");

			Debug.Log("task Faulted Upload Failed PANIC");
		}
		else if (uploadtask.IsCanceled)
		{
	//		UIScreenManager.instance.TransitionFromscreen("Loading");

			Debug.Log("task cancelled Upload Failed PANIC");
		}
	}
}
