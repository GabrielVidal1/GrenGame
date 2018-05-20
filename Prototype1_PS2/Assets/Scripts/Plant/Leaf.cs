using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Leaf : MonoBehaviour {

	[Range(0f, 1f)]
	public float time;

	[Range(1, 20)] public int nbOfTangentSegments;
	public int NbOfTangentSegments
	{ get { return nbOfTangentSegments; } set { nbOfTangentSegments = value; } }

	[Range(1, 20)] public int nbOfNormalSegments;
	public int NbOfNormalSegments
	{ get { return nbOfNormalSegments; } set { nbOfNormalSegments = value; } }



	public Vector3 initialDirection;
	public Vector3 upDirection;

	[Header("Dimensions")]

	public float width;
	public float Width
	{ get { return width; } set { width = value; } }

	public float length;
	public float Length
	{ get { return length; } set { length = value; } }


	public AnimationCurve widthOverTime;
	public AnimationCurve lengthOverTime;


	[Space()]
	public AnimationCurve pointsDistributionOverLength;
	public AnimationCurve pointsDistributionOverTime;

	[Header("Transverse Plane")]
	public AnimationCurve topDownShape;

	[Header("Coronal Plane")]
	public bool coronalVariations;
	public bool CoronalVariations
	{ get { return coronalVariations; } set { coronalVariations = value; } }

	public float coronalCurvatureIntensity;
	public float CoronalCurvatureIntensity
	{ get { return coronalCurvatureIntensity; } set { coronalCurvatureIntensity = value; } }

	public AnimationCurve coronalCurvature;
	public AnimationCurve CoronalCurvature
	{ get { return coronalCurvature; } set { coronalCurvature = value; } }

	public AnimationCurve coronalCurvatureIntensityOverLength;
	public AnimationCurve CoronalCurvatureIntensityOverLength
	{ get { return coronalCurvatureIntensityOverLength; } set { coronalCurvatureIntensityOverLength = value; } }

	public AnimationCurve coronalCurvatureIntensityOverTime;
	public AnimationCurve CoronalCurvatureIntensityOverTime
	{ get { return coronalCurvatureIntensityOverTime; } set { coronalCurvatureIntensityOverTime = value; } }


	[Header("Sagittal Plane")]
	public bool sagittalVariations;
	public bool SagittalVariations
	{ get { return sagittalVariations; } set { sagittalVariations = value; } }

	public float sagittalCurvatureIntensity;
	public float SagittalCurvatureIntensity
	{ get { return sagittalCurvatureIntensity; } set { sagittalCurvatureIntensity = value; } }

	public AnimationCurve sagittalCurvature;
	public AnimationCurve SagittalCurvature
	{ get { return sagittalCurvature; } set { sagittalCurvature = value; } }

	public AnimationCurve sagittalCurvatureIntensityOverLength;
	public AnimationCurve SagittalCurvatureIntensityOverLength
	{ get { return sagittalCurvatureIntensityOverLength; } set { sagittalCurvatureIntensityOverLength = value; } }

	public AnimationCurve sagittalCurvatureIntensityOverTime;
	public AnimationCurve SagittalCurvatureIntensityOverTime
	{ get { return sagittalCurvatureIntensityOverTime; } set { sagittalCurvatureIntensityOverTime = value; } }

	private MeshFilter mf;
	private Vector3[] points;
	private int[] triangles;
	private Vector2[] uvs;

	private float lastUpdateTime;


	//INITIALIZE THE TAB OF TRIANGLES AND POINTS
	public void Initialize()
	{
		lastUpdateTime = 0f;
		mf = GetComponent<MeshFilter> ();

		//TRIANGLES
		triangles = new int[12 * nbOfTangentSegments * nbOfNormalSegments];

		GenerateTriangles (triangles);

		//POINTS
		points = new  Vector3[2 * (nbOfNormalSegments + 1) * (nbOfTangentSegments + 1)];
		//GeneratePoints (points);

		//UVs
		uvs = new Vector2[points.Length];
		GenerateUVMap(uvs);


		UpdateMesh ();
	}

	//UPDATE THE TAB OF POINTS OF THE MESH TO MATCH THE TIME
	public void UpdateMesh()
	{
		if (lastUpdateTime < 1f) {
			
			//UPDATE POINTS
			GeneratePoints (points);

			//MODIFY MESH
			Mesh m = mf.mesh;
			m.Clear ();

			m.vertices = points;
			m.triangles = triangles;
			m.uv = uvs;
			m.RecalculateNormals ();


			lastUpdateTime = time;
		}
	}

	//GENERATE THE LIST OF TRIANGLES FOR THE MESH CREATION
	public void GenerateTriangles(int[] triangleArrayRef)
	{
		int nbOfNormals = nbOfNormalSegments + 1;

		for (int i = 0; i < nbOfTangentSegments; i++) 
		{


			for (int j = 0; j < nbOfNormalSegments; j++) 
			{
				int index = i * nbOfNormalSegments + j;


				int p1 = nbOfNormals * i + j;
				int p2 = nbOfNormals * i + (j + 1);

				int p3 = nbOfNormals * (i + 1) + j;
				int p4 = nbOfNormals * (i + 1) + (j + 1);

				triangleArrayRef[12 * index + 0] = 2 * p1;
				triangleArrayRef[12 * index + 1] = 2 * p2;
				triangleArrayRef[12 * index + 2] = 2 * p3;

				triangleArrayRef[12 * index + 3] = 2 * p2;
				triangleArrayRef[12 * index + 4] = 2 * p4;
				triangleArrayRef[12 * index + 5] = 2 * p3;

				triangleArrayRef[12 * index + 6] = 2 * p1 + 1;
				triangleArrayRef[12 * index + 7] = 2 * p3 + 1;
				triangleArrayRef[12 * index + 8] = 2 * p2 + 1;

				triangleArrayRef[12 * index + 9] = 2 * p2 + 1;
				triangleArrayRef[12 * index + 10] = 2 * p3 + 1;
				triangleArrayRef[12 * index + 11] = 2 * p4 + 1;
			}
		}
	}
	
	//GENERATE THE POINTS FOR THE CREATION OF THE MESH
	public void GeneratePoints(Vector3[] pointsArrayRef)
	{
		Vector3 u = initialDirection.normalized;	
		Vector3 v = Vector3.Cross (u, upDirection).normalized;
		Vector3 up = upDirection;


		float halfWidth = width * 0.5f;

		for (int i = 0; i < nbOfTangentSegments + 1; i++) 
		{

			float lengthRatio = i / (float)nbOfTangentSegments;

			for (int j = 0; j < nbOfNormalSegments + 1; j++) 
			{


				float ratio = j / (float)nbOfNormalSegments ;

				float pointPos = 
					pointsDistributionOverTime.Evaluate (time) * 
					pointsDistributionOverLength.Evaluate (lengthRatio);


				float u_Multiplier = length * pointPos;
				

				float v_Multiplier = (ratio - 0.5f) * halfWidth *
				                     widthOverTime.Evaluate (time) *
				                     topDownShape.Evaluate (pointPos);

				if (lengthRatio == 1f) {
					v_Multiplier *= topDownShape.Evaluate(lengthRatio) == 1f ? 1f : 0f;
				}

				float up_Multiplier = 0f;

				//CORONAL DISPLACE CALCULATION
				if (coronalVariations)
					up_Multiplier = coronalCurvatureIntensity *
					coronalCurvature.Evaluate (Mathf.Abs (2f * ratio - 1f)) *
					coronalCurvatureIntensityOverTime.Evaluate (time) *
					coronalCurvatureIntensityOverLength.Evaluate (lengthRatio);

				//SAGITTAL DISPLACE CALCULATION
				if (sagittalVariations)
					up_Multiplier += sagittalCurvatureIntensity *
					sagittalCurvature.Evaluate (pointPos) *
					sagittalCurvatureIntensityOverTime.Evaluate (time) *
					sagittalCurvatureIntensityOverLength.Evaluate (lengthRatio);

				Vector3 point = 
					v * v_Multiplier +
					u * u_Multiplier +
					up * up_Multiplier;


				//Debug.DrawRay (transform.position + point, Vector3.up * 0.1f, Color.red, 10f);

				int index = i * (nbOfNormalSegments + 1) + j;

				pointsArrayRef [2 * index] = point;
				pointsArrayRef [2 * index + 1] = point;

				//Debug.Log ("p_" + index + " : " + point);
			}
		}
	}

	//GENERATE THE MAP OF UV
	public void GenerateUVMap(Vector2[] uvsArrayRef)
	{
		for (int i = 0; i < nbOfTangentSegments + 1; i++) 
		{
			float lengthRatio = i / (float)nbOfTangentSegments;
			for (int j = 0; j < nbOfNormalSegments + 1; j++) 
			{


				float ratio = j / (float)nbOfNormalSegments ;

				int index = i * (nbOfNormalSegments + 1) + j;

				uvsArrayRef [2 * index] = new Vector2 (lengthRatio, ratio);
				uvsArrayRef [2 * index + 1] = new Vector2 (lengthRatio, 1f - ratio);
			}
		}
	}
}
