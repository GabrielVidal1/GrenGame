using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour {

	[Header("Global Characteristics")]

	public bool smooth;

	[Range(0f, 1f)]
	public float smoothCoef;

	public bool noise;
	[Range(0f, 1f)]
	public float noiseCoef;

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


	//COLLISION CURVE START

	public float gravityForce;

	public AnimationCurve gravityOverLength;

	public float initialSegmentLength;
	public float finalSegmentLength;


	public AnimationCurve segmentLengthOverLength;


	public float noiseForce;

	[Range(0.01f, 100f)]
	public float noiseSize;

	public AnimationCurve noiseOverLength;
	//COLLISION CURVE END







	private List<Vector3> points;
	private List<int> triangles;
	private MeshFilter mf;






	public void Initialize()
	{
		positionsAndNormals = GenerateTrajectory (transform.position, initalDirection, nbOfSegments + 1);

		mf = GetComponent<MeshFilter> ();

		points = GeneratePoints ();

		triangles = GenerateTriangles (points);

		Mesh m = new Mesh ();

		m.vertices = points.ToArray ();

		m.triangles = triangles.ToArray ();

		m.RecalculateNormals ();

		mf.mesh = m;


	}



	public void UpdateMesh()
	{
		//UPDATE POINTS
		points.Clear ();
		points = GeneratePoints ();

		//MODIFY MESH
		Mesh m = mf.mesh;
		m.vertices = points.ToArray ();
		m.triangles = triangles.ToArray ();

		m.RecalculateNormals ();

	}



	//CALLED ONCE AT THE BEGINNING
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

	//CALLED EVERY TIME THE VARIABLE time IS CHANGED
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

			//DIRECTION DETERMINATION
			if (i == 0)
				direction = initalDirection;
			else if (i < nbOfSegments)
				direction = (positionsAndNormals.pos [i + 1] - transform.position - currentPosition).normalized;
			else
				direction = ( - positionsAndNormals.pos [i - 1] + transform.position - currentPosition).normalized;


			//RATIOS
			float length = (float)i / (float)nbOfSegments;




			//RADIUS OVER TIME
			float lengthMultiplier = 1f;
			float radius;

			if (lengthOverTime.Evaluate (time) < length) {
				radius = 0f;
				lengthMultiplier = 0f;
			} else {

				float ratio = 1f -(lengthOverTime.Evaluate (time) - length);

				//SHAPE DETERMINATION
				float shapeInit = initialShapeOverLength.Evaluate (ratio);
				float shapeFinal = finalShapeOverLength.Evaluate (ratio);
				float shape = Mathf.Lerp (shapeInit, shapeFinal, shapeOverTime.Evaluate (time));




				radius = radiusOverTime.Evaluate(time) * initialRadius * shape;
			}

			//CENTER OFFSET ALONG NORMALS
			Vector3 positionOffset = positionsAndNormals.nor [i] * radius;



			//BASE (U, V)
			Vector3 u = Vector3.Cross (v, direction).normalized;
			v = Vector3.Cross (direction, u).normalized;

			//SECTION CONSTRUCTION
			for (int j = 0; j < nbOfSides; j++) {

				float angle = j * angleCoefficient;

				Vector3 point = (positionOffset + currentPosition)
					+ (u * Mathf.Cos (angle) + v * Mathf.Sin (angle)) * radius;

				tempPoints.Add (point);
			}
		}

		//SMOOTHING
		if (smooth) {
			tempPoints = Smooth (tempPoints, smoothCoef);
			tempPoints = Smooth (tempPoints, smoothCoef);
		}

		//NOISING
		if (noise) {
			tempPoints = Noise (tempPoints, noiseCoef);
		}


		return tempPoints;
	}

	//USED TO SMOOTH THE POINTS OF THE 3D MESH
	private List<Vector3> Smooth(List<Vector3> tempPoints, float coef)
	{
		int np = tempPoints.Count;
		List<Vector3> nTempPoints = new List<Vector3>();

		for (int i = 0; i < np; i++) {

			if (i < nbOfSides || i > np - nbOfSides - 1) {
				//NO SMOOTH FOR FIRST AND LAST POINTS
				nTempPoints.Add(tempPoints [i]);
			} else {
				nTempPoints.Add (((tempPoints [i - nbOfSides] + tempPoints [i + nbOfSides]) * 0.5f * coef) + (1f - coef) * tempPoints [i]);
			}
		}
		return nTempPoints;
	}

	//USED TO NOISE THE POINTS OF THE 3D MESH
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

	//USED TO GENERATE THE TRAJECTORY AT THE CREATION OF THE PLANT
	public PositionsAndNormals GenerateTrajectory(Vector3 initialPosition, Vector3 initialDirection, int arrayLength)
	{

		//PARAMETERS

		initialDirection.Normalize ();

		PositionsAndNormals positionsNormals = new PositionsAndNormals (arrayLength);

		Vector3 currentDirection = initialDirection;
		Vector3 currentPosition = initialPosition;
		Vector3 currentNormal = Vector3.Cross(initialDirection, new Vector3(0.21f, 0.656f, 0.254f));

		positionsNormals.pos [0] = initialPosition;
		positionsNormals.nor [0] = currentNormal;


		for (int i = 1; i < arrayLength; i++) {

			//RATIO
			float lengthRatio = i / (float)arrayLength;


			//DIRECTION FORCES CALCULATION
			Vector3 gravity = Vector3.down * gravityForce * gravityOverLength.Evaluate (lengthRatio);
			Vector3 noise = noiseForce * noiseOverLength.Evaluate (lengthRatio) * Noise3D.PerlinNoise3D (currentPosition / noiseSize);
			float distance = Mathf.Lerp (initialSegmentLength, finalSegmentLength, segmentLengthOverLength.Evaluate (lengthRatio));

			currentDirection = distance * (currentDirection + gravity + noise).normalized;



			//COLLISION CALCULATION WITH 10 ITERATIONS
			PosDir pd = Collision(new PosDir(currentPosition, currentDirection, currentNormal), distance);
			for (int k = 1; k < 10; k++) 
				pd = Collision(new PosDir(currentPosition, pd.dir, pd.normal), distance);



			currentPosition = pd.pos;
			currentDirection = pd.dir;

			positionsNormals.pos [i] = currentPosition;
			positionsNormals.nor [i] = pd.normal;
		}

		return positionsNormals;

	}

	//USED FOR CALCULATE NEW DIRECTION AFTER COLLISION
	PosDir Collision(PosDir pd, float distance)
	{

		Ray ray = new Ray (pd.pos, pd.dir);
		RaycastHit hit;

		if (Physics.Raycast (ray, out hit, distance)) {

			//NORMAL SMOOTHING
			pd.normal = 0.25f * hit.normal + 0.75f * pd.normal;

			//DIRECTION PROJECTION OVER COLLIDER - DIRECTION AJUSTEMENT
			float d = Vector3.Dot (hit.normal.normalized, hit.point - pd.pos);
			Vector3 H = pd.pos + d * hit.normal;
			pd.dir = (hit.point - H).normalized * distance;

			float dist = Vector3.Distance (pd.pos, hit.point);
			float dd = Mathf.Sqrt (distance * distance - d * d);
			pd.pos = (hit.point - H).normalized * dd + H + 0.05f * hit.normal;

		} else {
			//IF NO COLISION
			pd.pos += pd.dir;
		}
		return pd;
	}


}

/*
//STRUCUTRE FOR (DIRECTION, POSITION, NORMAL)
struct PosDir
{
	public Vector3 pos;
	public Vector3 dir;
	public Vector3 normal;

	public PosDir(Vector3 pos, Vector3 dir, Vector3 normal)
	{
		this.pos = pos;
		this.dir = dir;
		this.normal = normal;
	}
}

//STRUCTURE FOR (POSITIONS[], NORMALS[])
public struct PositionsAndNormals
{
	public Vector3[] pos;
	public Vector3[] nor;

	public PositionsAndNormals(int arrayLength)
	{
		pos = new Vector3[arrayLength];
		nor = new Vector3[arrayLength];
	}
}
*/