using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.UI;

public class ProjectorSystem : MonoBehaviour
{


    public GameObject DecalProjectorObject;
    public Texture DecalImage;



    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 direction = transform.TransformDirection(this.transform.forward) * 500;
        Gizmos.DrawRay(this.transform.position, direction);
    }

    void FixedUpdate()
    {

        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            Debug.Log("Did Hit");

            float distance = Vector3.Distance(this.transform.position, hit.point);
            Debug.Log("Distance to hit is "+ distance);


            DecalProjectorObject.transform.GetComponent<DecalProjector>().size = new Vector3(1 * distance, 1 * distance, 1*distance );

            DecalProjectorObject.transform.localEulerAngles = new Vector3(0,0,0);
            Material mat = new Material(DecalProjectorObject.GetComponent<DecalProjector>().material);
            mat.mainTexture= DecalImage;
            DecalProjectorObject.GetComponent<DecalProjector>().material = mat;


            DecalProjectorObject.transform.localScale=(Vector3.one * Vector3.Distance(this.transform.position, hit.point));
            DecalProjectorObject.transform.position = hit.point;
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
            Debug.Log("Did not Hit");
        }
    }
}
