namespace Mapbox.Unity.Map
{
	using System.Collections;
	using Mapbox.Unity.Location;
	using UnityEngine;

	public class InitializeMapWithLocationProvider : MonoBehaviour
	{
		[SerializeField]
		AbstractMap _map;

//        Mapbox.Utils.Vector2d vect = new Mapbox.Utils.Vector2d(34.06674, -118.14745);

		ILocationProvider _locationProvider;
    
		private void Awake()
		{
			// Prevent double initialization of the map. 
			_map.InitializeOnStart = false;
		}

		//protected virtual IEnumerator
		protected virtual IEnumerator Start()
		{
			yield return null;
            Initiate();
        }

        public void Initiate()
        {
            _locationProvider = LocationProviderFactory.Instance.DefaultLocationProvider;
            _locationProvider.OnLocationUpdated += LocationProvider_OnLocationUpdated;
        }


        void LocationProvider_OnLocationUpdated(Unity.Location.Location location)
		{
			_locationProvider.OnLocationUpdated -= LocationProvider_OnLocationUpdated;
            _map.Initialize(location.LatitudeLongitude, _map.AbsoluteZoom);
            //_map.SetCenterLatitudeLongitude(location.LatitudeLongitude);
            //_map.SetZoom(18.34f);
            _map.UpdateMap(18.34f);
        }

        //void Update()
        //{
        //    //Debug.LogError("GeoLocation: " + vect + " - worldpos:" + _map.GeoToWorldPosition(vect));
        //}
	}
}
