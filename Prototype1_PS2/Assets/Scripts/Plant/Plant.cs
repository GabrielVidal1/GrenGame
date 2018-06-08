using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LeavesRepartitionMode
{Density, Number}

[RequireComponent(typeof(MeshFilter))]
[System.Serializable]
public class Plant : MonoBehaviour{

	#region STATIC VARIABLES

	public static int plantNumber = 0;

	#endregion


	#region EDITOR VARIABLES

	public float plantLength;
	public float PlantLenght
	{
		get {return plantLength;}
		set{ 
			plantLength = value;
			finalSegmentLength = plantLength / (float)nbOfSegments;
			segmentLengthOverLength = AnimationCurve.Linear (0f, 1f, 1f, 1f);
		}
	}




	#endregion

	#region PUBLIC VARIABLES

	public string crossingFamily;


	public bool isCrossing = false;
	public int parentIndex1; //IN GameManager.gm.pm.plantPrefabs
	public int parentIndex2;

	public int plantTextureIndex = -1;

	public int pointValue;
	public int PointValue { get; set; }

	public int plantTypeIndex;

	public int plantSeed;
	[Range(0f, 1f)]
	public float time;
	public float maxDuration;

	public Interval trunkGrowthDuration;
	public Interval TrunkGrowthDuration 
	{ get {return trunkGrowthDuration;} set{trunkGrowthDuration = value;} }

	public bool offsetTangents;

	public bool smooth;
	public bool Smooth 
	{ get { return smooth; } set { smooth = value; } }

	public float smoothCoef;
	public float SmoothCoef 
	{ get { return smoothCoef; } set { smoothCoef = value; } }


	public bool noise;
	public bool Noise
	{ get { return noise; } set { noise = value; } }

	public float noiseCoef;
	public float NoiseCoef
	{ get { return noiseCoef; } set { noiseCoef = value; } }

	public bool initialForce;
	public bool InitialForce
	{ get { return initialForce; } set { initialForce = value; } }

	public float initialForceCoef;
	public float InitialForceCoef
	{ get { return initialForceCoef; } set { initialForceCoef = value; } }

	public int nbOfSides;
	public int NbOfSides
	{ get { return nbOfSides; } set { nbOfSides = value; } }

	public int nbOfSegments;
	public int NbOfSegments
	{ get { return nbOfSegments; } 
		set { nbOfSegments = value;
			finalSegmentLength = plantLength / (float)nbOfSegments;} }

	public float initialRadius;
	public float InitialRadius 
	{ get { return initialRadius; } set { initialRadius = value; } }

	public Vector3 initialDirection;
	public Vector3 InitialDirection 
	{ get { return initialDirection; } set { initialDirection = value; } }

	public Vector3 initialNormal;

	public int sectionsPerUvUnit;

	public AnimationCurve initialShapeOverLength;

	public AnimationCurve finalShapeOverLength;
	public AnimationCurve FinalShapeOverLength
	{ get { return finalShapeOverLength; } set { finalShapeOverLength = value; } }

	public AnimationCurve shapeOverTime;

	public AnimationCurve radiusOverAngle;

	public float initialSegmentLength;
	public float finalSegmentLength;
	public AnimationCurve lengthOverTime;
	public AnimationCurve segmentLengthOverLength;


	public bool hasGravity;
	public bool HasGravity
	{ get { return hasGravity;} set { hasGravity = value; } }

	public float gravityForce;
	public float GravityForce
	{ get { return gravityForce; } set { gravityForce = value; } }

	public AnimationCurve gravityOverLength;
	public AnimationCurve GravityOverLength
	{ get { return gravityOverLength; } set { gravityOverLength = value; } }

	public bool hasNoise;
	public bool HasNoise
	{ get { return hasNoise; } set { hasNoise = value; } }

	public float noiseForce;
	public float NoiseForce
	{ get { return noiseForce; } set { noiseForce = value; } }

	public float noiseSize;
	public float NoiseSize
	{ get { return noiseSize; } set { noiseSize = value; } }

	public AnimationCurve noiseOverLength;
	public AnimationCurve NoiseOverLength
	{ get { return noiseOverLength; } set { noiseOverLength = value; } }

	public bool hasCollisions;
	public bool HasCollisions
	{ get { return hasCollisions; } set { hasCollisions = value; } }

	//LEAVES
	public bool hasLeaves;
	public bool HasLeaves
	{ get { return hasLeaves; } set { hasLeaves = value; } }

	public Leaf leafPrefab;

