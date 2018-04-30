using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
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
		if (!CanvasManager.cm.inGameMenu.isPaused) {
			if (CC.isGrounded) {
				moveDirection = new Vector3 (Input.GetAxis ("Horizontal"), 0, Input.GetAxis ("Vertical"));
				moveDirection = transform.TransformDirection (moveDirection);
				moveDirection *= Speed;

				if (Input.GetButtonDown ("Jump")) {
					moveDirection.y = jumpSpeed;
				}
			}
		
			moveDirection.y -= gravity * Time.deltaTime;
			//transform.rotation = Quaternion.Euler(camera01.GetComponent<Mouselook>().xRotation, camera01.GetComponent<Mouselook>().yRotation, 0);
			transform.rotation = Quaternion.Euler (0, camera01.GetComponent<Mouselook> ().yRotation, 0);


			CC.Move (moveDirection * Time.deltaTime);
		

		}
	}
     		
}
*/

public class PlayerCC : MonoBehaviour
{

	public float horizontalSpeed = 6f;

	public float verticalSpeed;

	public float hooverHeigth;
	public float jumpHeigth;
    public float sprint;

	CharacterController CC;
	public GameObject camera01;

	[SerializeField]
	private float height;

	bool moved = false;

	[SerializeField]
	private float groundHeigth;

	Vector3 moveDirection;

	void Start ()
	{


		CC = GetComponent<CharacterController>();
		height = hooverHeigth;


	}

	// Update is called once per frame
	void Update ()
	{
		if (!CanvasManager.cm.inGameMenu.isPaused
			&& !CanvasManager.cm.playerInventoryGrid.Opened) {


			//if (moved) {
			Ray ray = new Ray (transform.position, Vector3.down);
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit, 100f)) {
				groundHeigth = hit.point.y;
			}
            //moved = false;
            //}

            if (Input.GetKey(KeyCode.LeftShift))
                horizontalSpeed = sprint;
            else
                horizontalSpeed = 6f;

            moveDirection = new Vector3 (Input.GetAxis ("Horizontal"), moveDirection.y, Input.GetAxis ("Vertical"));
			moveDirection = transform.TransformDirection (moveDirection);
			moveDirection *= horizontalSpeed * Time.deltaTime;

			//if (moveDirection != Vector3.zero)
			//	moved = true;

		} else {
			moveDirection = Vector3.zero;
		}


		if (Input.GetButton ("Jump")) {
			height = jumpHeigth;
		} else  {
			height = hooverHeigth;
		}

		moveDirection.y = Mathf.Lerp (0, (height  + groundHeigth)- transform.position.y, verticalSpeed * Time.deltaTime);
		CC.Move (moveDirection);
	
	}

}