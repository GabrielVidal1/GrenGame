using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCC : MonoBehaviour
{

	public float Speed = 6f;
	public float jumpSpeed = 8f;
	public float gravity = 20f;
	private Vector3 moveDirection = Vector3.zero;
	CharacterController CC;
	public GameObject camera01;
	
	void Start ()
	{
		CC = GetComponent<CharacterController>();
		

	}
	
	// Update is called once per frame
	void Update ()
	{
		/*CORRECTION
		if (Input.GetKey(KeyCode.Escape))
			Screen.lockCursor = false;
		else
			Screen.lockCursor = true;
		*/

		if (CC.isGrounded)
		{
			moveDirection = new Vector3(Input.GetAxis("Horizontal"),0,Input.GetAxis("Vertical"));
			moveDirection = transform.TransformDirection(moveDirection);
			moveDirection *= Speed;

			if (Input.GetButtonDown("Jump"))
			{
				moveDirection.y = jumpSpeed;
			}
		}
		
		moveDirection.y -= gravity * Time.deltaTime;
		//transform.rotation = Quaternion.Euler(camera01.GetComponent<Mouselook>().xRotation, camera01.GetComponent<Mouselook>().yRotation, 0);
		transform.rotation = Quaternion.Euler(0, camera01.GetComponent<Mouselook>().yRotation, 0);
		CC.Move(moveDirection * Time.deltaTime);
		


	}
     		
}