	public Interval leafSize;
	public Interval LeafSize
	{ get { return leafSize; } set { leafSize = value; } }

	public bool leavesOnlyOnSections;

	public float leavesDensity;
	public float LeavesDensity
	{ get { return leavesDensity; } set { leavesDensity = value; } }

	public int leavesNumber;
	public int LeavesNumber
	{ get { return leavesNumber; } set { leavesNumber = value;
			leavesRepartitionMode = LeavesRepartitionMode.Number;} }

	public LeavesRepartitionMode leavesRepartitionMode;
	public LeavesRepartitionMode LeavesRepartitionMode
	{ get { return leavesRepartitionMode; } set { leavesRepartitionMode = value; } }

	public AnimationCurve leavesDistribution;
	public AnimationCurve LeavesDistribution
	{ get { return leavesDistribution; } set { leavesDistribution = value; } }

	public AnimationCurve leavesBirthdateDistribution;


	public Interval leafGrowthDuration;
	public Interval LeafGrowthDuration
	{ get { return leafGrowthDuration; } set { leafGrowthDuration = value; } }


	public float sunOrientationIntensity;
	public float SunOrientationIntensity
	{ get { return sunOrientationIntensity; } set { sunOrientationIntensity = value; } }


	public AnimationCurve sunOrientationIntensityOverTime;
	public AnimationCurve leafGrowthOverTime;


	//RECURSIONS
	public bool hasRecursions;
	public bool HasRecursions
	{ get { return hasRecursions; } set { hasRecursions = value;
			DestroyChilds ();} }

	public Plant branchPrefab;

	public float brancheAngleDelta;
	public float BrancheAngleDelta
	{ get { return brancheAngleDelta; } set { brancheAngleDelta = value; } }

	public int nbOfBranches;
	public int NbOfBranches
	{ get { return nbOfBranches; } set { nbOfBranches = value; } }

	public AnimationCurve branchesDistribution;
	public AnimationCurve BranchesDistribution
	{ get { return branchesDistribution; } set { branchesDistribution = value; } }

	public AnimationCurve branchBirthDateDistribution;

	public AnimationCurve branchesDistributionOverLength;
	public AnimationCurve BranchesDistributionOverLength
	{ get { return branchesDistributionOverLength; } set { branchesDistributionOverLength = value; } }

	public Interval branchGrowthDuration;
	public Interval BranchGrowthDuration
	{ get { return branchGrowthDuration; } set { branchGrowthDuration = value; } }


	public bool brancheIndependentRadius;

	public Interval branchInitialRadiusMultiplier;
	public Interval BranchInitialRadiusMultiplier
	{ get { return branchInitialRadiusMultiplier; } set { branchInitialRadiusMultiplier = value; } }



	public AnimationCurve branchesTangencityOverLength;
	public AnimationCurve BranchesTangencityOverLength
	{ get { return branchesTangencityOverLength; } set { branchesTangencityOverLength = value; } }

	public bool branchesOnlyOnSections;
	public bool BranchesOnlyOnSections
	{ get { return branchesOnlyOnSections; } set { branchesOnlyOnSections = value; } }

	public AnimationCurve branchLengthOverTrunkLength;
	public AnimationCurve BranchLengthOverTrunkLength
	{ get { return branchLengthOverTrunkLength; } set { branchLengthOverTrunkLength = value; } }


	public bool brancheIndependentLength;

	public Interval brancheLengthRatio;
	public Interval BrancheLengthRatio
	{ get { return brancheLengthRatio; } set { brancheLengthRatio = value; } }

	//FLOWERS
	public bool hasFlowers;
	public bool HasFlowers
	{ get { return hasFlowers; } set { hasFlowers = value; } }

	public bool uniqueEndFlower;
	public bool UniqueEndFlower
	{ get { return uniqueEndFlower; } set { uniqueEndFlower = value; } }

	public Flower flowerPrefab;

	public bool hasFixedDirection;
	public bool HasFixedDirection
	{ get { return hasFixedDirection; } set { hasFixedDirection = value; } }


	public AnimationCurve flowerGrowthOverTime;
	public AnimationCurve flowersBirthdateDistribution;


	public Interval flowerGrowthDuration;
	public Interval FlowerGrowthDuration
	{ get { return flowerGrowthDuration; } set { flowerGrowthDuration = value; } }

	public AnimationCurve flowerSizeOverLength;
	public AnimationCurve FlowerSizeOverLength
	{ get { return flowerSizeOverLength; } set { flowerSizeOverLength = value; } }

