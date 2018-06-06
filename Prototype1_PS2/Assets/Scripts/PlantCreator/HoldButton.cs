using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine;

[System.Serializable]
public class OnHoldEvent : UnityEvent {}

public class HoldButton : MonoBehaviour, IPointerDownHandler,  IPointerUpHandler{

	[SerializeField] private OnHoldEvent onHoldEvent;

	bool going;

	public void OnPointerDown(PointerEventData data)
	{
		going = true;
	}

	public void OnPointerUp(PointerEventData data)
	{
		going = false;
	}

	void Update()
	{
		if (going)
			onHoldEvent.Invoke ();
	}
}
