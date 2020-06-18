namespace Mapbox.Examples
{
	using Mapbox.Unity.Location;
	using UnityEngine;

	public class InitOrientationWithLocationProvider : MonoBehaviour
	{
        ILocationProvider _locationProvider;

        private void Start()
        {
            _locationProvider = LocationProviderFactory.Instance.DefaultLocationProvider;
            _locationProvider.OnLocationUpdated += LocationProvider_OnLocationUpdated;
        }

        void LocationProvider_OnLocationUpdated(Unity.Location.Location location)
        {
            _locationProvider.OnLocationUpdated -= LocationProvider_OnLocationUpdated;
            transform.localRotation = Quaternion.Euler(getNewEulerAngles(location.DeviceOrientation));
		}

		private Vector3 getNewEulerAngles(float newAngle)
		{
			var currentEuler = transform.localRotation.eulerAngles;
			var euler = Mapbox.Unity.Constants.Math.Vector3Zero;

			euler.y = -newAngle;
			euler.x = currentEuler.x;
			euler.z = currentEuler.z;
			return euler;
		}
	}
}
