using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine;

[System.Serializable]
public class ClickEvent : UnityEvent {}

public class ClickAndHoldButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

	public float holdingDuration;

	public ClickEvent startClickEvent;
	public ClickEvent midClickEvent;
	public ClickEvent endClickEvent;

	public float progress;

	private float startingTime;
	private bool go;

	public void OnPointerDown(PointerEventData p)
	{
		Debug.Log ("CLICK");
		go = true;
		startingTime = Time.time;
		progress = 0f;

		startClickEvent.Invoke ();

	}
	/*
	public void OnDrag(PointerEventData p)
	{
		Debug.Log ("DRAG");


		if (go) {
				
			if (startingTime + holdingDuration < Time.time) {
				go = false;
				endClickEvent.Invoke ();
				Debug.Log ("END");
				return;
			}

			progress = (Time.time - startingTime) / holdingDuration;
		}
	}
*/
	void Update()
	{
		if (go){
			Debug.Log ("DRAG");

			if (startingTime + holdingDuration < Time.time) {
				go = false;
				endClickEvent.Invoke ();
				Debug.Log ("END");
				return;
			}

			progress = (Time.time - startingTime) / holdingDuration;
		}
			

	}

	public void OnPointerUp(PointerEventData p)
	{
		Debug.Log ("UP TO LATE");

		if (go) {
			midClickEvent.Invoke ();
			go = false;
		}
	}

}
