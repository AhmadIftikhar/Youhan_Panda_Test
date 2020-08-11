using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;
using UnityEngine.UI;
public class FileName : MonoBehaviour
{
	public string[] FileNames;
	public int index=-1;
	public Button Procceed;

	public FetchSesorsofscenario fsos;
	public void SetFileindex1(bool val)
	{
		if (val)
		{
			index = 0;
			Debug.Log("Value changed" + val);
		}
		else
		{
			index = -1;
		}
	}
	public void SetFileindex2(bool val)
	{
		if (val)
		{
			index = 1;
			Debug.Log("Value changed" + val);
		}
		else
		{
			index = -1;
		}
	}
	public void SetFileindex3(bool val)
	{
		if (val)
		{
			index = 2;
			Debug.Log("Value changed" + val);
		}
		else
		{
			index = -1;
		}
	}
	public void SetFileindex4(bool val)
	{
		if (val)
		{
			index = 3;
			Debug.Log("Value changed" + val);
		}
		else
		{
			index = -1;
		}
	}
	public void SetFileindex5(bool val)
	{
		if (val)
		{
			index = 4;
			Debug.Log("Value changed" + val);
		}
		else
		{
			index = -1;
		}
	}
	public void SetFileindex6(bool val)
	{
		if (val)
		{
			index = 5;
			Debug.Log("Value changed" + val);
		}
		else
		{
			index = -1;
		}
	}
	public void SetFileindex7(bool val)
	{
		if (val)
		{
			index = 6;
			Debug.Log("Value changed" + val);
		}
		else
		{
			index = -1;
		}
	}
	public void SetFileindex8(bool val)
	{
		if (val)
		{
			index = 7;
			Debug.Log("Value changed" + val);
		}
		else 
		{
			index = -1;
		}
	}
	public void SetFileindex9(bool val)
	{
		if (val)
		{
			index = 8;
			Debug.Log("Value changed" + val);
		}
		else
		{
			index = -1;
		}
	}
	private void Update()
	{
		if (index == -1) 
		{
			Procceed.interactable = false;
		}
		if (index != -1)
		{
			Procceed.interactable = true;
		}
	}

	public void ProcceedWIthName()
	{
		if (index != -1)
		{
			fsos.Scenariosensor = FileNames[index];
			fsos.CurrentscenarioPath = fsos.GeneralscenarioPath + fsos.Scenariosensor;
			fsos.EnableReadFileInfo();
		}
	}
}
