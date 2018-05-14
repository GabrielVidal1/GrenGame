using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantModifier : MonoBehaviour {
	/*
	[SerializeField] private Plant plant;


	[SerializeField] private ManipulationNode radiusNode;
	[SerializeField] private ManipulationNode lengthNode;


	[SerializeField] private ManipulationNode[] shapeNodes;

	void Start () {

		radiusNode.Initialize ();
		radiusNode.xValue = plant.InitialRadius;
		radiusNode.xRange = new Vector2 (0.01f, 3f);

		lengthNode.Initialize ();
		lengthNode.yRatio = 1f;
		lengthNode.yValue = plant.nbOfSegments * plant.initialSegmentLength;


		for (int i = 0; i < shapeNodes.Length; i++) {


			//float ratio = ((float)i / shapeNodes.Length);

			//Debug.Log (ratio);
			shapeNodes[i].yRange = new Vector2(0f, 1f);
			shapeNodes[i].zRange = new Vector2(0.01f, 1f);


			shapeNodes [i].Initialize ();
			shapeNodes [i].yRatio = plant.nbOfSegments * plant.initialSegmentLength;
			shapeNodes [i].zRatio = plant.InitialRadius;

			//shapeNodes [i].SetValueY (ratio * (plant.nbOfSegments * plant.initialSegmentLength));
			shapeNodes [i].zValue = plant.finalShapeOverLength.Evaluate (shapeNodes [i].yValue);



		}

	}
	
	// Update is called once per frame
	void Update () {
		
	}



	public void ChangeInitialRadius(float radius)
	{
		//Debug.Log ("RADIUS : " + radius);
		plant.initialRadius = radius;
		plant.InitializePlant ();

		Debug.Log ("changed radius");


		for (int i = 0; i < shapeNodes.Length; i++) {
			shapeNodes [i].zRatio = plant.initialRadius;
			shapeNodes [i].zValue = plant.finalShapeOverLength.Evaluate (shapeNodes [i].yValue);

		}
	}

	public void ChangeNbOfSegments(float value)
	{
		plant.nbOfSegments = (int)(value / plant.initialSegmentLength) + 2;
		plant.InitializePlant ();

		Debug.Log ("changed nb of segments");

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
			
			keys [i] = new Keyframe (shapeNodes[i].yValue, shapeNodes[i].zValue);

			shapeNodes [i].zValue = plant.finalShapeOverLength.Evaluate (shapeNodes[i].yValue );

		}
		Debug.Log ("changed shape");
		plant.finalShapeOverLength = new AnimationCurve (keys);
		plant.InitializePlant ();
	}
*/

}
