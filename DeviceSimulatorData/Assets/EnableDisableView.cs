using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EnableDisableView : MonoBehaviour
{
	public GameObject view;

	public void OnMouseDown()
	{
	 view.gameObject.SetActive(!view.activeInHierarchy); 	
	}


}
