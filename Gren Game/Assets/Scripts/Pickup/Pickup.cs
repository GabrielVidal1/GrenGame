using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Pickup : MonoBehaviour {

	[SerializeField]
	[Range(0.01f, 1f)]
	protected float pickupSpeed = 0.15f;


	protected Collider col;

	protected virtual void Init()
	{
		col = GetComponent<Collider> ();


		//GameManager.gm.pm.collectibles.Add (this);
	}


	public virtual void PickupItem(PlayerInventory playerInventory)	
	{
		StartCoroutine (GetPickedUpByPlayer (playerInventory.transform));
	}

	IEnumerator GetPickedUpByPlayer(Transform playerTransform)
	{
		col.enabled = false;

		Vector3 initialPosition = transform.position;

		for (float i = 0f; i < 1; i += pickupSpeed) {
			transform.position = Vector3.Lerp (initialPosition, playerTransform.position, i);
			yield return null;
		}

		col.enabled = true;
		gameObject.SetActive (false);
	}

}
