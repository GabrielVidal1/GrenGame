﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flower : MonoBehaviour {

	[Range(0f, 1f)]
	public float time;

	public float radius;
	public AnimationCurve radiusOverTime;

	[Range(3, 20)]
	public int nbOfPetals;

	public int nbOfSegments;

	public AnimationCurve segmentPointDistribution;

	public float length;

	public Vector3 initialDirection;

	public AnimationCurve sideShape;
	public AnimationCurve sideShapeOverTime;
	public float sideShapeCoef;

	public AnimationCurve closureForce;
	public float closureForceCoef;

	public bool flipTexture;

	[Range(0f, 2f)]
	public float textureSize;

	private MeshFilter mf;
	private Vector3[] points;
	private int[] triangles;
	private Vector2[] uvs;


	void Start () 
	{
		
	}


	//INITIALIZE THE TAB OF TRIANGLES AND POINTS
	public void Initialize()
	{
		mf = GetComponent<MeshFilter> ();

		//TRIANGLES
		triangles = new int[6*nbOfPetals*(1+(nbOfSegments-1)*2)];
		//Debug.Log ("t Length : " + triangles.Length);
		GenerateTriangles (triangles);

		//POINTS
		points = new  Vector3[(1 + (nbOfPetals + 1) * nbOfSegments) * 2 ];
		//GeneratePoints (points);

		//UVs
		uvs = new Vector2[points.Length];
		GenerateUVMap(uvs);


		UpdateMesh ();
	}

	// Update is called once per frame
	void Update () {
		
	}

	public void UpdateMesh()
	{

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
		points [0] = Vector3.zero;
		points [1] = Vector3.zero;

		Vector3 u = Vector3.Cross (initialDirection, Noise3D.RandomVector()).normalized;	
		Vector3 v = Vector3.Cross (u, initialDirection).normalized;
		Vector3 w = initialDirection;

		Vector3 directionNorm = initialDirection.normalized;

		float constant = 2f * Mathf.PI / (float)nbOfPetals; 

		for (int i = 0; i < nbOfPetals + 1; i++) 
		{

			float angleRatio = i * constant;

			for (int j = 0; j < nbOfSegments; j++) 
			{

				float ratio = (j + 1) / (float)nbOfSegments ;



				Vector3 dir = (Mathf.Cos (angleRatio) * u +
					Mathf.Sin (angleRatio) * v).normalized;


				Vector3 point = dir * radius * segmentPointDistribution.Evaluate (ratio) * radiusOverTime.Evaluate(time);

				point -= dir * radius * closureForce.Evaluate(ratio) * closureForceCoef;

				point += w * sideShape.Evaluate (ratio) * sideShapeCoef * sideShapeOverTime.Evaluate(time);
					


				int index = 1 + j + i * nbOfSegments;

				//Debug.Log ("p_" + index + " : " + point);
				//Debug.DrawRay (transform.position + point, Vector3.up * 0.1f, Color.red, 10f);

				points [2 * index] = point;
				points [2 * index + 1] = point;

			}
		}
	}

	void GenerateTriangles(int[] triangles)
	{
		int actual_index = 0;
		for (int i = 0; i < nbOfPetals; i++) 
		{
				for (int j = 0; j < nbOfSegments; j++) {

				int index = i * nbOfSegments + j;

				//Debug.Log (i + "*" + nbOfSegments + "+" + j + " = " + index + "        " + (index * 12 + 12));
				//Debug.Log ("actual index : " + actual_index);

				if (j == 0) {
					
					int p1 = 0;
					int p2 = nbOfSegments * i + 1;
					int p3 = nbOfSegments * (i + 1) + 1;

					triangles [actual_index] = 2 * p1;
					actual_index++;
					triangles [actual_index] = 2 * p2;
					actual_index++;
					triangles [actual_index] = 2 * p3;
					actual_index++;

					triangles [actual_index] = 2 * p1 + 1;
					actual_index++;
					triangles [actual_index] = 2 * p3 + 1;
					actual_index++;
					triangles [actual_index] = 2 * p2 + 1;
					actual_index++;
					/*
					triangles [12 * index + 0] = 2 * p1;
					triangles [12 * index + 1] = 2 * p3;
					triangles [12 * index + 2] = 2 * p2;

					triangles [12 * index + 3] = 2 * p1 + 1;
					triangles [12 * index + 4] = 2 * p2 + 1;
					triangles [12 * index + 5] = 2 * p3 + 1;
*/
				} else {


					int p1 = nbOfSegments * i + j;
					int p2 = nbOfSegments * i + (j + 1);

					int p3 = nbOfSegments * (i + 1) + j;
					int p4 = nbOfSegments * (i + 1) + (j + 1);

					triangles [actual_index] = 2 * p1;
					actual_index++;
					triangles [actual_index] = 2 * p2;
					actual_index++;
					triangles [actual_index] = 2 * p3;
					actual_index++;

					triangles [actual_index] = 2 * p2;
					actual_index++;
					triangles [actual_index] = 2 * p4;
					actual_index++;
					triangles [actual_index] = 2 * p3;
					actual_index++;

					triangles [actual_index] = 2 * p1 + 1;
					actual_index++;
					triangles [actual_index] = 2 * p3 +1;
					actual_index++;
					triangles [actual_index] = 2 * p2 + 1;
					actual_index++;

					triangles [actual_index] = 2 * p2 + 1;
					actual_index++;
					triangles [actual_index] = 2 * p3 +1;
					actual_index++;
					triangles [actual_index] = 2 * p4 + 1;
					actual_index++;

					/*
					triangles [12 * index + 0] = 2 * p1;
					triangles [12 * index + 1] = 2 * p2;
					triangles [12 * index + 2] = 2 * p3;

					triangles [12 * index + 3] = 2 * p2;
					triangles [12 * index + 4] = 2 * p4;
					triangles [12 * index + 5] = 2 * p3;

					triangles [12 * index + 6] = 2 * p1 + 1;
					triangles [12 * index + 7] = 2 * p3 + 1;
					triangles [12 * index + 8] = 2 * p2 + 1;

					triangles [12 * index + 9] = 2 * p2 + 1;
					triangles [12 * index + 10] = 2 * p3 + 1;
					triangles [12 * index + 11] = 2 * p4 + 1;
					*/
				}
			}
		}
	}

	void GenerateUVMap(Vector2[] uvs)
	{
		float constant = 2f * Mathf.PI / (float)nbOfPetals; 

		Vector2 p1 = new Vector2 (0.5f, 0.25f);
		Vector2 p2 = new Vector2 (0.5f, 0.75f);

		if (flipTexture) {
			Vector2 temp = p1;
			p1 = p2;
			p2 = temp;
		}


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



	}

}