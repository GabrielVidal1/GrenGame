using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CameraManipulation : MonoBehaviour {

	public float maxRot = 90f;
	public float minRot = 0f;
	public float lookSensitivity = 3f;

	private Vector2 camRotation;
	private Vector2 actualCamRotation;

	public float minHeight;
	public float maxHeight;
	public float ascendingSpeed;

	public float maxZoom = 10f;
	public float minZoom = 1f;
	public float zoomSpeed = 3f;

	private float actualZValueOfCam;

	[Range(0.1f, 1f)]
	public float smoothCoef;


	[SerializeField] private Camera previewCamera;

	[SerializeField] private RawImage targetTexture;

	private RectTransform previewArea;

	private bool holdRightClick;

	private float yOrigin;
	private float targetY;
	void Start () 
	{
		actualZValueOfCam = Camera.main.transform.position.z;

		previewArea = targetTexture.GetComponent<RectTransform> ();

		Rect rect = previewArea.rect;

		RenderTexture rt = new RenderTexture ((int)rect.width, (int)rect.height, 24);

		previewCamera.targetTexture = rt;

		targetTexture.texture = rt;

		yOrigin = transform.position.y;
	}

	public void GoUp()
	{
		targetY += Time.deltaTime * ascendingSpeed;
		targetY = Mathf.Min (targetY, maxHeight);
	}

	public void GoDown()
	{
		targetY -= Time.deltaTime * ascendingSpeed;
		targetY = Mathf.Max (targetY, minHeight);

	}


	void Update () 
	{

		bool inPreviewWindow = RectTransformUtility.RectangleContainsScreenPoint (previewArea, Input.mousePosition);
		//Debug.Log (inPreviewWindow);

		if (inPreviewWindow && !holdRightClick) {
			if (Input.GetMouseButtonDown (1))
				Cursor.visible = false;
			else if (Input.GetMouseButtonUp (1))
				Cursor.visible = true;
		
			if (Input.GetMouseButton (1)) {
				camRotation.x -= Input.GetAxis ("Mouse Y") * lookSensitivity;
				camRotation.y += Input.GetAxis ("Mouse X") * lookSensitivity;

				camRotation.x = Mathf.Max (minRot, Mathf.Min (camRotation.x, maxRot));
			}

			if (Input.mouseScrollDelta.y != 0) {

				float zValue = previewCamera.transform.localPosition.z + Input.mouseScrollDelta.y * zoomSpeed;
				actualZValueOfCam = Mathf.Max (-maxZoom, Mathf.Min (-minZoom, zValue));
			}
		} else {
			
			Cursor.visible = true;

			if (Input.GetMouseButtonDown (1))
				holdRightClick = true;
			else if (Input.GetMouseButtonUp (1))
				holdRightClick = false;
		}





		actualZValueOfCam = Mathf.Lerp (previewCamera.transform.localPosition.z, actualZValueOfCam, smoothCoef);
		previewCamera.transform.localPosition = new Vector3(0f, 0f, actualZValueOfCam);


		actualCamRotation = Vector2.Lerp (actualCamRotation, camRotation, smoothCoef);
		transform.rotation = Quaternion.Euler (actualCamRotation.x, actualCamRotation.y, 0f);


		transform.position = Vector3.Lerp (transform.position, 
			new Vector3 (transform.position.x, yOrigin + targetY, transform.position.z), smoothCoef);
	}
}

