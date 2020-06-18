namespace Mapbox.Unity.Location
{
	using System;
	using UnityEngine;

	public abstract class AbstractLocationProvider : MonoBehaviour, ILocationProvider
	{
		protected Location _currentLocation;

		/// <summary>
		/// Gets the last known location.
		/// </summary>
		/// <value>The current location.</value>
		public Location CurrentLocation
		{
			get
			{
				return _currentLocation;
			}
		}

		public event Action<Location> OnLocationUpdated = delegate { };

		protected virtual void SendLocation(Location location)
		{
            // YB: added

            //Debug.LogError("DeviceOrientation before:" + location.DeviceOrientation);
            location.DeviceOrientation += azBias;
            //Debug.LogError("DeviceOrientation after:" + location.DeviceOrientation);

            OnLocationUpdated(location);
		}
        
        public void SetAzBias ( float a)
        {
            azBias = a;
        }


        //    // YB: Hereunder all added

        Camera mainCamera;

        //private void Update()
        //{
        //    if (mainCamera == null)
        //    {
        //        mainCamera = Camera.main;
        //        if (mainCamera == null) return;
        //    }
        //    if (mainCamera.transform.forward.y > -0.75f)
        //        UpdateGesture();
        //}

        bool gestureStarted = false;
        Vector3 startTouchPosition;
        Vector3 startBias;
        float azBias = 0;
        float tiltBias = 0;

        // Update is called once per frame
        void UpdateGesture()
        {

            //		if (Input.touchCount > 0 )
            //		{
            //			//Debug.LogError("TouchCount:" + Input.touchCount );
            //			Debug.LogError("TouchPhase:" + Input.GetTouch(0).phase + " delta:" + Input.GetTouch(0).deltaPosition );
            //			if ( Input.GetTouch(0).phase == TouchPhase.Moved ) 
            //			{
            //
            //			}
            //		}

            if (Input.GetMouseButtonUp(0))
            {
                gestureStarted = false;
            }
            else if (Input.GetMouseButtonDown(0))
            {
                startTouchPosition = Input.mousePosition;
                startBias = new Vector3(azBias, tiltBias, 0);
                gestureStarted = true;
            }
            if (gestureStarted)
            {
                Vector3 gestureVector = startTouchPosition - Input.mousePosition;
                float fov = Camera.main.GetComponent<Camera>().fieldOfView;
                gestureVector.x *= fov / Screen.width;
                gestureVector.y *= fov / Screen.height;
                azBias = startBias.x + gestureVector.x;
                tiltBias = startBias.y + gestureVector.y;

                Debug.LogError("AzBias:" + azBias);
            }
        }
    }
}