	public Interval flowerSize;
	public Interval FlowerSize
	{ get { return flowerSize; } set { flowerSize = value; } }

	public float leafChanceOfBeingFlower;
	public float LeafChanceOfBeingFlower
	{ get { return leafChanceOfBeingFlower; } set { leafChanceOfBeingFlower = value; } }


	//FRUITS

	public bool hasFruits;
	public bool HasFruits
	{ get { return hasFruits; } set { hasFruits = value; } }

	public Fruit fruitPrefab;

	public LeavesRepartitionMode fruitRepartitionMode;
	public LeavesRepartitionMode FruitRepartitionMode
	{ get { return fruitRepartitionMode; } set { fruitRepartitionMode = value; } }

	public int fruitNumber;
	public int FruitNumber
	{ get { return fruitNumber; } set { fruitNumber = value; } }

	public float fruttiFlowersRatio;
	public float FruttiFlowersRatio
	{ get { return fruttiFlowersRatio; } set { fruttiFlowersRatio = value; } }

	public Interval fruitSize;
	public Interval FruitSize
	{ get { return fruitSize; } set { fruitSize = value; } }

	public Interval fruitGrowthDuration;
	public Interval FruitGrowthDuration
	{ get { return fruitGrowthDuration; } set { fruitGrowthDuration = value; } }


	public int fruitSequence;


	public Plant parent;
	public Plant Trunk {
		get {
			if (!isBranch) {
				return this;
			} else {
				if (isBranch && parent == null) {
					parent = transform.parent.parent.GetComponent<Plant> ();
				}
				return parent.Trunk;
			}
		}
	}


	void DestroyChilds()
	{
		for (int i = 0; i < transform.childCount; i++) {
			Destroy (transform.GetChild (i).gameObject);
		}
	}


	#endregion

	#region PrivateVariables

	[HideInInspector]
	public int placedFruits;


	//[HideInInspector]
	public bool isBranch;
	
	[HideInInspector]
	public int indexInGameData;

	private PositionsAndNormals positionsAndNormals;

	private LeafInfo[] leaves;
	private int actualLeavesNumber;
	private Transform leavesParent;

	private BranchesInfo[] branches;
	private int actualBranchesNumber;
	private Transform branchesParent;


	private Vector3[] points;
	private int[] triangles;
	private Vector2[] uvs;
	private MeshFilter mf;

	private float totalLength;

	private float actualTrunkGrowthDuration;
	private float lastUpdateTime;
	private AnimationCurve trunkTimeOverTime;
	#endregion



	private bool hasSetSeed = false;

	public void SetSeed(int seed)
	{
		plantSeed = seed;
		Random.InitState (plantSeed);
		plantNumber++;
		hasSetSeed = true;

		//Debug.Log ("seed : " + plantSeed);
	}

	public void InitializePlant()
	{
		//Debug.Log ("radius : " + initialRadius);

		//RANDOM INITIALISATION
		if (isBranch)
			parent = transform.parent.parent.GetComponent<Plant> ();


		
		if (!hasSetSeed) {
			SetSeed (plantNumber);
		}

		lastUpdateTime = 0f;

		actualTrunkGrowthDuration = TrunkGrowthDuration.RandomValue();
			
		maxDuration = actualTrunkGrowthDuration;
		if (hasFlowers)
			maxDuration += flowerGrowthDuration.max;
		if (hasFruits)
			maxDuration += fruitGrowthDuration.max;
		if (hasLeaves)
			maxDuration += leafGrowthDuration.max;
		if (hasRecursions) {
			maxDuration += branchGrowthDuration.max;
			if (branchPrefab.hasLeaves)
				maxDuration += branchPrefab.leafGrowthDuration.max;
			if (branchPrefab.hasFlowers)
				maxDuration += branchPrefab.flowerGrowthDuration.max;
			if (branchPrefab.hasFruits)
				maxDuration += branchPrefab.fruitGrowthDuration.max;
			if (branchPrefab.hasRecursions)
				maxDuration += branchPrefab.branchGrowthDuration.max;
			if (branchPrefab.hasRecursions && branchPrefab.branchPrefab != null) {
				if (branchPrefab.branchPrefab.hasLeaves)
					maxDuration += branchPrefab.branchPrefab.leafGrowthDuration.max;
				if (branchPrefab.branchPrefab.hasFlowers)
					maxDuration += branchPrefab.branchPrefab.flowerGrowthDuration.max;
				if (branchPrefab.branchPrefab.hasFruits)
					maxDuration += branchPrefab.branchPrefab.fruitGrowthDuration.max;
				if (branchPrefab.branchPrefab.hasRecursions)
					maxDuration += branchPrefab.branchPrefab.branchGrowthDuration.max;


			}
		}



		InitializeMesh ();


		if (hasFruits) {
			placedFruits = 0;
			if (time == 0f)
				fruitSequence = 0;
		}

		if (hasLeaves || hasFlowers)
			GenerateLeavesAndFLowers ();

		if (hasRecursions)
			GenerateRecursions ();


		UpdatePlant ();
	}

