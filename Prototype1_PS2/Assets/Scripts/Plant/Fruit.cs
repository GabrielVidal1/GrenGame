using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
//[RequireComponent(typeof(SphereCollider))]
public class Fruit : PlantSeed {

	[Range(0f, 1f)]
	public float time = 1f;

	public float radius = 0.1f;
	public AnimationCurve radiusOverLength;
	public AnimationCurve radiusOverTime;
	public AnimationCurve radiusOverAngle;


	public float length = 1f;
	public AnimationCurve lengthOverTime;

	[Range(3, 20)]
	public int nbOfSides = 3;

	[Range(2, 20)]
	public int nbOfSections = 3;
	public AnimationCurve segmentPointDistribution;
	public Vector3 initialDirection;

	private MeshFilter mf;
	private Vector3[] points;
	private int[] triangles;
	private Vector2[] uvs;

	public Flower flowerMother;

	public int fruitIndex;

	public override void PickupItem (PlayerInventory playerInventory)
	{
		base.PickupItem (playerInventory);
		flowerMother.LoseFruit ();
	}

	//INITIALIZE THE TAB OF TRIANGLES AND POINTS
	public void Initialize()
	{

		col = GetComponent<Collider> ();
		col.enabled = false;

		((SphereCollider)col).center = length * initialDirection * 0.5f;
		((SphereCollider)col).radius = length * 0.5f;


		mf = GetComponent<MeshFilter> ();

		//POINTS
		points = new  Vector3[2 + nbOfSides * (nbOfSections - 1)];
		//GeneratePoints (points);


		//TRIANGLES
		triangles = new int[3*2*nbOfSides*(nbOfSections - 1)];
		GenerateTriangles (triangles);

		//UVs
		uvs = new Vector2[points.Length];
		GenerateUVMap(uvs);


		UpdateMesh ();
	}

	public void UpdateMesh()
	{

		if (time == 1f) {

			col.enabled = true;
		}

		//UPDATE POINTS
		GeneratePoints (points);

		//MODIFY MESH
		Mesh m = mf.mesh;
		m.Clear ();

		m.vertices = points;
		m.triangles = triangles;
		m.uv = uvs;

		m.RecalculateNormals ();

	}

	void GeneratePoints(Vector3[] points)
	{

		Vector3 u = Vector3.zero;	
		Vector3 v = Noise3D.RandomVector ();
		Vector3 w = initialDirection;

		Vector3 previousAdvance = initialDirection.normalized;


		points [0] = Vector3.zero;
		points [points.Length  - 1] = previousAdvance * length * lengthOverTime.Evaluate (time);

		float constant = 2f * Mathf.PI; 

		for (int i = 0; i < nbOfSections - 1; i++) 
		{

			float lengthRatio = (float)(i+1) / (float)nbOfSections;

			Vector3 advance = previousAdvance * length * lengthOverTime.Evaluate (time) * lengthRatio;

			u = Vector3.Cross (advance, v).normalized;	
			v = Vector3.Cross (u, advance).normalized;
			/*
			if (curvature) {
				advance = Vector3.Lerp (advance, u, xCurvatureCoef);
				advance = Vector3.Lerp (advance, v, zCurvatureCoef);
			}
			*/

			previousAdvance = advance.normalized;

			for (int j = 0; j < nbOfSides; j++) 
			{


				float ratio = (j + 1) / (float)nbOfSides;

				float angleRatio = ratio * constant;

				Vector3 dir = (Mathf.Cos (angleRatio) * u +
					Mathf.Sin (angleRatio) * v).normalized;

				dir *= radiusOverAngle.Evaluate (ratio) * radius * radiusOverLength.Evaluate(lengthRatio) * radiusOverTime.Evaluate(time);

				Vector3 point = dir + advance;

				int index = j + i * nbOfSides;

				//Debug.Log ("p_" + index + " : " + point);
				//Debug.DrawRay (transform.position + point, Vector3.up * 0.1f, Color.red, 10f);

				points [index + 1] = point;

			}
		}
	}

