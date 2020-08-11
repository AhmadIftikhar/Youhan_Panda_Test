using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FetchSesorsofscenario : MonoBehaviour
{
    public Dropdown dd;

    public string GeneralscenarioPath;
    public string CurrentscenarioPath;
    public ReadFileSpawnOnMap rfsom;
    public string Scenariosensor = "";
   

    public enum Scenarios 
    { 
        Fire,Flood,Shooter,Transit   
    }

    public GameObject Firepannel;
    public GameObject Floodpannel;
    public GameObject Shooterpannel;
    public GameObject Transitpannel;

    public void Selectchange() 
    {
        switch (dd.value)
        {
            case (int) Scenarios.Fire :
                Firepannel.SetActive(true);
                Floodpannel.SetActive(false);
                Shooterpannel.SetActive(false);
                Transitpannel.SetActive(false);
                break;
            case (int)Scenarios.Flood:
                Firepannel.SetActive(false);
                Floodpannel.SetActive(true);
                Shooterpannel.SetActive(false);
                Transitpannel.SetActive(false);

                break;
            case (int)Scenarios.Shooter:
                Firepannel.SetActive(false);
                Floodpannel.SetActive(false);
                Shooterpannel.SetActive(true);
                Transitpannel.SetActive(false);

                break;
            case (int)Scenarios.Transit:
                Firepannel.SetActive(false);
                Floodpannel.SetActive(false);
                Shooterpannel.SetActive(false);
                Transitpannel.SetActive(true);
                 break;
        }
    
    }

    public void EnableReadFileInfo() 
    {
        rfsom.SimulateDataFilePath =CurrentscenarioPath;
        rfsom.gameObject.SetActive(true);


    }


}