	public void UpdatePlant()
	{
		if (lastUpdateTime < 1f) {

			UpdateMesh ();
		
			if (hasLeaves || hasFlowers)
				UpdateLeavesAndFlowers ();
		
			if (hasRecursions)
				UpdateRecursions ();

			lastUpdateTime = time;
		}
	}

	public void InitializeMesh()
	{
		trunkTimeOverTime = new AnimationCurve (new Keyframe[] {
			new Keyframe (0, 0),
			new Keyframe (actualTrunkGrowthDuration / maxDuration, 1f),
			new Keyframe (1, 1)
		});
		positionsAndNormals = GenerateTrajectory (transform.position, InitialDirection, nbOfSegments + 1);

		mf = GetComponent<MeshFilter> ();

		triangles = new int[6 * nbOfSides * nbOfSegments];
		GenerateTriangles (points, triangles);

		points = new Vector3[nbOfSides * (nbOfSegments + 1)];
		//GeneratePoints (0f, points);

		uvs = new Vector2[points.Length];
		GenerateUVs (uvs);


		Mesh m = mf.mesh;
		m.Clear ();

		m.vertices = points;
		m.triangles = triangles;
		m.uv = uvs;

		m.RecalculateNormals ();

		mf.mesh = m;

	}

	public void UpdateMesh()
	{
		//UPDATE POINTS
		GeneratePoints (time, points);

		//MODIFY MESH
		Mesh m = mf.mesh;

		m.Clear ();

		m.vertices = points;
		m.triangles = triangles;
		m.uv = uvs;


		m.RecalculateNormals ();

		//Destroy (mf.mesh);

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
		GeneratePoints (1f, points);

		float angle = Random.value * 2 * Mathf.PI;


		for(int i = 0 ; i < nbOfBranches ; i++){

			float lengthRatio = (float)i / (float)nbOfBranches;

			lengthRatio = branchesDistributionOverLength.Evaluate (lengthRatio);
			if (lengthRatio == 1f)
				lengthRatio = (nbOfSegments - 1f) / (float)nbOfSegments;
			
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

				if (offsetTangents) {
					Vector3 normal = positionsAndNormals.nor [segment];
					position += normal * actualTrunkRadius;
				}
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

				branche.TrunkGrowthDuration = branchGrowthDuration;

				//branche.leafGrowthDuration = leafGrowthDuration;

				branche.isBranch = true;
				branche.InitialDirection = direction;
				if (!branche.brancheIndependentRadius)
					branche.initialRadius = initialBrancheRadiusBranch * branchInitialRadiusMultiplier.RandomValue();
				else
					branche.initialRadius *= branchInitialRadiusMultiplier.RandomValue();

				branche.initialNormal = tangentDirection;

				//branche.initialSegmentLength = initialSegmentLength;
				//branche.finalSegmentLength = finalSegmentLength;

				//branche.nbOfSides = Mathf.Max (3, nbOfSides - 1);
				if (!branche.brancheIndependentLength) {
					float nbOfS = branchLengthOverTrunkLength.Evaluate (lengthRatio) * nbOfSegments * brancheLengthRatio.RandomValue () + 1;
					branche.nbOfSegments = (int)nbOfS;
				} else {

					float mult = brancheLengthRatio.RandomValue ();
					branche.finalSegmentLength *= mult;
					branche.initialSegmentLength *= mult;

				}

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
	public void GenerateLeavesAndFLowers()
	{

		GeneratePoints (1f, points);
		
		actualLeavesNumber = 0;

		Vector3 randomVector = new Vector3 (0.132354f, 1.98654f, -1.5646f).normalized;

		//CREATE THE PARENT OF ALL LEAVES
		if (leavesParent != null)
			DestroyImmediate (leavesParent.gameObject);

		leavesParent = new GameObject ("LeaveParent").transform;
		leavesParent.parent = transform;
		leavesParent.transform.localPosition = Vector3.zero;

		int leavesIteration = 0;

		if (leavesRepartitionMode == LeavesRepartitionMode.Density)
			leavesIteration = (int)(leavesDensity * totalLength);
		else if (leavesRepartitionMode == LeavesRepartitionMode.Number)
			leavesIteration = leavesNumber;

		if (uniqueEndFlower)
			leavesIteration += 1;

		leaves = new LeafInfo[leavesIteration];

		int iteration = 0;

		while (iteration < leavesIteration) {

			float lengthRatio = Random.value;
			float proba = leavesDistribution.Evaluate (lengthRatio);

			if (uniqueEndFlower && iteration == 0) {
				lengthRatio = 0.99f;
				proba = 2f;
			}

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
				if (uniqueEndFlower && iteration == 0) {
					position = mf.sharedMesh.vertices [mf.sharedMesh.vertices.Length - 1];
				} else {
					if (branchesOnlyOnSections)
						position = mf.sharedMesh.vertices [pointIndex1];
					else
						position = (1f - lerpValue) * mf.sharedMesh.vertices [pointIndex1] + lerpValue * mf.sharedMesh.vertices [pointIndex1 + nbOfSides];
				}
				//INITIAL DIRECTION
				Vector3 direction = mf.sharedMesh.normals[pointIndex1] - mf.sharedMesh.normals[pointIndex2];

				//TANGEANT DIRECTION
				Vector3 tangentDirection = mf.sharedMesh.vertices [pointIndex1] - mf.sharedMesh.vertices [pointIndex1 + nbOfSides];


				//ORIENTATION TOWARD THE SUN
				Vector3 up = (1f - sunOrientationIntensity) * tangentDirection.normalized + sunOrientationIntensity * Vector3.up * sunOrientationIntensityOverTime.Evaluate(time);
				direction.y *= sunOrientationIntensity * sunOrientationIntensityOverTime.Evaluate(time);

				if (uniqueEndFlower && iteration == 0) {
					int last = positionsAndNormals.pos.Length - 1;
					direction = positionsAndNormals.pos[last] - positionsAndNormals.pos[last - 1];
					Debug.DrawRay (position, direction.normalized, Color.red, 10f);
				}

				direction.Normalize ();

				GameObject thing;
				bool isFlower = hasFlowers &&
				                ((uniqueEndFlower && iteration == 0)
				                || (Random.value < leafChanceOfBeingFlower && !uniqueEndFlower));



				//Debug.Log ("Is Leaf ? : " + !isFlower);

				float birthDate = 0f;
				float growthDuration = 0f;
				if (!isFlower) {
					
					//LEAF CREATION
					Leaf leaf = Instantiate (leafPrefab, position, Quaternion.identity);
					thing = leaf.gameObject;
					leaf.name = "Leaf_" + actualLeavesNumber.ToString ();
					leaf.initialDirection = direction;
					leaf.upDirection = up;

					growthDuration = leafGrowthDuration.RandomValue ();

					//Debug.Log (leafGrowthDuration.min + "   " + leafGrowthDuration.max);

					//Debug.Log ("Leaf growthDuration = " + growthDuration);

					birthDate = Mathf.Lerp (ValueAt (trunkTimeOverTime, lengthRatio) * maxDuration, maxDuration - growthDuration, leavesBirthdateDistribution.Evaluate (Random.value));

					leaf.transform.localScale *= leafSize.RandomValue ();

					leaf.time = 0f;
				} else {





					Flower flower = Instantiate (flowerPrefab, position, Quaternion.identity);
					thing = flower.gameObject;

					growthDuration = flowerGrowthDuration.RandomValue ();

					flower.name = "Flower_" + actualLeavesNumber.ToString ();
					if (!hasFixedDirection)
						flower.initialDirection = direction;

					//Debug.Log ("at time " + time + " : " + (Trunk.fruitSequence & (1 << Trunk.placedFruits)));
					if (hasFruits) {
						if ((
						       Trunk.time == 0f && (Trunk.fruitRepartitionMode == LeavesRepartitionMode.Number &&
						       Trunk.placedFruits < Trunk.fruitNumber) ||
						       (Trunk.fruitRepartitionMode == LeavesRepartitionMode.Density &&
						       Random.value < Trunk.fruttiFlowersRatio))
						   || ((Trunk.fruitSequence & (1 << Trunk.placedFruits)) > 0)) {

							//Debug.Log ("j'arrive ICI");

							Fruit fruit = (Fruit)Instantiate (fruitPrefab, flower.transform);

							fruit.transform.localPosition = Vector3.zero;
							fruit.initialDirection = flower.initialDirection;


							float sizeMult = fruitSize.RandomValue ();

							fruit.length *= sizeMult;
							fruit.radius *= sizeMult;


							flower.fruitGrowthDuration = fruitGrowthDuration.RandomValue ();
							flower.hasAFruit = true;
							flower.fruit = fruit;

							fruit.fruitIndex = Trunk.placedFruits;
							fruit.indexInPlantManager = Trunk.plantTypeIndex;

							growthDuration += flower.fruitGrowthDuration;

							//Debug.Log ("je met un fruit  " + Trunk.placedFruits + "/" + Trunk.fruitNumber);

							if (time == 0f) {

								Trunk.fruitSequence = Trunk.fruitSequence | (1 << Trunk.placedFruits);

							}

							Trunk.placedFruits++;
						} else {
							Trunk.placedFruits++;

						}
					}

					flower.thisGrowthDuration = growthDuration;

						
					//flower.upDirection = up;


					birthDate = Mathf.Lerp (ValueAt (trunkTimeOverTime, lengthRatio) * maxDuration, maxDuration - growthDuration, flowersBirthdateDistribution.Evaluate (Random.value));

					flower.transform.localScale *= 
						flowerSize.RandomValue () * flowerSizeOverLength.Evaluate (lengthRatio);

					flower.time = 0f;

					flower.SetPlantRoot (Trunk);

				}

				thing.transform.parent = leavesParent;

				//Debug.Log ("growthDuration = " + growthDuration);

				LeafInfo l = new LeafInfo (uniqueEndFlower && iteration == 0, !isFlower, thing, lengthRatio, direction, pointIndex1, pointIndex2, lerpValue, birthDate, growthDuration);
					
				leaves [actualLeavesNumber] = l;


				//INCREMENTS
				actualLeavesNumber++;
			}

			iteration++;

		}
	}

	//USED TO UPDATE THE LEAVES EVERY FRAME
	public void UpdateLeavesAndFlowers()
	{
		float absolutTime = time * maxDuration;

		for (int i = 0; i < actualLeavesNumber; i++) {

			//DISTANCE FROM ROOT / LENGTH
			LeafInfo thing = leaves [i];


			if (thing.birthDate > absolutTime) {
				//NOT GROWN
				if (thing.isLeafOrFlower)
					thing.leaf.time = 0f;
				else
					thing.flower.time = 0f;
			} else {

				if (absolutTime - thing.birthDate < thing.growthDuration || !thing.grownOnce) {

					float localTime = 0f;

					if (thing.isLeafOrFlower) {
						localTime = Mathf.Min(1f, leafGrowthOverTime.Evaluate ((absolutTime - thing.birthDate) / thing.growthDuration));
						//Debug.Log (absolutTime + "    " + thing.birthDate + "      " + thing.growthDuration);
						//Debug.Log (localTime);

						thing.leaf.time = leafGrowthOverTime.Evaluate (localTime);
					} else {
						//Debug.Log (absolutTime + "    " + thing.birthDate + "      " + thing.growthDuration);
						localTime = Mathf.Min(1f, flowerGrowthOverTime.Evaluate ((absolutTime - thing.birthDate) / thing.growthDuration));
						//Debug.Log (localTime);
						thing.flower.time = flowerGrowthOverTime.Evaluate (localTime);
					}

					//Debug.Log ("growth duration : " + thing.growthDuration);

					//INTIAL DIRECTION
					Vector3 initialDirection = (mf.sharedMesh.vertices [thing.pointIndex1] - mf.sharedMesh.vertices [thing.pointIndex2]).normalized;
					initialDirection.y = Mathf.Lerp (initialDirection.y, 1f - sunOrientationIntensity, sunOrientationIntensityOverTime.Evaluate (localTime));

					//TANGENT DIRECTION
					Vector3 tangentDirection = mf.sharedMesh.vertices [thing.pointIndex1] - mf.sharedMesh.vertices [thing.pointIndex1 + nbOfSides];

					//UP DIRECTION
					Vector3 up = Vector3.Lerp(tangentDirection.normalized ,Vector3.up * sunOrientationIntensityOverTime.Evaluate (localTime), sunOrientationIntensity);

					Vector3 localPosition = Vector3.Lerp(mf.sharedMesh.vertices [thing.pointIndex1], mf.sharedMesh.vertices [thing.pointIndex1 + nbOfSides] , thing.lerpValue);

					if (thing.isLeafOrFlower) {
						thing.leaf.initialDirection = initialDirection.normalized;
						thing.leaf.transform.localPosition = localPosition;
						thing.leaf.upDirection = up.normalized;
						thing.leaf.UpdateMesh ();
					} else {
						if (!thing.isEndFlower && !hasFixedDirection)
							thing.flower.initialDirection = initialDirection.normalized;

						thing.flower.transform.localPosition = localPosition;
						//thing.flower.upDirection = up.normalized;
						thing.flower.UpdateMesh ();
					}
					thing.grownOnce = true;
				}
				

			}
		}
	}

	//CALLED ONCE AT THE BEGINNING
	public void GenerateTriangles(Vector3[] points, int[] triangleArrayRef)
	{


		for (int i = 0; i < nbOfSegments; i++) {

			for (int j = 0; j < nbOfSides; j++) 
			{

				int index = i * nbOfSides + j;

				int p0 = index;
				int p1 = j + 1 == nbOfSides ? i * nbOfSides : p0 + 1;

				int p2 = p0 + nbOfSides;
				int p3 = p1 + nbOfSides;

				triangleArrayRef [6 * index + 0] = p0;
				triangleArrayRef [6 * index + 1] = p1;
				triangleArrayRef [6 * index + 2] = p2;

				triangleArrayRef [6 * index + 3] = p1;
				triangleArrayRef [6 * index + 4] = p3;
				triangleArrayRef [6 * index + 5] = p2;
			}
		}
	}

	//CALLED EVERY TIME THE VARIABLE time IS CHANGED
	public void GeneratePoints(float time, Vector3[] points)
	{
		//CONSTANTS

		float trunkTime = trunkTimeOverTime.Evaluate(time);

		Vector3 randomVector = new Vector3 (0.132354f, 1.98654f, -1.5646f).normalized;
		float angleCoefficient = Mathf.PI * 2f / nbOfSides;

		//INITIALIZATIONS

		Vector3 v = randomVector;


		Vector3 direction;



		for (int i = 0; i < nbOfSegments + 1; i++) {

			Vector3 currentPosition = positionsAndNormals.pos [i] - transform.position;

			//DIRECTION DETERMINATION
			if (i == 0)
				direction = InitialDirection;
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

				Vector3 point = currentPosition
					+ (u * Mathf.Cos (angle) + v * Mathf.Sin (angle)) * radius * radiusOverAngle.Evaluate((float)j / nbOfSides);

				if (offsetTangents)
					point += positionOffset;

				int index = i * nbOfSides + j;
				points[index] = point;
			}
		}

		//SMOOTHING
		if (smooth) {
			SmoothPoints (points, smoothCoef);
			SmoothPoints (points, smoothCoef);
		}

		//NOISING
		if (noise) {
			NoisePoints (points, noiseCoef);
		}
	}

