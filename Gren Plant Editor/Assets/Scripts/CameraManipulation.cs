using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManipulation : MonoBehaviour {

	public float maxRot;
	public float minRot;
	public float lookSensitivity;

	private Vector2 camRotation;
	private Vector2 actualCamRotation;


	public float maxZoom;
	public float minZoom;
	public float zoomSpeed;

	private float actualZValueOfCam;

	[Range(0.1f, 1f)]
	public float smoothCoef;
	


	void Start () 
	{
		actualZValueOfCam = Camera.main.transform.position.z;
	}

	void Update () 
	{
		
		if (Input.GetMouseButtonDown(1))
			Cursor.visible = false;
		else if (Input.GetMouseButtonUp(1))
			Cursor.visible = true;
		
		if (Input.GetMouseButton (1)) {
			camRotation.x -= Input.GetAxis ("Mouse Y") * lookSensitivity;
			camRotation.y += Input.GetAxis ("Mouse X") * lookSensitivity;

			camRotation.x = Mathf.Max (minRot, Mathf.Min (camRotation.x, maxRot));
		}

		if (Input.mouseScrollDelta.y != 0) {

			float zValue = Camera.main.transform.localPosition.z + Input.mouseScrollDelta.y * zoomSpeed;
			actualZValueOfCam = Mathf.Max(-maxZoom, Mathf.Min(-minZoom, zValue));
		}






		actualZValueOfCam = Mathf.Lerp (Camera.main.transform.localPosition.z, actualZValueOfCam, smoothCoef);
		Camera.main.transform.localPosition = new Vector3(0f, 0f, actualZValueOfCam);


		actualCamRotation = Vector2.Lerp (actualCamRotation, camRotation, smoothCoef);
		transform.rotation = Quaternion.Euler (actualCamRotation.x, actualCamRotation.y, 0f);
		
	}
}