	void GenerateTriangles(int[] triangles)
	{

		int last = points.Length - 1;
		
		int actual_index = 0;
		for (int i = 0; i < nbOfSections; i++) 
		{
			for (int j = 0; j < nbOfSides; j++) {

				int index = i * nbOfSides + j;

				//Debug.Log (i + "*" + nbOfSegments + "+" + j + " = " + index + "        " + (index * 12 + 12));
				//Debug.Log ("actual index : " + actual_index);

				if (i == 0) {

					//Debug.Log ("i == 0");

					int p1 = 0;
					int p2 = j + 1;
					int p3 = j == nbOfSides - 1 ? 1 : j + 2;

					triangles [actual_index] = p1;
					actual_index++;
					triangles [actual_index] = p2;
					actual_index++;
					triangles [actual_index] = p3;
					actual_index++;

					//Debug.Log (p1 +"  "+ p2 +"  "+ p3);


				} else if (i == nbOfSections - 1) {

					//Debug.Log ("i == " + (nbOfSections - 1) );

					int p1 = last;
					int p2 = (i-1) * nbOfSides + j + 1;
					int p3 = (i-1) * nbOfSides + (j == nbOfSides - 1 ? 1 : j + 2);

					triangles [actual_index] = p1;
					actual_index++;
					triangles [actual_index] = p3;
					actual_index++;
					triangles [actual_index] = p2;
					actual_index++;

					//Debug.Log (p1 +"  "+ p2 +"  "+ p3);


				} else {


					int p1 = nbOfSides * (i-1) + j + 1;
					int p2 = nbOfSides * (i-1) + (j == nbOfSides - 1 ? 0 : j + 1) + 1;

					int p3 = nbOfSides * i + j + 1;
					int p4 = nbOfSides * i + (j == nbOfSides - 1 ? 0 : j + 1) + 1;

					//Debug.Log (p1 +"  "+ p2 +"  "+ p3 +"  "+ p4);

					triangles [actual_index] = p1;
					actual_index++;
					triangles [actual_index] = p3;
					actual_index++;
					triangles [actual_index] = p2;
					actual_index++;

					triangles [actual_index] = p2;
					actual_index++;
					triangles [actual_index] = p3;
					actual_index++;
					triangles [actual_index] = p4;
					actual_index++;
				}
			}
		}
	}

	void GenerateUVMap(Vector2[] uvs)
	{
		/*
		float constant = 2f * Mathf.PI / (float)nbOfSides; 

		Vector2 p1 = new Vector2 (0.5f, 0.25f);
		Vector2 p2 = new Vector2 (0.5f, 0.75f);


		uvs [0] = p1;
		uvs [1] = p2;

		for (int i = 0; i < nbOfPetals + 1; i++) 
		{

			float angleRatio = i * constant;

			for (int j = 0; j < nbOfSegments; j++) 
			{


				float ratio = (j + 1) / (float)nbOfSegments ;



				Vector2 dir = Mathf.Cos (angleRatio) * new Vector2 (0f, 0.25f) +
					Mathf.Sin (angleRatio) * new Vector2 (0.5f, 0f);


				Vector2 point = dir * segmentPointDistribution.Evaluate (ratio) * textureSize;

				//point -= dir * radius * closureForce.Evaluate(ratio) * closureForceCoef;

				//point += w * sideShape.Evaluate (ratio) * sideShapeCoef;



				int index = 1 + j + i * nbOfSegments;

				//Debug.Log ("p_" + index + " : " + point);
				//Debug.DrawRay (transform.position + point, Vector3.up * 0.1f, Color.red, 10f);

				uvs [2 * index ] = p1 + point;
				uvs [2 * index + 1] = p2 + point;
			}
		}


		*/
	}
}