	//USED TO SMOOTH THE POINTS OF THE 3D MESH
	private void SmoothPoints(Vector3[] pointsArrayRef, float coef)
	{
		int np = pointsArrayRef.Length;
		//List<Vector3> nTempPoints = new List<Vector3>();

		for (int i = 0; i < np; i++) {

			if (i < nbOfSides || i > np - nbOfSides - 1) {

			} else {
				pointsArrayRef[i] = ((pointsArrayRef [i - nbOfSides] + pointsArrayRef [i + nbOfSides]) * 0.5f * coef) + (1f - coef) * pointsArrayRef [i];
			}
		}
	}

	//USED TO NOISE THE POINTS OF THE 3D MESH
	//USES REF OF POINTS ARRAY
	private void NoisePoints(Vector3[] refPoints, float coef)
	{
		for (int i = 0; i < refPoints.Length; i++) {

			float ratio = i / (float)refPoints.Length;

			float v = shapeOverTime.Evaluate (0.5f);

			float radius = (initialShapeOverLength.Evaluate(ratio) * v + (1f - v) * finalShapeOverLength.Evaluate(ratio)) * initialRadius;

			if (lengthOverTime.Evaluate (time) > ratio) 
			{
				Vector3 noise = Noise3D.PerlinNoise3D (5f * refPoints [i] / radius);
				refPoints [i] += noise.normalized * coef * radius;
			}
		}
	}

