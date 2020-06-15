namespace Mapbox.Examples
{
	using Mapbox.Unity.Map;
	using Mapbox.Unity.Utilities;
	using Mapbox.Utils;
	using UnityEngine;
	using UnityEngine.EventSystems;
	using System;
	using Mapbox.Unity.MeshGeneration.Data;
#if UNITY_IOS && MSSHAREDANCHORS
	using UnityEngine.XR.iOS;
#endif

    public class QuadTreeCameraMovementNew : MonoBehaviour
	{
		[SerializeField]
		[Range(1, 20000)]
		public float _panSpeed = 1.0f;

		[SerializeField]
		float _zoomSpeed = 0.25f;

		[SerializeField]
		public Camera referenceCamera;

        [SerializeField]
        AbstractMap miniMap;

        [SerializeField]
        bool onlyWhenLookingDown = true;

		//[SerializeField]
		//bool useNewMethod = true; 

	//public	AbstractMap overlayMap;
		bool use = true;
        Vector3 startMouseWorldPosition;
		Vector3 mouseWorldPosition;
		Vector3 previousMouseWorldPosition;
		bool dragging = false;
		bool initialized = false;
		Plane _groundPlane = new Plane(Vector3.up, 0);
		bool dragStartedOnUI = false;
		Matrix4x4 worldPose = new Matrix4x4();
		Matrix4x4 cameraPose = new Matrix4x4();
		Matrix4x4 parentPose = new Matrix4x4();
		Camera mainCamera;
		float factor;
		Vector2d latlongDelta;
		Vector2d startDragLatitudeLongitude;
		Vector2d targetLatitudeLongitude;
		Vector3 mouseScreenPosition;
		Vector3 newWorldTargetPosition;
		Vector3 offset;
		Vector3 previousWorldTargetPosition;
		Vector3 previousAbsoluteMouseWorldPositionOffset;
		Vector3 previousMouseScreenPosition = Vector3.zero;
		bool debugDrag = false;
	//	ContentManager geolocatedContentManager; 

		void Awake()
		{
            miniMap.OnInitialized += () =>
			{
				initialized = true;
			};
		}

        private void Start()
        {
			mouseScreenPosition = new Vector3(Screen.width / 2, Screen.height / 2);
			ContinueWhenARManagerInited(); 
		}

        void ContinueWhenARManagerInited ()
		{
		//	overlayMap = ARManager.instance.GetOverlayMap();
			mainCamera = Camera.main;
		//	ARManager.instance.ARManagerInitialized -= ContinueWhenARManagerInited;
	//		geolocatedContentManager = ARManager.instance.GetGeoLocatedContentLoader().GetComponent<ContentManager>(); 
        }


		public void Update()
		{
		//	if (overlayMap == null) return;
			if (mainCamera == null) return; 

			if (null == referenceCamera)
			{
				referenceCamera = mainCamera;
				if (null == referenceCamera)
				{
					Debug.LogErrorFormat("{0}: reference camera not set", this.GetType().Name);
					return;
				}
			}

			// if we only processing inputs when looking down and not looking down then return
            // OR also return if the prevent pan and zoom flag has been set (set onpointerenter and onpointerexit of dropdown)

			//			if (onlyWhenLookingDown  ARUIManager.Instance.GetPreventPanAndZoom() || geolocatedContentManager.IsRTHandleSelected()) return;
			
			//          if (Input.GetMouseButtonDown(0) && EventSystem.current.IsPointerOverGameObject())
			//	{
			//		_dragStartedOnUI = true;
			//	}

			//	if (Input.GetMouseButtonUp(0))
			//	{
			//		_dragStartedOnUI = false;
			//	}
			//}


			//private void LateUpdate()
			//{
			//	if (!_isInitialized) { return; }

			//          if (mainCamera == null || (onlyWhenLookingDown &&  mainCamera.transform.forward.y > -0.75f)) return;

			//	if (!_dragStartedOnUI)

			//{
			if (Input.touchSupported && Input.touchCount > 0)
			{
				HandleTouch();
			}
			else
			{
				HandleMouseAndKeyBoard();
			}
			//}
		}
		void HandleMouseAndKeyBoard()
		{
			// zoom
			float scrollDelta = 0.0f;
			scrollDelta = Input.GetAxis("Mouse ScrollWheel");
			if ( scrollDelta != 0 ) ZoomMapUsingTouchOrMouse(scrollDelta);
            
            //pan keyboard
            float xMove = Input.GetAxis("Horizontal");
            float zMove = Input.GetAxis("Vertical");

            if ((Math.Abs(xMove) > 0.0f || Math.Abs(zMove) > 0.0f))
            {
                Vector2 v = new Vector2(xMove, zMove);
                v = Quaternion.Euler(0, -miniMap.gameObject.transform.eulerAngles.y, 0) * v;
                xMove = v.x;
                zMove = v.y;

                PanMapUsingKeyBoard(xMove, zMove);
            }

			//pan mouse
			PanMapUsingTouchOrMouse();
		}

		void HandleTouch()
		{
			float zoomFactor = 0.0f;
			//pinch to zoom.
			switch (Input.touchCount)
			{
				case 1:
					{
						PanMapUsingTouchOrMouse();
					}
					break;
				case 2:
					{
						// Store both touches.
						Touch touchZero = Input.GetTouch(0);
						Touch touchOne = Input.GetTouch(1);

						// Find the position in the previous frame of each touch.
						Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
						Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

						// Find the magnitude of the vector (the distance) between the touches in each frame.
						float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
						float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

						// Find the difference in the distances between each frame.
						zoomFactor = 0.01f * (touchDeltaMag - prevTouchDeltaMag);
					}
					ZoomMapUsingTouchOrMouse(zoomFactor);
					break;
				default:
					break;
			}
		}

		void ZoomMapUsingTouchOrMouse(float zoomFactor)
		{
            var zoom = Mathf.Max(0.0f, Mathf.Min(miniMap.Zoom + zoomFactor * _zoomSpeed, 21.0f));
			if (Math.Abs(zoom - miniMap.Zoom) > 0.0f)
			{
                if ( miniMap != null && isTouchingContentMap())
                    miniMap.UpdateMap(miniMap.CenterLatitudeLongitude, zoom);
            }
        }

		void PanMapUsingKeyBoard(float xMove, float zMove)
		{

            if ( !isTouchingContentMap()) return;

            // Get the number of degrees in a tile at the current zoom level.
            // Divide it by the tile width in pixels ( 256 in our case)
            // to get degrees represented by each pixel.
            // Keyboard offset is in pixels, therefore multiply the factor with the offset to move the center.
            float factor = _panSpeed * (Conversions.GetTileScaleInDegrees((float)miniMap.CenterLatitudeLongitude.x, miniMap.AbsoluteZoom));
			//MapLocationOptions locationOptions = new MapLocationOptions
			//{
			var latitudeLongitude = new Vector2d(miniMap.CenterLatitudeLongitude.x + zMove * factor * 2.0f, miniMap.CenterLatitudeLongitude.y + xMove * factor * 4.0f);
            //};

            miniMap.UpdateMap(latitudeLongitude, miniMap.Zoom);
      //      if (overlayMap != null) overlayMap.UpdateMap(latitudeLongitude, overlayMap.Zoom);
			
		}

        bool isTouchingContentMap()
        {
		//	var handleController = transform.root.gameObject.GetComponentInChildren<ContentManager>();
            if (onlyWhenLookingDown) return true;

            Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (!Physics.Raycast(r, out hit)) return false;

            //AbstractMap map = hit.collider.gameObject.GetComponent<AbstractMap>();

            //Debug.LogWarning("QuadTree Raycast Hit " + hit.collider.name);
		
            if (hit.collider.gameObject.GetComponentInParent<AbstractMap>() != miniMap) return false;
            UnityTile tileHit = hit.collider?.GetComponent<UnityTile>();
            if (tileHit == null) return false;
		//	if (handleController != null) if (handleController.IsRTHandleSelected()) return false; 
            //if (hit.collider.gameObject.transform.parent.gameObject != _miniMap.gameObject) return false; 
            return true;

        }

        void PanMapUsingTouchOrMouse()
		{
            // Disable dragging of the content map (onlyWhenLoookingDown set only on look down map)
			if (!onlyWhenLookingDown) return;

            if (Input.GetMouseButton(0)) // fire everytime the mouse button is down
            {
                if (true )//!EventSystem.current.IsPointerOverGameObject())
				{
                    mouseScreenPosition = Input.mousePosition;
                    // simulate mouse move
                    //mouseScreenPosition += new Vector3(5f, 5f, 0f);

                    if (debugDrag ) Debug.LogError("\nMouseScreenPositionChange:" + (mouseScreenPosition - previousMouseScreenPosition).ToString("F3"));
					previousMouseScreenPosition = mouseScreenPosition;

					//assign distance of camera to ground plane to z, otherwise ScreenToWorldPoint() will always return the position of the camera
					//http://answers.unity3d.com/answers/599100/view.html

					mouseScreenPosition.z = referenceCamera.transform.localPosition.y;
                    mouseWorldPosition = referenceCamera.ScreenToWorldPoint(mouseScreenPosition);

					if (dragging == false)
                    {
                        // if onlyWhenLookingDown not set (for content map) and not touching the content map then return
                        // this is different of first test of the function, it allow the dragging of content map but
                        // only if that map is touched
                        if (!isTouchingContentMap() && !onlyWhenLookingDown) return;

                        dragging = true;
                        startMouseWorldPosition = mouseWorldPosition;
						startDragLatitudeLongitude = miniMap.CenterLatitudeLongitude;
					}
                }
            }
			else dragging = false;

            // if (Input.GetKeyDown(KeyCode.D))
            // {
			//	    dragging = true;
			//  	mouseScreenPosition += new Vector3(10f, 10f, 0f);
			//  	mouseScreenPosition.z = referenceCamera.transform.localPosition.y;
			//  	startMouseWorldPosition =  mouseWorldPosition = referenceCamera.ScreenToWorldPoint(mouseScreenPosition);
			//  }
			//else if ( Input.GetKeyUp (KeyCode.D)) dragging = false;


			if (dragging)
			{
				Vector3 mouseWorldPositionChange = previousMouseWorldPosition - mouseWorldPosition;

				if (debugDrag) Debug.LogError("MouseWorldPositionChange:" + mouseWorldPositionChange.ToString("F3"));

                if (mouseWorldPositionChange.sqrMagnitude > 0)
				{
					//Debug.LogError("Mouse world position changed > 0.075f  - " + worldPositionChange);

					previousMouseWorldPosition = mouseWorldPosition;

                    Vector3 absoluteMouseWorldPositionOffset = startMouseWorldPosition - mouseWorldPosition;

					if (use)
                        absoluteMouseWorldPositionOffset = Quaternion.Euler(0, -miniMap.gameObject.transform.eulerAngles.y, 0) * absoluteMouseWorldPositionOffset;

					//absoluteMouseWorldPositionOffset += new Vector3(0.2f, 0.000f, 0f);

					if (debugDrag) Debug.LogError(string.Format("absoluteMouseWorldPositionOffset:{0:F6} {1:F6} {2:F6}", absoluteMouseWorldPositionOffset.x, absoluteMouseWorldPositionOffset.y, absoluteMouseWorldPositionOffset.z));
					// Debug.LogError("Mousepose: " + mouseScreenPosition.ToString("F3") + "offset: " + absoluteMouseWorldPositionOffset.ToString("F3"));
					if (debugDrag) Debug.LogError("MouseAbsoluteOffsetChange:" + (absoluteMouseWorldPositionOffset - previousAbsoluteMouseWorldPositionOffset).ToString("F6"));

					previousAbsoluteMouseWorldPositionOffset = absoluteMouseWorldPositionOffset;

					if (absoluteMouseWorldPositionOffset.sqrMagnitude > 0 && miniMap != null )
					{
                        factor = _panSpeed * Conversions.GetTileScaleInMeters((float)0, miniMap.AbsoluteZoom) / miniMap.UnityTileSize;
						latlongDelta = Conversions.MetersToLatLon(new Vector2d(absoluteMouseWorldPositionOffset.x * factor, absoluteMouseWorldPositionOffset.z * factor));

						if ( false )// useNewMethod)
                        {
                            //targetLatitudeLongitude = startDragLatitudeLongitude + latlongDelta;
                            targetLatitudeLongitude = miniMap.CenterLatitudeLongitude + latlongDelta;

                            //newWorldTargetPosition = overlayMap.GeoToWorldPosition(targetLatitudeLongitude, true);
                            newWorldTargetPosition = miniMap.GeoToWorldPosition(targetLatitudeLongitude, true);

							//newWorldTargetPosition = startMouseWorldPosition + absoluteMouseWorldPositionOffset * factor;
							newWorldTargetPosition.y = mainCamera.transform.position.y;

#if UNITY_IOS && MSSHAREDANCHORS && !(UNITY_EDITOR || UNITY_STANDALONE )
                            //ARKit requires you to use their function for setting the AR Session origin -- wont work in editor
                            GameObject tempPlaneObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
                            tempPlaneObject.transform.position = newWorldTargetPosition;
                            UnityARSessionNativeInterface.GetARSessionNativeInterface().SetWorldOrigin(tempPlaneObject.transform);
                            Destroy(tempPlaneObject);
#else
							worldPose.SetTRS(newWorldTargetPosition, mainCamera.transform.rotation, Vector3.one);
                            cameraPose.SetTRS(mainCamera.transform.localPosition, mainCamera.transform.localRotation, Vector3.one);
                            parentPose = worldPose * cameraPose.inverse;
                            mainCamera.transform.parent.localRotation = Quaternion.LookRotation(parentPose.GetColumn(2), parentPose.GetColumn(1));
                            mainCamera.transform.parent.localPosition = parentPose.GetColumn(3);
#endif

							//Debug.LogError("parent pose: " + mainCamera.transform.parent.localPosition );

							if (debugDrag) Debug.LogError("NewWorldPositionChange: " + (newWorldTargetPosition - previousWorldTargetPosition).ToString("F6"));
							previousWorldTargetPosition = newWorldTargetPosition;
                        }
                        else
                        {
							targetLatitudeLongitude = miniMap.CenterLatitudeLongitude + latlongDelta;
                            miniMap.UpdateMap(targetLatitudeLongitude, miniMap.Zoom);
							//MapModelManager.Instance.UpdateModelsFromMapUpdate();
					//		if (overlayMap != null) overlayMap.UpdateMap(targetLatitudeLongitude, overlayMap.Zoom);

                            //updating model positions based on their geolocation (as we have moved the map) DONE AFTER BOTH MAPS ARE UPDATED
						//	MapManager.Instance.UpdateAllModelsFromMapUpdate();

						}
                    }
                    startMouseWorldPosition = mouseWorldPosition;
				}
				else
				{
                    // YB? Explain effect of this test
					if (EventSystem.current.IsPointerOverGameObject()) return;

                    // YB? Is this really needed?
					previousMouseWorldPosition = mouseWorldPosition;
					startMouseWorldPosition = mouseWorldPosition;
				}
			}
		}

		Vector3 getGroundPlaneHitPoint(Ray ray)
		{
			float distance;
			if (!_groundPlane.Raycast(ray, out distance)) { return Vector3.zero; }
			return ray.GetPoint(distance);
		}
	}
}