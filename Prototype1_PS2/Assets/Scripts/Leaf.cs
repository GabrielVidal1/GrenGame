using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaf : MonoBehaviour {

	[Range(0f, 1f)]
	public float time;

	[Range(1, 20)] public int nbOfTangentSegments;
	[Range(1, 20)] public int nbOfNormalSegments;


	public Vector3 initialDirection;
	public Vector3 upDirection;

	[Header("Dimensions")]

	public float width;
	public float length;


	public AnimationCurve widthOverTime;
	public AnimationCurve lengthOverTime;


	[Space()]
	public AnimationCurve pointsDistributionOverLength;
	public AnimationCurve pointsDistributionOverTime;

	[Header("Transverse Plane")]
	public AnimationCurve topDownShape;

	[Header("Coronal Plane")]
	public bool coronalVariations;
	public float coronalCurvatureIntensity;
	public AnimationCurve coronalCurvature;
	public AnimationCurve coronalCurvatureIntensityOverLength;
	public AnimationCurve coronalCurvatureIntensityOverTime;


	[Header("Sagittal Plane")]
	public bool sagittalVariations;
	public float sagittalCurvatureIntensity;
	public AnimationCurve sagittalCurvature;
	public AnimationCurve sagittalCurvatureIntensityOverLength;
	public AnimationCurve sagittalCurvatureIntensityOverTime;

	private MeshFilter mf;
	private List<Vector3> points;
	private int[] triangles;
	private Vector2[] uvs;


	//INITIALIZE THE TAB OF TRIANGLES AND POINTS
	public void Initialize()
	{
		mf = GetComponent<MeshFilter> ();

		//TRIANGLES
		triangles = GenerateTriangles ().ToArray();

		//UVs
		uvs = GenerateUVMap().ToArray();

		UpdateMesh ();
	}

	//UPDATE THE TAB OF POINTS OF THE MESH TO MATCH THE TIME
	public void UpdateMesh()
	{
		//UPDATE POINTS
		points = new List<Vector3>();
		points.Clear ();
		points = GeneratePoints ();

		//MODIFY MESH

		Mesh m = new Mesh ();

		m.vertices = points.ToArray ();
		m.triangles = triangles;
		m.uv = uvs;

		m.RecalculateNormals ();

		mf.mesh = m;

	}

	//GENERATE THE LIST OF TRIANGLES FOR THE MESH CREATION
	public List<int> GenerateTriangles()
	{
		List<int> triangles = new List<int> ();

		int nbOfNormals = nbOfNormalSegments + 1;

		for (int i = 0; i < nbOfTangentSegments; i++) 
		{


			for (int j = 0; j < nbOfNormalSegments; j++) 
			{

				int p1 = nbOfNormals * i + j;
				int p2 = nbOfNormals * i + (j + 1);

				int p3 = nbOfNormals * (i + 1) + j;
				int p4 = nbOfNormals * (i + 1) + (j + 1);

				triangles.Add (2 * p1);
				triangles.Add (2 * p2);
				triangles.Add (2 * p3);

				triangles.Add (2 * p2);
				triangles.Add (2 * p4);
				triangles.Add (2 * p3);

				triangles.Add (2 * p1 + 1);
				triangles.Add (2 * p3 + 1);
				triangles.Add (2 * p2 + 1);

				triangles.Add (2 * p2 + 1);
				triangles.Add (2 * p3 + 1);
				triangles.Add (2 * p4 + 1);
			}
		}

		return triangles;
	}
	
	//GENERATE THE POINTS FOR THE CREATION OF THE MESH
	public List<Vector3> GeneratePoints()
	{
		List<Vector3> points = new List<Vector3> ();

		Vector3 u = initialDirection.normalized;	
		Vector3 v = Vector3.Cross (u, upDirection).normalized;
		Vector3 up = upDirection;


		float halfWidth = width * 0.5f;

		for (int i = 0; i < nbOfTangentSegments + 1; i++) 
		{

			float lengthRatio = i / (float)nbOfTangentSegments;

			//if (lengthOverTime.Evaluate(time) < lengthRatio )



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


				points.Add (point);
				points.Add (point);
			}
		}

		return points;

	}

	//GENERATE THE MAP OF UV
	public List<Vector2> GenerateUVMap()
	{
		List<Vector2> uv = new List<Vector2> ();

		for (int i = 0; i < nbOfTangentSegments + 1; i++) 
		{
			float lengthRatio = i / (float)nbOfTangentSegments;
			for (int j = 0; j < nbOfNormalSegments + 1; j++) 
			{
				float ratio = j / (float)nbOfNormalSegments ;

				uv.Add (new Vector2 (lengthRatio, ratio));
				uv.Add (new Vector2 (lengthRatio, 1f - ratio));
			}
		}
		return uv;

	}
}
