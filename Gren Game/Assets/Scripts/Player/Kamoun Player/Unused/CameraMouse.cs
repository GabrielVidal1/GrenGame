using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMouse : MonoBehaviour
{

	public float SpeedH = 2f;
	public float SpeedV = 2f;

	private float yaw = 0f;
	private float pitch = 0f;
	void Start () {
		
		
	}
	
	
	void Update ()
	{

		yaw += SpeedH * Input.GetAxis("Mouse X");
		pitch -= SpeedV * Input.GetAxis("Mouse Y");
		
		transform.eulerAngles = new Vector3(pitch, yaw, 0f);




	}
}
