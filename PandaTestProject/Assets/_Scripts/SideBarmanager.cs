﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SideBarmanager : MonoBehaviour
{
	public static SideBarmanager instance;
	public Animator anim;

	void Start()
	{
		instance = this;
	}

	public void Changeanimatorstate()
	{
		anim.SetTrigger("animate");
	}
	public void ClearallplayerPrefs()
	{
		PlayerPrefs.DeleteAll();
		Application.Quit();
	}
	
}
