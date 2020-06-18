namespace Mapbox.Examples
{
	using UnityEngine;

	public class CameraBillboard : MonoBehaviour
	{
		public Camera _camera;

		public void Start()
		{
			_camera = Camera.main;
		}

		void Update()
		{
            _camera = Camera.main;
            if (_camera == null) return;
            transform.LookAt(transform.position + _camera.transform.rotation * Vector3.forward, _camera.transform.rotation * Vector3.up);
            transform.eulerAngles = new Vector3(90, transform.eulerAngles.y, 0);
		}
	}
}