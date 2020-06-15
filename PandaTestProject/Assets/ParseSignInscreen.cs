using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using Parse;
using UnityEngine.Events;
using UnityEngine.SceneManagement;


public class ParseSignInscreen : MonoBehaviour
{
	[Header("SignUp  Settings")]
	[SerializeField]
	InputField SignUp_username;
	[SerializeField]
	InputField SignUp_password;
	[SerializeField]
	InputField SignUp_email;



	[Header("Loading  Settings")]

	public UnityEvent OnLoadingstart;
	public UnityEvent OnLoadingDone;

	[Header("SignIn  Settings")]
	[SerializeField]
	InputField SignIn_username;
	[SerializeField]
	InputField SignIn_password;
	public UnityEvent OnLogInSuccess;



	[Header("Feedback  Settings")]
	[SerializeField]
	Text feedbackMessage;
	public UnityEvent OnSignInSuccess;

	[Header("Forgotpassword  Settings")]
	[SerializeField]
	InputField Forgot_Email;
	public UnityEvent OnForgotsucess;


	void Start()
	{
		OnLoadingstart.Invoke();
		string username = PlayerPrefs.GetString("username","");
		string password = PlayerPrefs.GetString("password","");

		if (username != "" && password != "")
		{
			SignIn_username.text = username;
			SignIn_password.text = password;
			SignIn();
		}
		OnLoadingDone.Invoke();
	}


	public async void SignUp()
	{
		OnLoadingstart.Invoke(); //Loading started

		var user = new ParseUser()
		{
			Username = SignUp_username.text.Normalize(),
			Password = SignUp_password.text.Normalize(),
			Email = SignUp_email.text.Normalize()
		};	
		
		await user.SignUpAsync().ContinueWith(t =>
	  {
		  if (t.IsFaulted || t.IsCanceled)
		  {
			  // The login failed. Check the error to see why.
			  UnityMainThread.wkr.AddJob(() =>
			  {
				  // Will run on main thread, hence issue is solved
				  OnLoadingDone.Invoke();
				  Debug.Log("SignUpfailed "+ t.Exception.Message); //SignUp Complete

				  feedbackMessage.text = t.Exception.Message;
				  OnSignInSuccess.Invoke(); //SignUp Complete
			  });
		  }
		  else
		  {
			  //SignUp was  was successful.
			  UnityMainThread.wkr.AddJob(() =>
			  {
				  // Will run on main thread, hence issue is solved
				  feedbackMessage.text = "Sign Up Complete verify Email to Log In to App";
				  OnLoadingDone.Invoke();
				  OnSignInSuccess.Invoke(); //SignUp Complete
			  });
			
		  }
	  });

	}



	public  async void SignIn()
	{
		OnLoadingstart.Invoke(); //Loading started
		string Username = SignIn_username.text.Normalize();
		string Password = SignIn_password.text.Normalize();

		

		Debug.Log("UName"+Username+"UPassword"+Password);
		await ParseUser.LogInAsync(Username, Password).ContinueWith(t =>
		{
			if (t.IsFaulted || t.IsCanceled)
			{
				// The login failed. Check the error to see why.
				UnityMainThread.wkr.AddJob(() =>
				{
					// Will run on main thread, hence issue is solved
					OnLoadingDone.Invoke();
				});
					Debug.Log("OMG I Failed to LogIN ");
			}
			else
			{
				// Login was successful.
				Debug.Log("OMG I LOOGED IN");

			
				UnityMainThread.wkr.AddJob(() =>
				{
				
					PlayerPrefs.SetString("username", Username);
					PlayerPrefs.SetString("password", Password);

					OnLoadingDone.Invoke();
					OnLogInSuccess.Invoke(); //SignIn Complete
				});

			}
		});
	}



	public async void Forgotpassword()
	{
		Task requestPasswordTask = ParseUser.RequestPasswordResetAsync(Forgot_Email.text.Normalize()).ContinueWith(t =>
		{

			if (t.IsFaulted || t.IsCanceled)
		
				{
				feedbackMessage.text = "Reset Message Sent To Email";
				OnForgotsucess.Invoke(); //Clear forgot Screen
			}

			if (t.IsCompleted)
				{
				feedbackMessage.text = "Reset Message Sent To Email";
				OnSignInSuccess.Invoke(); 
				OnForgotsucess.Invoke(); //Clear forgot Screen
			}

		});


	}


	// Load Next level shen signIn done
	public void LoadNextscene()
	{
		SceneManager.LoadScene(1);

	}
}
