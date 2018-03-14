﻿using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class Mouselook : MonoBehaviour
{

	public float lookSensitivity = 5f;
	public float xRotation;
	public float yRotation;
	

	

	void Start () {
		
	}
	

	void Update ()
	{
		/* CORRECTION
		if (Input.GetKey(KeyCode.Escape))
			Screen.lockCursor = false;
		else
			Screen.lockCursor = true;
		*/

		if (!GameManager.gm.isGamePaused ()) {


			Cursor.lockState = CursorLockMode.Locked;


			xRotation -= Input.GetAxis ("Mouse Y") * lookSensitivity;
			yRotation += Input.GetAxis ("Mouse X") * lookSensitivity;

			//CORRECTION
			xRotation = Mathf.Max (-90f, Mathf.Min (90f, xRotation));
		
			transform.rotation = Quaternion.Euler (xRotation, yRotation, 0);
		} else
			Cursor.lockState = CursorLockMode.None;
	}
}