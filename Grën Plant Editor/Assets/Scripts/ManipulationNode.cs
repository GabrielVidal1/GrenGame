using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

[System.Serializable]
public class ChangeEvent : UnityEvent<float> {}

[System.Serializable]
public class ChangeEventNode : UnityEvent<ManipulationNode> {}

public class ManipulationNode : MonoBehaviour {

	[SerializeField] public bool xAxis;
	[SerializeField] public Vector2 xRange;
	public float xRatio;
	public float xValue
	{
		get { return ((transform.position - origin).x - xRange.x) / xRatio; }
		set { SetValueX (value); }
	}

	[SerializeField] public bool yAxis;
	[SerializeField] public Vector2 yRange;
	public float yRatio;
	public float yValue
	{
		get { return ((transform.position - origin).y - yRange.x) / yRatio; }
		set { SetValueY (value); }
	}

	[SerializeField] public bool zAxis;
	[SerializeField] public Vector2 zRange;
	public float zRatio;
	public float zValue
	{
		get { return ((transform.position - origin).z - zRange.x) / zRatio; }
		set { SetValueZ (value); }
	}

	[SerializeField] private ChangeEvent OnChangeValueX;
	[SerializeField] private ChangeEvent OnChangeValueY;
	[SerializeField] private ChangeEvent OnChangeValueZ;

	[SerializeField] private ChangeEventNode OnChangeValue;

	[SerializeField] private Material activeMaterial;
	[SerializeField] private Material inactiveMaterial;

	[SerializeField] private Material transparent;

	private MeshRenderer mr;
	private Collider col;


	private GameObject back;

	private bool isHeld;
	private bool initialized;

	private Vector3 origin;


	public void Initialize()
	{
		origin = transform.position;
		mr = GetComponent<MeshRenderer> ();
		mr.material = inactiveMaterial;
		col = GetComponent<Collider> ();
	}

	public void SetActiveColor()
	{
		mr.material = activeMaterial;
	}

	public void SetInactiveColor()
	{
		mr.material = inactiveMaterial;
	}

	public void EnableCollider()
	{
		col.enabled = true;
	}

	public void DisableCollider()
	{
		col.enabled = false;
	}



	void OnMouseStay()
	{
		//PREVIEW
	}


	void SetValueX(float value)
	{
		transform.position = new Vector3(origin.x + xRatio * value, transform.position.y, transform.position.z );
		//transform.position = origin + Vector3.right * xRatio * value;
	}

	void SetValueY(float value)
	{
		transform.position = new Vector3(transform.position.x, origin.y + yRatio * value, transform.position.z);
		//transform.position = origin + Vector3.up * yRatio * value;
	}

	void SetValueZ(float value)
	{
		transform.position = new Vector3(transform.position.x, transform.position.y, origin.z + zRatio * value);
	}

	void OnMouseDown()
	{
		isHeld = true;

		back = GameObject.CreatePrimitive (PrimitiveType.Cube);
		back.name = "NodeBack";
		back.GetComponent<MeshRenderer> ().material = transparent;
		back.transform.position = origin;


		if (xAxis)
			back.transform.Translate (new Vector3 ((xRange.x + xRange.y) / 2f, 0f, 0f));
		if (yAxis)
			back.transform.Translate (new Vector3 (0f, (yRange.x + yRange.y) / 2f, 0f));
		if (zAxis)
			back.transform.Translate (new Vector3 (0f, 0f, (zRange.x + zRange.y) / 2f));


		float xScale = Mathf.Abs(xRange.y - xRange.x) * xRatio;
		float yScale = Mathf.Abs(yRange.y - yRange.x) * yRatio;
		float zScale = Mathf.Abs(zRange.y - zRange.x) * zRatio;

		//back.transform.localScale = new Vector3 (xScale, yScale, zScale);
		back.transform.localScale = Vector3.one * 20f;

		int nb = 0;
		nb += xAxis ? 1 : 0;
		nb += yAxis ? 2 : 0;
		nb += zAxis ? 4 : 0;

		if (nb == 7)
			Debug.LogError ("Impossible to handle 3 axis !");


		float minScale = 0.1f;

		if (zAxis && !xAxis) {
			back.transform.localScale = new Vector3 (minScale, back.transform.localScale.y, back.transform.localScale.z);
		}
		if (zAxis && xAxis) {
			back.transform.localScale = new Vector3 (back.transform.localScale.x,minScale, back.transform.localScale.z);
		}
		if (xAxis && !zAxis || yAxis && !xAxis && !zAxis) {
			back.transform.localScale = new Vector3 (back.transform.localScale.x, back.transform.localScale.y, minScale);
		}
			
		back.layer = LayerMask.NameToLayer ("ManipulationNodeZone");
	}

	void OnMouseUp()
	{

		Destroy (back, 3f);

		isHeld = false;
	}

	void Update()
	{
		if (isHeld) {

			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit, 1000f, LayerMask.GetMask("ManipulationNodeZone"))) {

				transform.position = hit.point;

				transform.position = origin +
				new Vector3 (
						xAxis ? Mathf.Max (Mathf.Min ((transform.position - origin).x, xRatio * xRange.y), xRatio * xRange.x) : 0f,				
						yAxis ? Mathf.Max (Mathf.Min ((transform.position - origin).y, yRatio * yRange.y), yRatio * yRange.x) : 0f, 
						zAxis ? Mathf.Max (Mathf.Min ((transform.position - origin).z, zRatio * zRange.y), zRatio * zRange.x) : 0f);
				

				if (xAxis) {
					OnChangeValueX.Invoke (xValue);
				}

				if (yAxis) {
					OnChangeValueY.Invoke (yValue);
				}

				if ( zAxis) {
					OnChangeValueZ.Invoke (zValue);
				}

				OnChangeValue.Invoke (this);
			}
		}
	}

}
