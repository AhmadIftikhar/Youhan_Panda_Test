using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mapbox.Unity.Map;
using Mapbox.Map;
using Mapbox.Unity.MeshGeneration.Factories;
using Mapbox.Unity.Map.Interfaces;
using System;

public class ContentMapCanvasScale : MonoBehaviour
{
    GameObject mapParent;
    GameObject canvas;
    AbstractMap miniMap;
    int previousZoom; 

    private void Start()
    {
        mapParent = transform.parent.gameObject;
        canvas = GetComponentInChildren<Canvas>(true).gameObject;
        
        if (mapParent != null) miniMap = mapParent.GetComponent<AbstractMap>();
        if (miniMap != null) miniMap.OnUpdated += ScaleContentMapCanvas;
        if (miniMap != null) miniMap.OnInitialized += ScaleContentMapCanvas;

        transform.localScale *= 1.2f;

        previousZoom = (int)miniMap.Zoom;
    }

    private void ScaleContentMapCanvas()
    {

        MeshRenderer[] tiles = mapParent.transform.GetComponentsInChildren<MeshRenderer>();
        Bounds bounds = tiles[0].bounds;

        foreach (MeshRenderer renderer in tiles)
        {
            bounds.Encapsulate(renderer.bounds);
        }

        Vector3 pos = bounds.center;
        pos.y = 0;
        transform.localPosition = pos;

        int currentZoom = Mathf.FloorToInt(miniMap.Zoom);
        //this.transform.position = bounds.center;
        if (currentZoom != previousZoom) 
        {
            
            transform.localScale *= (float)Math.Pow(2, (previousZoom - currentZoom));
            previousZoom = currentZoom; 
        }
    }

    private void OnDestroy()
    {
        miniMap.OnUpdated -= ScaleContentMapCanvas;
    }

    


}
