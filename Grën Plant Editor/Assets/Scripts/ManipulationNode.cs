using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManipulationNode : MonoBehaviour {

	public int nodeIndex;

	public Vector3 axe;

	public NodeType nodeType;

	[SerializeField] private Material activeMaterial;
	[SerializeField] private Material inactiveMaterial;

	private MeshRenderer mr;
	private Collider col;
	void Start()
	{
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
}
