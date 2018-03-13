using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class Plant : MonoBehaviour {

	public float time;

	public bool smooth;
	public float smoothCoef;

	public bool noise;
	public float noiseCoef;

	public int nbOfSides;
	public int nbOfSegments;

	public float initialRadius;
	public Vector3 initalDirection;

	public int sectionsPerUvUnit;

	public AnimationCurve initialShapeOverLength;
	public AnimationCurve finalShapeOverLength;
	public AnimationCurve shapeOverTime;


	public float initialSegmentLength;
	public float finalSegmentLength;
	public AnimationCurve lengthOverTime;
	public AnimationCurve segmentLengthOverLength;


	public bool hasGravity;
	public float gravityForce;
	public AnimationCurve gravityOverLength;


	public bool hasNoise;
	public float noiseForce;
	public float noiseSize;
	public AnimationCurve noiseOverLength;


	public bool hasLeaves;
	public Leaf leafPrefab;
	public float leafWidth;
	public float leafLength;

	public bool leavesOnlyOnSections;

	public int leavesPerSegment;
	public AnimationCurve leavesDistribution;
	public float leafGrowthDurationRatio;
	public float sunOrientationIntensity;
	public AnimationCurve sunOrientationIntensityOverTime;
	public AnimationCurve leafGrowthOverTime;
	


	public bool hasRecursions;



	#region PrivateVariables

	private PositionsAndNormals positionsAndNormals;
	private LeafRatioLength[] leaves;
	private int actualLeavesNumber;
	private Transform leavesParent;


	private List<Vector3> points;
	private List<int> triangles;
	private List<Vector2> uvs;
	private MeshFilter mf;

	private AnimationCurve trunkTimeOverTime;

	#endregion



	public void Initialize()
	{
		trunkTimeOverTime = AnimationCurve.Linear (0f, 0f, 1f - leafGrowthDurationRatio, 1f);


		positionsAndNormals = GenerateTrajectory (transform.position, initalDirection, nbOfSegments + 1);

		mf = GetComponent<MeshFilter> ();

		points = GeneratePoints ();

		triangles = GenerateTriangles (points);
		uvs = GenerateUVs ();
		Mesh m = new Mesh ();

		m.vertices = points.ToArray ();

		m.triangles = triangles.ToArray ();
		m.uv = uvs.ToArray ();

		m.RecalculateNormals ();

		mf.mesh = m;

		actualLeavesNumber = 0;

		if (hasLeaves)
			GenerateLeaves ();
		
		UpdateMesh ();

	}

	public void UpdateMesh()
	{
		//UPDATE POINTS
		points.Clear ();
		points = GeneratePoints ();

		//MODIFY MESH
		Mesh m = mf.sharedMesh;
		m.vertices = points.ToArray ();
		m.triangles = triangles.ToArray ();

		m.RecalculateNormals ();

		if (hasLeaves)
			UpdateLeaves ();
	}

	public void GenerateRecursions()
	{



	}



	//USED TO FIRST GENERATE THE LEAVES ON THE PLANT
	public void GenerateLeaves()
	{ 
		Vector3 randomVector = new Vector3 (0.132354f, 1.98654f, -1.5646f).normalized;

		//CREATE THE PARENT OF ALL LEAVES
		if (leavesParent != null)
			DestroyImmediate (leavesParent.gameObject);

		leavesParent = new GameObject ("LeaveParent").transform;
		leavesParent.parent = transform;
		leavesParent.transform.position = transform.position;


		//DESTROY PRE-EXISTING CHILDREN OF THE LEAVES PARENT
		for (int k = 0; k < leavesParent.childCount; k++) {
			if (Application.isEditor)
				DestroyImmediate(leavesParent.GetChild(k).gameObject);
			else
				Destroy(leavesParent.GetChild(k).gameObject);
		}


		leaves = new LeafRatioLength[leavesPerSegment * nbOfSegments];


		actualLeavesNumber = 0;
		int iteration = 0;

		//SUR SAMPLE OF MAX 10 * NB OF LEAVES ITERATIONS
		while ( iteration < leavesPerSegment * nbOfSegments) {


			float lengthRatio = Random.value;
			float proba = leavesDistribution.Evaluate (lengthRatio);

			if (Random.value < proba) {


				//POSITION
				int segment = (int)(lengthRatio * nbOfSegments);

				float lerpValue = (lengthRatio - ( segment / (float)nbOfSegments)) * nbOfSegments;



				float value = Random.value;
				//POINT AND OPPOSED POINT
				int pointIndex1 = nbOfSides * segment + (int)(value * nbOfSides);
				int pointIndex2 = nbOfSides * segment + (int)((value + 0.5f > 1f ? value - 0.5f : value + 0.5f) * nbOfSides);


				// INTERPOLATION BETWEEN SEGMENTS


				Vector3 position = Vector3.zero;

				if (leavesOnlyOnSections)
					position = mf.sharedMesh.vertices [pointIndex1];
				else
					position = (1f-lerpValue) * mf.sharedMesh.vertices [pointIndex1] + lerpValue * mf.sharedMesh.vertices [pointIndex1 + nbOfSides];

				//INITIAL DIRECTION
				Vector3 direction = mf.sharedMesh.normals[pointIndex1] - mf.sharedMesh.normals[pointIndex2];

				//TANGEANT DIRECTION
				Vector3 tangentDirection = mf.sharedMesh.vertices [pointIndex1] - mf.sharedMesh.vertices [pointIndex1 + nbOfSides];


				//ORIENTATION TOWARD THE SUN
				Vector3 up = (1f - sunOrientationIntensity) * tangentDirection.normalized + sunOrientationIntensity * Vector3.up * sunOrientationIntensityOverTime.Evaluate(time);
				direction.y *= sunOrientationIntensity * sunOrientationIntensityOverTime.Evaluate(time);


				direction.Normalize ();


				//LEAF CREATION
				Leaf leaf = Instantiate (leafPrefab, position, Quaternion.identity).GetComponent<Leaf> ();
				leaf.transform.parent = leavesParent;
				leaf.name = "Leaf_" + actualLeavesNumber.ToString ();
				leaf.initialDirection = direction;
				leaf.upDirection = up;

				leaf.width = leafPrefab.width * leafWidth;
				leaf.length = leafPrefab.length * leafLength;

				leaf.time = 0f;


				float rndValue = Random.value;

				float birthDate = lengthRatio * (1f - rndValue) + rndValue *(1f - leafGrowthDurationRatio);



				LeafRatioLength l = new LeafRatioLength (leaf, lengthRatio, direction, pointIndex1, pointIndex2, lerpValue, birthDate, leafGrowthDurationRatio);
					
				leaves [actualLeavesNumber] = l;


				//INCREMENTS
				actualLeavesNumber++;
			}

			iteration++;

		}
	}

	//USED TO UPDATE THE LEAVES EVERY FRAME
	public void UpdateLeaves()
	{
		for (int i = 0; i < actualLeavesNumber; i++) {

			//DISTANCE FROM ROOT / LENGTH
			LeafRatioLength leaf = leaves [i];

			//NOT GROWN
			if (leaf.birthDate > time) {
				leaf.leaf.time = 0f;

			} else {

				if (time - leaf.birthDate < leafGrowthDurationRatio) {
					//float ratio = 1f - (lengthOverTime.Evaluate (time) - leaf.lengthRatio);
					//leaf.leaf.time = leafGrowthOverTime.Evaluate(1f - ratio);

					float localTime = leafGrowthOverTime.Evaluate ((time - leaf.birthDate) / leafGrowthDurationRatio);
					leaf.leaf.time = leafGrowthOverTime.Evaluate (localTime);

					//INTIAL DIRECTION
					Vector3 initialDirection = (mf.sharedMesh.vertices [leaf.pointIndex1] -
					                           mf.sharedMesh.vertices [leaf.pointIndex2]).normalized;
					initialDirection.y = initialDirection.y * (1f - sunOrientationIntensityOverTime.Evaluate (localTime)) + (1f - sunOrientationIntensity) * sunOrientationIntensityOverTime.Evaluate (localTime);

					//TANGENT DIRECTION
					Vector3 tangentDirection = mf.sharedMesh.vertices [leaf.pointIndex1] - mf.sharedMesh.vertices [leaf.pointIndex1 + nbOfSides];

					//UP DIRECTION
					Vector3 up = (1f - sunOrientationIntensity) * tangentDirection.normalized + sunOrientationIntensity * Vector3.up * sunOrientationIntensityOverTime.Evaluate(localTime);

					Vector3 position = transform.position +
						mf.sharedMesh.vertices [leaf.pointIndex1] * (1f - leaf.lerpValue) +
						mf.sharedMesh.vertices [leaf.pointIndex1 + nbOfSides] * leaf.lerpValue;


					leaf.leaf.initialDirection = initialDirection.normalized;
					leaf.leaf.transform.position = position;
					leaf.leaf.upDirection = up.normalized;


					leaf.leaf.UpdateMesh ();
				}
			}
		}
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


		float trunkTime = trunkTimeOverTime.Evaluate (time);


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

			if (lengthOverTime.Evaluate (trunkTime) < length) {
				radius = 0f;
				lengthMultiplier = 0f;
			} else {

				float ratio = 1f -(lengthOverTime.Evaluate (trunkTime) - length);

				//SHAPE DETERMINATION
				float shapeInit = initialShapeOverLength.Evaluate (ratio);
				float shapeFinal = finalShapeOverLength.Evaluate (ratio);
				float shape = Mathf.Lerp (shapeInit, shapeFinal, shapeOverTime.Evaluate (trunkTime));




				radius = initialRadius * shape;
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

			float v = shapeOverTime.Evaluate (0.5f);

			float radius = (initialShapeOverLength.Evaluate(ratio) * v + (1f - v) * finalShapeOverLength.Evaluate(ratio)) * initialRadius;

			if (lengthOverTime.Evaluate (time) > ratio) 
			{
				Vector3 noise = Noise3D.PerlinNoise3D (5f * tempPoints [i] / radius);
				tempPoints [i] += noise.normalized * coef * radius;
			}
		}
		return tempPoints;
	}

	//GENERATE THE UV MAP FOR THE MESH
	private List<Vector2> GenerateUVs()
	{
		List<Vector2> uv = new List<Vector2> ();

		for (int i = 0; i < nbOfSegments + 1; i++) {

			int iMod = i % (sectionsPerUvUnit + 1);

			float ratioY = iMod / (float)nbOfSegments;

			for (int j = 0; j < nbOfSides; j++) 
			{
				float ratioX = j / (float)nbOfSides;

				Vector2 p = new Vector2 (ratioX, ratioY);
				uv.Add (p);
			}
		}
		return uv;
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
			Vector3 gravity = Vector3.zero;
			if (hasGravity)
				gravity = Vector3.down * gravityForce * gravityOverLength.Evaluate (lengthRatio);

			Vector3 noise = Vector3.zero;
			if (hasNoise)
				noise = noiseForce * noiseOverLength.Evaluate (lengthRatio) * Noise3D.PerlinNoise3D (currentPosition / noiseSize);


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

//STRUCTURE FOR LEAVES AND THEIR RESPECTIVE RATIO
struct LeafRatioLength
{
	public Leaf leaf;
	public float lengthRatio;

	public Vector3 normalDirection;

	public int pointIndex1;
	public int pointIndex2;

	public float lerpValue;

	public float birthDate;
	public float growthDuration;

	public LeafRatioLength(Leaf leaf, float lengthRatio, Vector3 normalDirection, int pointIndex1, int pointIndex2, float lerpValue, float birthDate, float growthDuration )
	{
		this.leaf = leaf;
		this.lengthRatio = lengthRatio;
		this.pointIndex1 = pointIndex1;
		this.pointIndex2 = pointIndex2;
		this.lerpValue = lerpValue;

		this.birthDate = birthDate;
		this.growthDuration = growthDuration;

		this.normalDirection = normalDirection;

		leaf.Initialize ();
	}




}

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
