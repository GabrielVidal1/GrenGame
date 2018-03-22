using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class Plant : MonoBehaviour {

	#region STATIC VARIABLES

	public static int plantNumber = 0;

	#endregion

	#region PUBLIC VARIABLES

	public int plantSeed;


	[Range(0f, 1f)]
	public float time;
	public float maxDuration;

	public Interval trunkGrowthDuration;


	public bool smooth;
	public float smoothCoef;

	public bool noise;
	public float noiseCoef;

	public bool initialForce;
	public float initialForceCoef;

	public int nbOfSides;
	public int nbOfSegments;

	public float initialRadius;
	public Vector3 initialDirection;
	public Vector3 initialNormal;

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

	public Interval leafSize;

	public bool leavesOnlyOnSections;

	public float leavesDensity;
	public AnimationCurve leavesDistribution;
	public AnimationCurve leavesBirthdateDistribution;
	public Interval leafGrowthDuration;



	public float sunOrientationIntensity;
	public AnimationCurve sunOrientationIntensityOverTime;
	public AnimationCurve leafGrowthOverTime;


	//RECURSIONS

	public bool hasRecursions;
	public Plant branchPrefab;

	public float brancheAngleDelta;

	public int nbOfBranches;
	public AnimationCurve branchesDistribution;
	public AnimationCurve branchBirthDateDistribution;

	public Interval branchGrowthDuration;
	public Interval branchInitialRadiusMultiplier;



	public AnimationCurve branchesTangencityOverLength;

	public bool branchesOnlyOnSections;

	public AnimationCurve branchLengthOverTrunkLength;

	public Interval brancheLengthRatio;

	public bool branchForceParameters;

	[HideInInspector]
	public bool isBranch;

	[HideInInspector]
	public int indexInGameData;

	#endregion

	#region PrivateVariables

	private PositionsAndNormals positionsAndNormals;

	private LeafInfo[] leaves;
	private int actualLeavesNumber;
	private Transform leavesParent;

	private BranchesInfo[] branches;
	private int actualBranchesNumber;
	private Transform branchesParent;


	private List<Vector3> points;
	private List<int> triangles;
	private List<Vector2> uvs;
	private MeshFilter mf;

	private float totalLength;

	private float actualTrunkGrowthDuration;
	#endregion
	[SerializeField]
	private AnimationCurve trunkTimeOverTime;



	private bool hasSetSeed = false;

	public void SetSeed(int seed)
	{
		plantSeed = seed;
		plantNumber++;
		Random.InitState (1234 * seed);
		hasSetSeed = true;
	}

	public void InitializePlant()
	{
		//RANDOM INITIALISATION
		
		if (!hasSetSeed) {
			SetSeed (plantNumber);
		}

		actualTrunkGrowthDuration = trunkGrowthDuration.RandomValue();
			
		maxDuration = actualTrunkGrowthDuration;
		if (hasLeaves)
			maxDuration += leafGrowthDuration.max;
		if (hasRecursions) {
			maxDuration += branchGrowthDuration.max;
			if (branchPrefab.hasLeaves)
				maxDuration += branchPrefab.leafGrowthDuration.max;
			if (branchPrefab.hasRecursions)
				maxDuration += branchPrefab.branchGrowthDuration.max;
		}



		InitializeMesh ();

		if (hasLeaves)
			GenerateLeaves ();

		if (hasRecursions)
			GenerateRecursions ();


		UpdatePlant ();
	}

	public void UpdatePlant()
	{
		UpdateMesh ();
		
		if (hasLeaves)
			UpdateLeaves ();
		
		if (hasRecursions)
			UpdateRecursions ();
	}

	public void InitializeMesh()
	{
		trunkTimeOverTime = new AnimationCurve (new Keyframe[] {
			new Keyframe (0, 0),
			new Keyframe (actualTrunkGrowthDuration / maxDuration, 1f),
			new Keyframe (1, 1)
		});
		positionsAndNormals = GenerateTrajectory (transform.position, initialDirection, nbOfSegments + 1);

		mf = GetComponent<MeshFilter> ();


		triangles = GenerateTriangles (points);
		uvs = GenerateUVs ();
		points = GeneratePoints (0f);

		Mesh m = new Mesh ();


		m.vertices = points.ToArray ();
		m.triangles = triangles.ToArray ();
		m.uv = uvs.ToArray ();

		m.RecalculateNormals ();

		mf.mesh = m;

	}

	public void UpdateMesh()
	{
		//UPDATE POINTS
		points.Clear ();
		points = GeneratePoints (time);

		//MODIFY MESH
		Mesh m = new Mesh();
		m.vertices = points.ToArray ();
		m.triangles = triangles.ToArray ();
		m.uv = uvs.ToArray ();


		m.RecalculateNormals ();

		mf.mesh = m;

	}


	//USED TO FIRST GENERATE THE BRANCHES ON THE PLANT
	public void GenerateRecursions()
	{
		actualBranchesNumber = 0;

		Vector3 randomVector = new Vector3 (0.132354f, 1.98654f, -1.5646f).normalized;

		//CREATE THE PARENT OF ALL LEAVES
		if (branchesParent != null)
			DestroyImmediate (branchesParent.gameObject);

		branchesParent = new GameObject ("branchesParent").transform;
		branchesParent.parent = transform;
		branchesParent.transform.localPosition = Vector3.zero;

		branches = new BranchesInfo[nbOfBranches];

		//FINISHED POINTS OF THE MODEL
		Vector3[] points = GeneratePoints (1f).ToArray ();

		float angle = Random.value * 2 * Mathf.PI;


		for(int i = 0 ; i < nbOfBranches ; i++){

			float lengthRatio = (float)i / (float)nbOfBranches;
			float proba = branchesDistribution.Evaluate (lengthRatio);


			if (Random.value < proba) {


				//POSITION
				int segment = (int)(lengthRatio * nbOfSegments);

				float lerpValue = (lengthRatio - ( segment / (float)nbOfSegments)) * nbOfSegments;


				//float value = Random.value;
				float value = 0f;
				//POINT AND OPPOSED POINT
				int pointIndex1 = nbOfSides * segment + (int)(value * nbOfSides);
				int pointIndex2 = nbOfSides * segment + (int)((value + 0.5f > 1f ? value - 0.5f : value + 0.5f) * nbOfSides);



				// INTERPOLATION BETWEEN SEGMENTS


				Vector3 position = Vector3.zero;
				if (branchesOnlyOnSections) {
					position = positionsAndNormals.pos [segment];
				} else {
					position = Vector3.Lerp (positionsAndNormals.pos [segment], positionsAndNormals.pos [segment + 1], lerpValue);
				}


				float actualTrunkRadius = finalShapeOverLength.Evaluate(lengthRatio) * initialRadius;

				//INITIAL RADIUS
				float initialBrancheRadiusBranch = actualTrunkRadius * 0.8f;

				
				Vector3 normal = positionsAndNormals.nor [segment];

				position += normal * actualTrunkRadius;

				Vector3 tangentDirection = (points [pointIndex1+ nbOfSides] - points [pointIndex1 ]).normalized;
				//INITIAL DIRECTION
				Vector3 u = (points[pointIndex1] - points[pointIndex2]).normalized;
				Vector3 v = Vector3.Cross (tangentDirection, u).normalized;

				angle += brancheAngleDelta;
				Vector3 direction = u * Mathf.Cos(angle) + v * Mathf.Sin(angle);

				//????????????????
				//TANGEANT DIRECTION
				//Debug.DrawRay (position, tangentDirection, Color.red, 10f);


				//ORIENTATION TOWARD THE SUN ?????????????????
				direction = Vector3.Lerp(direction, tangentDirection, branchesTangencityOverLength.Evaluate(lengthRatio));

				direction.Normalize ();

				float growthDuration = branchGrowthDuration.RandomValue ();

				float birthDate = Mathf.Lerp ( maxDuration - growthDuration, ValueAt(trunkTimeOverTime, lengthRatio) * maxDuration, branchBirthDateDistribution.Evaluate(Random.value));


				//BRANCHE CREATION
				Plant branche = Instantiate (branchPrefab, position, Quaternion.identity).GetComponent<Plant> ();


				branche.transform.parent = branchesParent;
				branche.transform.position = position;
				branche.name = "Branche_" + actualBranchesNumber.ToString ();

				//PARAMETERS SETTINGS

				branche.trunkGrowthDuration = branchGrowthDuration;

				branche.leafGrowthDuration = leafGrowthDuration;

				branche.isBranch = true;
				branche.initialDirection = direction;
				branche.initialRadius = initialBrancheRadiusBranch * branchInitialRadiusMultiplier.RandomValue();
				branche.initialNormal = tangentDirection;

				branche.initialSegmentLength = initialSegmentLength;
				branche.finalSegmentLength = finalSegmentLength;

				//branche.nbOfSides = Mathf.Max (3, nbOfSides - 1);

				float nbOfS = branchLengthOverTrunkLength.Evaluate (lengthRatio) * nbOfSegments * brancheLengthRatio.RandomValue() + 1;

				branche.nbOfSegments = (int)nbOfS;


				//BRANCH CREATION




				branche.time = 0f;
				branche.InitializePlant ();

				BranchesInfo b = new BranchesInfo (branche, birthDate, segment, position, lengthRatio, growthDuration);

				branches [actualBranchesNumber] = b;


				//INCREMENTS
				actualBranchesNumber++;
			}
		}

	}

	public float ValueAt( AnimationCurve f, float solution, float precision = 0.1f)
	{
		float a = 0f;
		float b = 1f;
		float c = 0.5f;

		while (Mathf.Abs (b - a) > precision) {
			c = (a + b) / 2f;
			if (f.Evaluate (c) >= solution)
				b = c;
			else if (f.Evaluate (c) < solution)
				a = c;
				
		}
		return c;
	}

	//USED TO UPDATE THE BRANCHES EVERY FRAME
	public void UpdateRecursions()
	{
		float absolutTime = time * maxDuration;

		for (int i = 0; i < actualBranchesNumber; i++) {

			//DISTANCE FROM ROOT / LENGTH
			BranchesInfo branch = branches [i];


			//NOT GROWN
			if (branch.birthDate > absolutTime) {
				branch.branch.time = 0f;

			} else {

				if (absolutTime - branch.birthDate < branch.growthDuration || !branch.grownOnce) {
					
					branch.branch.time = Mathf.Min ((absolutTime - branch.birthDate) / branch.growthDuration, 1f);

					float radius = Mathf.Lerp (
						               initialShapeOverLength.Evaluate (branch.lengthRatio),
						               finalShapeOverLength.Evaluate (branch.lengthRatio),
						               shapeOverTime.Evaluate (time));

					//Debug.DrawRay (branch.finalPosition, -positionsAndNormals.nor [branch.normalIndex] * radius, Color.red, 10f);

					branch.branch.transform.position = branch.finalPosition - positionsAndNormals.nor [branch.normalIndex] * radius;
					branch.grownOnce = true;
					branch.branch.UpdatePlant ();
				}

			}
		}

	}

	//USED TO FIRST GENERATE THE LEAVES ON THE PLANT
	public void GenerateLeaves()
	{ 
		actualLeavesNumber = 0;

		Vector3 randomVector = new Vector3 (0.132354f, 1.98654f, -1.5646f).normalized;

		//CREATE THE PARENT OF ALL LEAVES
		if (leavesParent != null)
			DestroyImmediate (leavesParent.gameObject);

		leavesParent = new GameObject ("LeaveParent").transform;
		leavesParent.parent = transform;
		leavesParent.transform.localPosition = Vector3.zero;

		int leavesIteration = (int)(leavesDensity * totalLength);

		leaves = new LeafInfo[leavesIteration];


		int iteration = 0;

		while ( iteration < leavesIteration) {


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

				if (branchesOnlyOnSections)
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


				leaf.transform.localScale *= leafSize.RandomValue();

				leaf.time = 0f;


				float growthDuration = leafGrowthDuration.RandomValue ();

				float birthDate = Mathf.Lerp (ValueAt (trunkTimeOverTime, lengthRatio) * maxDuration, maxDuration - growthDuration, leavesBirthdateDistribution.Evaluate (Random.value));


				LeafInfo l = new LeafInfo (leaf, lengthRatio, direction, pointIndex1, pointIndex2, lerpValue, birthDate, growthDuration);
					
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
		float absolutTime = time * maxDuration;

		for (int i = 0; i < actualLeavesNumber; i++) {

			//DISTANCE FROM ROOT / LENGTH
			LeafInfo leaf = leaves [i];


			if (leaf.birthDate > absolutTime) {
				//NOT GROWN
				leaf.leaf.time = 0f;
			} else {

				if (absolutTime - leaf.birthDate < leaf.growthDuration || !leaf.grownOnce) {
					float localTime = Mathf.Min(1f, leafGrowthOverTime.Evaluate ((absolutTime - leaf.birthDate) / leaf.growthDuration));

					leaf.leaf.time = leafGrowthOverTime.Evaluate (localTime);


					//INTIAL DIRECTION
					Vector3 initialDirection = (mf.sharedMesh.vertices [leaf.pointIndex1] - mf.sharedMesh.vertices [leaf.pointIndex2]).normalized;
					initialDirection.y = Mathf.Lerp (initialDirection.y, 1f - sunOrientationIntensity, sunOrientationIntensityOverTime.Evaluate (localTime));

					//TANGENT DIRECTION
					Vector3 tangentDirection = mf.sharedMesh.vertices [leaf.pointIndex1] - mf.sharedMesh.vertices [leaf.pointIndex1 + nbOfSides];

					//UP DIRECTION
					Vector3 up = Vector3.Lerp(tangentDirection.normalized ,Vector3.up * sunOrientationIntensityOverTime.Evaluate (localTime), sunOrientationIntensity);

					Vector3 localPosition = Vector3.Lerp(mf.sharedMesh.vertices [leaf.pointIndex1], mf.sharedMesh.vertices [leaf.pointIndex1 + nbOfSides] , leaf.lerpValue);


					leaf.leaf.initialDirection = initialDirection.normalized;
					leaf.leaf.transform.localPosition = localPosition;
					leaf.leaf.upDirection = up.normalized;

					leaf.grownOnce = true;
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
	public List<Vector3> GeneratePoints(float time)
	{
		//CONSTANTS

		float trunkTime = trunkTimeOverTime.Evaluate(time);

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
				direction = initialDirection;
			else if (i < nbOfSegments)
				direction = (positionsAndNormals.pos [i + 1] - transform.position - currentPosition).normalized;
			else
				direction = ( - positionsAndNormals.pos [i - 1] + transform.position - currentPosition).normalized;


			//RATIOS
			float length = (float)i / (float)nbOfSegments;

			float length1 = (float)(i+1) / (float)nbOfSegments;



			//RADIUS OVER TIME
			float lengthMultiplier = 1f;
			float radius;

			if (lengthOverTime.Evaluate (trunkTime) < length) {
				radius = 0f;
				lengthMultiplier = 0f;
			} else if (lengthOverTime.Evaluate (trunkTime) < length1) {

				radius = 0f;
				lengthMultiplier = 0f;

			} else {

				float ratio = 1f - (lengthOverTime.Evaluate (trunkTime) - length);

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

				//Debug.DrawRay (transform.position + 3f * new Vector3 (p.x, 0f, p.y), Vector3.up, Color.red, 10f);


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
		Vector3 currentNormal = initialNormal;

		positionsNormals.pos [0] = initialPosition;
		positionsNormals.nor [0] = currentNormal;

		float computedLength = nbOfSegments * (initialSegmentLength + finalSegmentLength) / 2f;
		float actualNoiseSize = noiseSize * computedLength;

		Vector3 noiseOffset = Vector3.zero;
		float noiseMultiplier = 1f;

		for (int i = 1; i < arrayLength; i++) {



			//RATIO
			float lengthRatio = i / (float)arrayLength;



			//DIRECTION FORCES CALCULATION
			Vector3 gravity = Vector3.zero;
			if (hasGravity)
				gravity = Vector3.down * gravityForce * gravityOverLength.Evaluate (lengthRatio);

			Vector3 noise = Vector3.zero;
			if (hasNoise) {

				if (i % 25 == 0) {
					noiseOffset.y += 3 * actualNoiseSize;
					noiseMultiplier = 5f;
				}
				noise = noiseForce * noiseOverLength.Evaluate (lengthRatio) * Noise3D.PerlinNoise3D ((currentPosition + noiseOffset)/ actualNoiseSize);
			
			
			}

			Vector3 force = currentDirection;
			if (initialForce)
				force = initialForceCoef * initialDirection;

			float distance = Mathf.Lerp (initialSegmentLength, finalSegmentLength, segmentLengthOverLength.Evaluate (lengthRatio));
			totalLength += distance;

			currentDirection = distance * (force + gravity + noiseMultiplier * noise).normalized;
			//currentDirection = distance * (force + currentDirection + gravity + noiseMultiplier * noise).normalized;



			//COLLISION CALCULATION WITH 10 ITERATIONS
			PosDir pd = Collision(new PosDir(currentPosition, currentDirection, currentNormal), distance);
			for (int k = 1; k < 10; k++) 
				pd = Collision(new PosDir(currentPosition, pd.dir, pd.normal), distance);



			currentPosition = pd.pos;
			currentDirection = pd.dir;

			noiseMultiplier = 1f;

			//Debug.DrawRay (currentPosition, currentDirection, Color.green, 10f);

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

struct BranchesInfo
{
	public Plant branch;
	public float birthDate;

	public int normalIndex;
	public Vector3 finalPosition;
	public float lengthRatio;
	public float growthDuration;

	public bool grownOnce;


	public BranchesInfo( Plant branch, float birthDate, int normalIndex, Vector3 finalPosition, float lengthRatio, float growthDuration)
	{
		this.branch = branch;
		this.birthDate = birthDate;
		this.normalIndex = normalIndex;
		this.finalPosition = finalPosition;
		this.lengthRatio = lengthRatio;
		this.growthDuration = growthDuration;

		grownOnce = false;

	}
}


//STRUCTURE FOR LEAVES AND INFORMATION ABOUT IT
struct LeafInfo
{
	public Leaf leaf;
	public float lengthRatio;

	public Vector3 normalDirection;

	public int pointIndex1;
	public int pointIndex2;

	public float lerpValue;

	public float birthDate;
	public float growthDuration;

	public bool grownOnce;

	public LeafInfo(Leaf leaf, float lengthRatio, Vector3 normalDirection, int pointIndex1, int pointIndex2, float lerpValue, float birthDate, float growthDuration )
	{
		this.leaf = leaf;
		this.lengthRatio = lengthRatio;
		this.pointIndex1 = pointIndex1;
		this.pointIndex2 = pointIndex2;
		this.lerpValue = lerpValue;

		this.birthDate = birthDate;
		this.growthDuration = growthDuration;

		this.normalDirection = normalDirection;

		grownOnce = false;

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
