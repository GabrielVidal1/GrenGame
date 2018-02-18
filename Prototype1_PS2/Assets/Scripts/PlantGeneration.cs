using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantGeneration : MonoBehaviour {

	[Header("Global Characteristics")]

	public float initialRadius;
	public float initialSegmentLength;
	public float finalSegmentLength;

	public Vector3 initalDirection;

	public int nbOfSides;
	public int nbOfSegments;

	[Header("Trajectory")]


	public Vector3[] tangentDirections;
	public Vector3[] normalDirections;



	[Header("Variation Over Lentgh")] 

	public AnimationCurve segmentLengthOverLength;



	public AnimationCurve radiusOverLength;


	[Header("Variation Over Time")]


	[Range(0f, 1f)]
	public float time;

	public AnimationCurve lengthOverTime;
	//public AnimationCurve radiusOverTime;



	private List<Vector3> points;
	private List<int> triangles;
	private MeshFilter mf;

	void Start () {

		Initialize ();

	}


	public void Initialize()
	{
		tangentDirections = SplinePath.GenerateTrajectory (transform.position, initalDirection, nbOfSegments);

		normalDirections = new Vector3[tangentDirections.Length];

		for (int i = 0; i < tangentDirections.Length; i++) {
			normalDirections [i] = Vector3.Cross (tangentDirections [i], Vector3.left).normalized;
		}

		mf = GetComponent<MeshFilter> ();

		points = GeneratePoints ();

		triangles = GenerateTriangles (points);

		Mesh m = new Mesh ();

		m.vertices = points.ToArray ();

		m.triangles = triangles.ToArray ();

		m.RecalculateNormals ();

		mf.mesh = m;


	}


	public void GenerateMesh()
	{


	}


	public void UpdateMesh()
	{
		points.Clear ();
		points = GeneratePoints ();

		Mesh m = new Mesh ();

		m.vertices = points.ToArray ();
		m.triangles = triangles.ToArray ();

		m.RecalculateNormals ();

		mf.mesh = m;

	}




	public List<int> GenerateTriangles(List<Vector3> points)
	{

		List<int> tempTriangles = new List<int> ();


		for (int i = 0; i < nbOfSegments; i++) {

			for (int j = 0; j < nbOfSides; j++) 
			{


				int p0 = i * nbOfSides + j;
				int p1 = j + 1 == nbOfSides ? i * nbOfSides : p0 + 1;

				int p2 = p0 + nbOfSides;
				int p3 = p1 + nbOfSides;



				tempTriangles.Add (p0);
				tempTriangles.Add (p1);
				tempTriangles.Add (p2);

				tempTriangles.Add (p1);
				tempTriangles.Add (p3);
				tempTriangles.Add (p2);





			}



		}




		return tempTriangles;


	}



	public List<Vector3> GeneratePoints()
	{
		//CONSTANTS

		Vector3 randomVector = new Vector3 (0.132354f, 1.98654f, -1.5646f).normalized;
		float angleCoefficient = Mathf.PI * 2f / nbOfSides;

		//INITIALIZATIONS

		List<Vector3> tempPoints = new List<Vector3> ();
		Vector3 currentPosition = Vector3.zero;


		Vector3 v = randomVector;

		for (int i = 0; i < nbOfSegments + 1; i++) {




			//RATIOS
			float length = (float)i / (float)nbOfSegments;

			float lengthMultiplier = 1f;



			//RADIUS OVER TIME
			float radius;

			if (lengthOverTime.Evaluate (time) < length) {
				radius = 0f;
				lengthMultiplier = 0f;
			} else {
				//RADIUS OVER LENGTH
				radius = radiusOverLength.Evaluate( lengthOverTime.Evaluate (time) - length ) * initialRadius;
			}

			Vector3 positionOffset = normalDirections [i] * radius;




			//BASE
			Vector3 u = Vector3.Cross (v, tangentDirections[i]).normalized;
			v = Vector3.Cross (tangentDirections[i], u).normalized;


			Debug.DrawRay (currentPosition + transform.position, positionOffset, Color.blue, 10f);

			for (int j = 0; j < nbOfSides; j++) {

				float angle = j * angleCoefficient;

				Vector3 point = (positionOffset + currentPosition)
					+ (u * Mathf.Cos (angle) + v * Mathf.Sin (angle)) * radius;

				tempPoints.Add (point);


			}

			//SEGMENT LENGTH OVER LENGTH

			float relativeLength = (segmentLengthOverLength.Evaluate (length)) * initialSegmentLength * lengthMultiplier;

			Debug.DrawRay (currentPosition + transform.position, tangentDirections [i] * relativeLength, Color.red, 10f);

			currentPosition += tangentDirections[i] * relativeLength ;



		}

		return tempPoints;
	}


}
