namespace Mapbox.Unity.Location
{
	using System;
	using Mapbox.Unity.Utilities;
	using Mapbox.Utils;
	using UnityEngine;

// 34.109333, -118.346583 Holywood hills
// 33.981240, -118.453338 Marina Del Rey


    /// <summary>
    /// The EditorLocationProvider is responsible for providing mock location and heading data
    /// for testing purposes in the Unity editor.
    /// </summary>
    public class LocationArrayEditorLocationProvider : AbstractEditorLocationProvider
	{
		/// <summary>
		/// The mock "latitude, longitude" location, respresented with a string.
		/// You can search for a place using the embedded "Search" button in the inspector.
		/// This value can be changed at runtime in the inspector.
		/// </summary>
		[SerializeField]
		[Geocode]
		string[] _latitudeLongitude;

        /// <summary>
        /// The mock heading value.
        /// </summary>
        [SerializeField]
        [Range(0, 359)]
        float _heading;

        /// <summary>
        /// The mock device orientation value.
        /// </summary>
        [SerializeField]
        [Range(0, 359)]
        float _deviceOri;


        private int idx = -1;
		Vector2d LatitudeLongitude
		{
			get
			{
				idx++;
				// reset index to keep looping through the location array
				if (idx >= _latitudeLongitude.Length) { idx = 0; }
				return Conversions.StringToLatLon(_latitudeLongitude[idx]);
			}
		}

		protected override void SetLocation()
		{
            // YB: on editor set the heading to be the orientation of device as well
            _currentLocation.DeviceOrientation = _deviceOri;

            _currentLocation.UserHeading = _heading;
			_currentLocation.LatitudeLongitude = LatitudeLongitude;
			_currentLocation.Accuracy = _accuracy;
			_currentLocation.Timestamp = UnixTimestampUtils.To(DateTime.UtcNow);
			_currentLocation.IsLocationUpdated = true;
			_currentLocation.IsUserHeadingUpdated = true;
		}
	}
}
