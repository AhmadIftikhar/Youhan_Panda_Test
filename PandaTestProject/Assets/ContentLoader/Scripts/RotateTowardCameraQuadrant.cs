using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mapbox.Unity.Map;

public class RotateTowardCameraQuadrant : MonoBehaviour
{
    float angle;
    float result;
    AbstractMap map;

    // Start is called before the first frame update
    void Start()
    {
        map = GetComponentInParent<AbstractMap>();
    }

    // Update is called once per frame

    /// <summary>
    /// helpers to debug what angle we see
    /// </summary>
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 direction = transform.TransformDirection(map.transform.forward) * 50;
        Gizmos.DrawRay(map.transform.position, direction);



        Gizmos.color = Color.yellow;
        Vector3 direction1 = Camera.main.transform.position;
        Gizmos.DrawRay(transform.position, direction1);

    }
  
        void Update()
    {

        if (Camera.main == null) return;
        angle = Vector3.Angle(map.transform.forward, Camera.main.transform.position );
        if (angle < 45)
            result = 180;
        else if (angle > 45 && angle < 135)
        {
          float  angle2 = Vector3.Angle(map.transform.right, Camera.main.transform.position);

            if (angle2<90 )
              result = 270;
            else
                result = 90;
        }
        else if (angle > 135 && angle < 225)
            result = 0;
 



        Debug.LogError("Angle:" + angle + " result:" + result );
        transform.localEulerAngles = new Vector3(0, result, 0);
    }
}
