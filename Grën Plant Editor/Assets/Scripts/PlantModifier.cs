using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantModifier : MonoBehaviour {

	[SerializeField] private Plant plant;


	[SerializeField] private ManipulationNode radiusNode;
	[SerializeField] private ManipulationNode lengthNode;


	[SerializeField] private ManipulationNode[] shapeNodes;

	void Start () {

		radiusNode.Initialize ();
		radiusNode.SetValueX (plant.initialRadius);


		lengthNode.Initialize ();
		lengthNode.yRatio = 1f;
		lengthNode.SetValueY (plant.nbOfSegments * plant.initialSegmentLength);


		for (int i = 0; i < shapeNodes.Length; i++) {


			float ratio = ((float)i / shapeNodes.Length);

			Debug.Log (ratio);

			shapeNodes [i].Initialize ();
			shapeNodes [i].yRatio = plant.nbOfSegments * plant.initialSegmentLength;
			shapeNodes [i].zRatio = plant.initialRadius;

			shapeNodes [i].SetValueY (ratio * (plant.nbOfSegments * plant.initialSegmentLength));
			shapeNodes [i].SetValueZ (plant.finalShapeOverLength.Evaluate (ratio ));



		}

	}
	
	// Update is called once per frame
	void Update () {
		
	}



	public void ChangeInitialRadius(float radius)
	{
		plant.initialRadius = radius;
		plant.InitializePlant ();

		for (int i = 0; i < shapeNodes.Length; i++) {
			shapeNodes [i].zRatio = plant.initialRadius;
			shapeNodes [i].SetValueZ (plant.finalShapeOverLength.Evaluate (shapeNodes [i].zValue / (plant.nbOfSegments * plant.initialSegmentLength)));

		}
	}

	public void ChangeNbOfSegments(float value)
	{
		plant.nbOfSegments = (int)(value / plant.initialSegmentLength) + 2;
		plant.InitializePlant ();

		for (int i = 0; i < shapeNodes.Length; i++) {
			shapeNodes [i].yRatio = plant.nbOfSegments * plant.initialSegmentLength;
		}

	}

	public void UpdateShape(ManipulationNode node)
	{
		//AnimationCurve curve = plant.finalShapeOverLength;

		float totalLength = plant.nbOfSegments * plant.initialSegmentLength;

		Keyframe[] keys = new Keyframe[shapeNodes.Length];
		for (int i = 0; i < shapeNodes.Length; i++) {
			
			keys [i] = new Keyframe (shapeNodes[i].yValue / totalLength, shapeNodes[i].zValue / plant.initialRadius);
		}
		Debug.Log ("Update");
		plant.finalShapeOverLength = new AnimationCurve (keys);
		plant.InitializePlant ();
	}


}
