using UnityEngine;
using System.Collections;

public class CameraHandler : MonoBehaviour {

	Camera worldCam;
	float speed = 500;
	float zoomSpeed = 4000;
	float minZoom = 100;
	float maxZoom = 500;

	// Use this for initialization
	void Start () {
		worldCam = GetComponent<Camera>();
		worldCam.orthographicSize = Screen.height / 2f;
	}

	// Update is called once per frame
	void Update () {
		float zoomChange = Input.GetAxis("Mouse ScrollWheel") != 0 ? Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * zoomSpeed : 0;

		transform.Translate(Input.GetAxis("Horizontal") * Time.deltaTime * speed, Input.GetAxis("Vertical") * Time.deltaTime * speed, 0);

		worldCam.orthographicSize = Mathf.Clamp(worldCam.orthographicSize + zoomChange, minZoom, maxZoom);

		if (Input.GetAxis("ResetCamera") > 0) {
			worldCam.orthographicSize = Screen.height / 2f;
		}
	}
}
