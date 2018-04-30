using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class Mouselook : MonoBehaviour
{

	public float lookSensitivity = 5f;
	private float xRotation;
	private float yRotation;

	void Update ()
	{
		/* CORRECTION
		if (Input.GetKey(KeyCode.Escape))
			Screen.lockCursor = false;
		else
			Screen.lockCursor = true;
		*/

		if (!CanvasManager.cm.inGameMenu.isPaused
			&& !CanvasManager.cm.playerInventoryGrid.Opened) {


			//Cursor.lockState = CursorLockMode.Locked;


			xRotation -= Input.GetAxis ("Mouse Y") * lookSensitivity;
			yRotation += Input.GetAxis ("Mouse X") * lookSensitivity;

			//CORRECTION
			xRotation = Mathf.Max (-90f, Mathf.Min (90f, xRotation));
		
			transform.rotation = Quaternion.Euler (xRotation, yRotation, 0);
		} else {
			Cursor.lockState = CursorLockMode.None;
		}

		transform.parent.rotation = Quaternion.Euler (0, yRotation, 0);


	}
}