	//GENERATE THE UV MAP FOR THE MESH
	private void GenerateUVs(Vector2[] uvsArrayRef)
	{
		for (int i = 0; i < nbOfSegments + 1; i++) {

			int iMod = i % (sectionsPerUvUnit + 1);

			float ratioY = iMod / (float)nbOfSegments;

			for (int j = 0; j < nbOfSides; j++) 
			{
				float ratioX = j / (float)nbOfSides;

				Vector2 p = new Vector2 (ratioX, ratioY);

				//Debug.DrawRay (transform.position + 3f * new Vector3 (p.x, 0f, p.y), Vector3.up, Color.red, 10f);

				int index = i * nbOfSides + j;
				uvsArrayRef[index] = p;
			}
		}
	}

	//USED TO GENERATE THE TRAJECTORY AT THE CREATION OF THE PLANT
	public PositionsAndNormals GenerateTrajectory(Vector3 initialPosition, Vector3 initialDirection, int arrayLength)
	{

		//PARAMETERS
		totalLength = 0f;
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
				noise = noiseForce * noiseOverLength.Evaluate (lengthRatio) * Noise3D.PerlinNoise3D ((currentPosition + noiseOffset + plantSeed*Vector3.left)/ actualNoiseSize);
			
			
			}

			Vector3 force = currentDirection;
			if (initialForce)
				force = initialForceCoef * initialDirection;

