using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaf : MonoBehaviour {

	[Range(0f, 1f)]
	public float time;

	public int nbOfTangentSegments;
	public int nbOfNormalSegments;


	public Vector3 initialDirection;

	[Header("Dimensions")]

	public float width;

	public AnimationCurve widthOverTime;

	public float length;


	[Space()]
	public AnimationCurve tangeantPointsDistribution;

	[Header("Shape")]

	public AnimationCurve topDownShape;

	public float normalStrengh;
	public AnimationCurve normalCurvature;
	public AnimationCurve normalIntensity;

	public AnimationCurve normalStrenghOverTime;


	public float tangentNormalStrengh;
	public AnimationCurve tangentNormalCurvature;


	private MeshFilter mf;
	private List<Vector3> points;
	private List<int> triangles;


	public void Initialize()
	{
		mf = GetComponent<MeshFilter> ();

		UpdateMesh ();

		//for (int i = 0; i < points.Count; i++)
			//print ("p" + i.ToString () + " : " + points [i].ToString ());





	}

	public void UpdateMesh()
	{
		//UPDATE POINTS
		points = new List<Vector3>();
		points.Clear ();
		points = GeneratePoints ();

		//TRIANGLES
		triangles = new List<int>();
		triangles.Clear ();
		triangles = GenerateTriangles ();

		//MODIFY MESH
		Mesh m = new Mesh();
		m.vertices = points.ToArray ();
		m.triangles = triangles.ToArray ();

		m.RecalculateNormals ();

		mf.mesh = m;
	}

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

				//print ("(" + p1 + ", " + p2 + ", " + p3 + ")");
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
	

	public List<Vector3> GeneratePoints()
	{
		List<Vector3> points = new List<Vector3> ();

		Vector3 u = initialDirection.normalized;	
		Vector3 v = Vector3.Cross (u, Vector3.up).normalized;

		Vector3 normal = Vector3.Cross (v, u);

		float halfWidth = width * 0.5f;

		for (int i = 0; i < nbOfTangentSegments + 1; i++) 
		{

			float lengthRatio = i / ((float)nbOfTangentSegments + 0f);
			//print (lengthRatio + "  ->  " + topDownShape.Evaluate (lengthRatio));


			for (int j = 0; j < nbOfNormalSegments + 1; j++) 
			{
				
				float ratio = (j / ((float)nbOfNormalSegments + 0f)) ;
				//print (ratio + "    " + (ratio - 0.5f));

				float pointsDistribution = tangeantPointsDistribution.Evaluate (lengthRatio);

				float pointsDistributionOverTime = Mathf.Lerp (0, pointsDistribution, time);

				float tangentDirection = Mathf.Lerp(0, length, pointsDistributionOverTime);

				float normalDirection = Mathf.Lerp(0, width, widthOverTime.Evaluate(time)) * (ratio - 0.5f) * topDownShape.Evaluate (pointsDistributionOverTime);

				float normalUpDirection = normalCurvature.Evaluate (Mathf.Abs (2f * ratio - 1f)) 
					* normalStrengh * normalStrenghOverTime.Evaluate(time)
					* normalIntensity.Evaluate (lengthRatio);

				normalUpDirection += tangentNormalCurvature.Evaluate (pointsDistributionOverTime) * tangentNormalStrengh;

				Vector3 point = normalDirection * v
				                + tangentDirection * u
					+ normal * normalUpDirection;


				//Debug.DrawRay (transform.position + point, Vector3.up * 0.1f, Color.red, 10f);


				points.Add (point);
				points.Add (point);
			}
		}

		return points;

	}


}
