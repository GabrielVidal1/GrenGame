using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantGeneration : MonoBehaviour {

	[Header("Global Characteristics")]

	public bool smooth;

	[Range(0f, 1f)]
	public float smoothCoef;

	public bool noise;
	[Range(0f, 1f)]
	public float noiseCoef;


	public bool generateFromCollision;

	public float initialRadius;



	public Vector3 initalDirection;

	public int nbOfSides;
	public int nbOfSegments;

	[Header("Trajectory")]

	public PositionsAndNormals positionsAndNormals;



	[Header("Variation Over Lentgh")] 


	public AnimationCurve initialShapeOverLength;
	public AnimationCurve finalShapeOverLength;

	public AnimationCurve shapeOverTime;

	public AnimationCurve radiusOverTime;

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
		positionsAndNormals = GetComponent<CollisionCurve>().GenerateTrajectory (transform.position, initalDirection, nbOfSegments + 1);

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



		Vector3 v = randomVector;

		Vector3 direction;

		for (int i = 0; i < nbOfSegments + 1; i++) {

			Vector3 currentPosition = positionsAndNormals.pos [i] - transform.position;

			if (i == 0)
				direction = initalDirection;
			else if (i < nbOfSegments)
				direction = (positionsAndNormals.pos [i + 1] - transform.position - currentPosition).normalized;
			else
				direction = ( - positionsAndNormals.pos [i - 1] + transform.position - currentPosition).normalized;


			//RATIOS
			float length = (float)i / (float)nbOfSegments;

			float lengthMultiplier = 1f;



			//RADIUS OVER TIME
			float radius;

			if (lengthOverTime.Evaluate (time) < length) {
				radius = 0f;
				lengthMultiplier = 0f;
			} else {

				float ratio = 1f -(lengthOverTime.Evaluate (time) - length);

				float shapeInit = initialShapeOverLength.Evaluate (ratio);
				float shapeFinal = finalShapeOverLength.Evaluate (ratio);

				float shape = Mathf.Lerp (shapeInit, shapeFinal, shapeOverTime.Evaluate (time));


				radius = radiusOverTime.Evaluate(time) * initialRadius * shape;
			}

			Vector3 positionOffset = positionsAndNormals.nor [i] * radius;



			//BASE
			Vector3 u = Vector3.Cross (v, direction).normalized;
			v = Vector3.Cross (direction, u).normalized;


			for (int j = 0; j < nbOfSides; j++) {

				float angle = j * angleCoefficient;

				Vector3 point = (positionOffset + currentPosition)
					+ (u * Mathf.Cos (angle) + v * Mathf.Sin (angle)) * radius;

				tempPoints.Add (point);
			}
		}

		if (smooth) {

			tempPoints = Smooth (tempPoints, smoothCoef);
			tempPoints = Smooth (tempPoints, smoothCoef);
		}

		if (noise) {

			tempPoints = Noise (tempPoints, noiseCoef);
		}


		return tempPoints;
		
	}


	private List<Vector3> Smooth(List<Vector3> tempPoints, float coef)
	{

		int np = tempPoints.Count;
		
		List<Vector3> nTempPoints = new List<Vector3>();
		
		for (int i = 0; i < np; i++) {
			
			//print ("np : " + np.ToString () + " / " + (i - nbOfSides).ToString () + "   " + i.ToString () + "   " + (i + nbOfSides).ToString ());
			
			if (i < nbOfSides || i > np - nbOfSides - 1) {
				
				//nTempPoints [i] = tempPoints [i];
				nTempPoints.Add(tempPoints [i]);
				
			} else {
				nTempPoints.Add (((tempPoints [i - nbOfSides] + tempPoints [i + nbOfSides]) * 0.5f * coef) + (1f - coef) * tempPoints [i]);
				//nTempPoints [i] = (tempPoints [i - nbOfSides] + tempPoints [i + nbOfSides]) * 0.5f;
			}
		}
		return nTempPoints;
	}

	private List<Vector3> Noise(List<Vector3> tempPoints, float coef)
	{


		for (int i = 0; i < tempPoints.Count; i++) {

			float ratio = i / (float)tempPoints.Count;

			float radius = radiusOverTime.Evaluate ( 1f - ratio) * initialRadius;

			if (lengthOverTime.Evaluate (time) > ratio) 
			{
				Vector3 noise = Noise3D.PerlinNoise3D (5f * tempPoints [i] / radius);

				tempPoints [i] += noise.normalized * coef * radius;
			}
		}
		return tempPoints;
	}

}
