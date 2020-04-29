using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeEditionMode : MonoBehaviour {

	/*

	public ManipulationNode[] nodes;
	public ManipulationNode nodePrefab;

	public ManipulationNode radiusNodePrefab;
	private ManipulationNode radiusNode;

	public Plant plant;

	public ManipulationNode selectedNode;

	public Transform plantHolder;

	private float plantTotalLength;

	public NodeType active;

	public float maxRadius;

	public int keyCount;

	[SerializeField] private AnimationCurve shapeCurve;

	void Start()
	{
		plantTotalLength = plant.nbOfSegments * (plant.initialSegmentLength + plant.finalSegmentLength) / 2f;
		shapeCurve = plant.finalShapeOverLength;

		keyCount = shapeCurve.keys.Length;

		InitShapeEditionMode ();
		plant.InitializePlant ();
	}


	public void InitShapeEditionMode()
	{
		Vector3 direction = Vector3.right;

		nodes = new ManipulationNode[keyCount];

		radiusNode = Instantiate (radiusNodePrefab, 
			Vector3.forward * plant.initialRadius + plantHolder.position,
			Quaternion.identity, plantHolder).GetComponent<ManipulationNode>();




		for (int i = 0; i < shapeCurve.keys.Length; i++) {

			Keyframe key = shapeCurve.keys [i];

			Vector3 position = plantTotalLength * key.time * Vector3.up
			                   + direction * key.value * plant.initialRadius
			                   + plantHolder.position;

			ManipulationNode node = Instantiate (nodePrefab, position, Quaternion.identity, plantHolder).GetComponent<ManipulationNode>();

			node.nodeIndex = i;
			node.axe = direction;

			nodes [i] = node;


		}

	}

	void Update () 
	{
		Debug.Log ("t");

		if (Input.GetMouseButtonDown (0)) {

			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;

			if (Physics.Raycast (ray, out hit)) {
				if (hit.collider.tag == "ManipulationNode") {

					selectedNode = hit.collider.GetComponent<ManipulationNode> ();
					active = selectedNode.nodeType;
					selectedNode.SetActiveColor ();
					selectedNode.DisableCollider();


				}
			}
		}

		if (Input.GetMouseButton (0) && active != NodeType.None) {

			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;

			if (Physics.Raycast (ray, out hit)) {
				
				if (hit.collider.tag == "BackWall") {
					Vector3 correctedPosition = 
						new Vector3 (
							Mathf.Max (0f, hit.point.x),
							Mathf.Max (0f, Mathf.Min (hit.point.y, plantTotalLength)),
							0f);
					selectedNode.transform.position = correctedPosition;
					UpdateCurveForNode (selectedNode);
				}

				if (hit.collider.tag == "Ground") {

					Vector3 correctedPosition = 
						new Vector3 (
							0f,
							0f,
							Mathf.Max (0.01f, Mathf.Min (hit.point.z, maxRadius)));
					
					selectedNode.transform.localPosition = correctedPosition;
					UpdateCurveForNode (selectedNode);

				}
			}



		} else if (Input.GetMouseButtonUp (0)) {
			if (active != NodeType.None) {
				selectedNode.SetInactiveColor ();
				selectedNode.EnableCollider();
				selectedNode = null;
			}
			active = NodeType.None;
		}
	}


	private void UpdateCurveForNode(ManipulationNode node)
	{
		if (node.nodeType == NodeType.ShapeNode) {

			Keyframe[] keys = new Keyframe[keyCount];
			for (int i = 0; i < keyCount; i++) {

				keys [i] = new Keyframe (nodes [i].transform.position.y / plantTotalLength, nodes [i].transform.position.x / plant.initialRadius);

			}




			shapeCurve.keys = keys;
			plant.finalShapeOverLength.keys  = keys;

		} else if (node.nodeType == NodeType.RadiusNode) {

			Debug.Log ("set radius : " + node.transform.position.z);
			UpdatePositionOfShapeNodes ();
			plant.initialRadius = node.transform.position.z;

		}
		plant.UpdateMesh ();
	}

	void UpdatePositionOfShapeNodes()
	{
		for (int i = 0; i < shapeCurve.keys.Length; i++) {

			Keyframe key = shapeCurve.keys [i];

			Vector3 position = plantTotalLength * key.time * Vector3.up
				+ Vector3.right * key.value * plant.initialRadius;

			nodes [i].transform.localPosition = position;


		}
	}
*/
}

public enum NodeType
{
	None,
	RadiusNode,
	ShapeNode
}