			float distance = Mathf.Lerp (initialSegmentLength, finalSegmentLength, segmentLengthOverLength.Evaluate (lengthRatio));
			totalLength += distance;

			currentDirection = distance * (force + gravity + noiseMultiplier * noise).normalized;
			//currentDirection = distance * (force + currentDirection + gravity + noiseMultiplier * noise).normalized;


			PosDir pd = new PosDir (currentPosition, currentDirection, currentNormal);


			//CALCULATE COLLISIONS
			if (hasCollisions) {
				//COLLISION CALCULATION WITH 10 ITERATIONS
				for (int k = 0; k < 10; k++)
					pd = Collision (new PosDir (currentPosition, pd.dir, pd.normal), distance);

				currentPosition = pd.pos;
				currentDirection = pd.dir;
			
			} else {

				currentPosition += currentDirection;
			}



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
/*
//STRUCTURE FOR FLOWERS AND INFORMATION ABOUT IT
struct FlowerInfo
{
	public Flower flower;
	public float lengthRatio;

	public Vector3 normalDirection;

	public int pointIndex1;
	public int pointIndex2;

	public float lerpValue;

	public float birthDate;
	public float growthDuration;

	public bool grownOnce;

	public FlowerInfo(Flower flower, float lengthRatio, Vector3 normalDirection, int pointIndex1, int pointIndex2, float lerpValue, float birthDate, float growthDuration )
	{
		this.flower = flower;
		this.lengthRatio = lengthRatio;
		this.pointIndex1 = pointIndex1;
		this.pointIndex2 = pointIndex2;
		this.lerpValue = lerpValue;

		this.birthDate = birthDate;
		this.growthDuration = growthDuration;

		this.normalDirection = normalDirection;

		grownOnce = false;

		flower.Initialize ();
	}




}
*/

//STRUCTURE FOR BRANCHES AND INFORMATION ABOUT IT
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
	public bool isLeafOrFlower;

	public Leaf leaf;
	public Flower flower;

	public float lengthRatio;

	public Vector3 normalDirection;

	public int pointIndex1;
	public int pointIndex2;

	public float lerpValue;

	public float birthDate;
	public float growthDuration;

	public bool grownOnce;

	public bool isEndFlower;

	public LeafInfo(bool isEndFlower, bool isLeafOrFlower, GameObject thing, float lengthRatio, Vector3 normalDirection, int pointIndex1, int pointIndex2, float lerpValue, float birthDate, float growthDuration )
	{
		this.isEndFlower = isEndFlower;
		this.isLeafOrFlower = isLeafOrFlower;

		if (isLeafOrFlower) {
			this.leaf = thing.GetComponent<Leaf> ();
			this.leaf.Initialize ();
			this.flower = null;
		} else {
			this.flower = thing.GetComponent<Flower> ();
			this.flower.Initialize ();
			this.leaf = null;
		}
		this.lengthRatio = lengthRatio;
		this.pointIndex1 = pointIndex1;
		this.pointIndex2 = pointIndex2;
		this.lerpValue = lerpValue;

		this.birthDate = birthDate;
		this.growthDuration = growthDuration;

		this.normalDirection = normalDirection;



		grownOnce = false;

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





