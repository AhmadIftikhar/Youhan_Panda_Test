using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopulateDictionaryPrefab : MonoBehaviour
{

    public Text Key;
    public Text value;

    public void UpdateMykeyValue(string k,string v) 
    {
        Key.text = k;
        value.text = v;
    }
}
