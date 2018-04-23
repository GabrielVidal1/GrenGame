using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class PlantSerializer {

	
	public static void SavePlant(Plant plant)
	{
		
		SerializedPlantData p = new SerializedPlantData(plant);
		
		string path = Application.persistentDataPath + "/" + plant.name.ToUpper () + ".grenplant";

		Debug.Log (path);
		BinaryFormatter bf = new BinaryFormatter ();


		FileStream file = File.Create( path );

		bf.Serialize( file, p );
		file.Close();


	}

}

[System.Serializable]
class SerializedPlantData
{

	#region PUBLIC VARIABLES

	public int plantSeed;

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
	public SerializedVector3 initialDirection;
	public SerializedVector3 initialNormal;

	public int sectionsPerUvUnit;

	public SerializedAnimationCurve initialShapeOverLength;
	public SerializedAnimationCurve finalShapeOverLength;
	public SerializedAnimationCurve shapeOverTime;


	public float initialSegmentLength;
	public float finalSegmentLength;
	public SerializedAnimationCurve lengthOverTime;
	public SerializedAnimationCurve segmentLengthOverLength;


	public bool hasGravity;
	public float gravityForce;
	public SerializedAnimationCurve gravityOverLength;


	public bool hasNoise;
	public float noiseForce;
	public float noiseSize;
	public SerializedAnimationCurve noiseOverLength;

	public bool hasCollisions;

	public bool hasLeaves;
	public SerializedLeaf leafPrefab;

	public Interval leafSize;

	public bool leavesOnlyOnSections;

	public float leavesDensity;
	public SerializedAnimationCurve leavesDistribution;
	public SerializedAnimationCurve leavesBirthdateDistribution;
	public Interval leafGrowthDuration;



	public float sunOrientationIntensity;
	public SerializedAnimationCurve sunOrientationIntensityOverTime;
	public SerializedAnimationCurve leafGrowthOverTime;


	//RECURSIONS

	public bool hasRecursions;
	public SerializedPlantData branchPrefab;

	public float brancheAngleDelta;

	public int nbOfBranches;
	public SerializedAnimationCurve branchesDistribution;
	public SerializedAnimationCurve branchBirthDateDistribution;

	public Interval branchGrowthDuration;
	public Interval branchInitialRadiusMultiplier;



	public SerializedAnimationCurve branchesTangencityOverLength;

	public bool branchesOnlyOnSections;

	public SerializedAnimationCurve branchLengthOverTrunkLength;

	public Interval brancheLengthRatio;

	public bool branchForceParameters;

	#endregion


	public SerializedPlantData( Plant plant)
	{
		if (plant == null)
			return;
		
		plantSeed = plant.plantSeed;
		time = plant.time;

		maxDuration = plant.maxDuration;

		
		trunkGrowthDuration = plant.trunkGrowthDuration;


        smooth = plant.smooth;
		
		smoothCoef = plant.smoothCoef;
		noise = plant.noise;
		
		noiseCoef = plant.noiseCoef;

		
		initialForce = plant.initialForce;
		
		initialForceCoef = plant.initialForceCoef;

		
		nbOfSides = plant.nbOfSides;
		
		nbOfSegments = plant.nbOfSegments;

		
		initialRadius = plant.initialRadius;
		
		initialDirection = new SerializedVector3(plant.initialDirection);
		
		initialNormal = new SerializedVector3(plant.initialNormal);

		
		sectionsPerUvUnit = plant.sectionsPerUvUnit;

		
		initialShapeOverLength = new SerializedAnimationCurve(plant.initialShapeOverLength);
		
		finalShapeOverLength = new SerializedAnimationCurve(plant.finalShapeOverLength);
		
		shapeOverTime = new SerializedAnimationCurve(plant.shapeOverTime);


		
		initialSegmentLength = plant.initialSegmentLength;
		
		finalSegmentLength = plant.finalSegmentLength;
		
		lengthOverTime = new SerializedAnimationCurve(plant.lengthOverTime);
		
		segmentLengthOverLength = new SerializedAnimationCurve(plant.segmentLengthOverLength);


		
		hasGravity = plant.hasGravity;
		
		gravityForce = plant.gravityForce;
		
		gravityOverLength = new SerializedAnimationCurve(plant.gravityOverLength);


		
		hasNoise = plant.hasNoise;
		
		noiseForce = plant.noiseForce;
		
		noiseSize = plant.noiseSize;
		
		noiseOverLength = new SerializedAnimationCurve(plant.noiseOverLength);

		
		hasCollisions = plant.hasCollisions;

		
		hasLeaves = plant.hasLeaves;
		
		leafPrefab = new SerializedLeaf(plant.leafPrefab);

		
		leafSize = plant.leafSize;

		
		leavesOnlyOnSections = plant.leavesOnlyOnSections;

		
		leavesDensity = plant.leavesDensity;
		
		leavesDistribution = new SerializedAnimationCurve(plant.leavesDistribution);
		
		leavesBirthdateDistribution = new SerializedAnimationCurve(plant.leavesBirthdateDistribution);
		
		leafGrowthDuration = plant.leafGrowthDuration;



		
		sunOrientationIntensity = plant.sunOrientationIntensity;
		
		sunOrientationIntensityOverTime = new SerializedAnimationCurve(plant.sunOrientationIntensityOverTime);
		
		leafGrowthOverTime = new SerializedAnimationCurve(plant.leafGrowthOverTime);


		//RECURSIONS

		
		hasRecursions = plant.hasRecursions;
		
		branchPrefab = new SerializedPlantData(plant.branchPrefab);

		
		brancheAngleDelta = plant.brancheAngleDelta;

		
		nbOfBranches = plant.nbOfBranches;
		
		branchesDistribution = new SerializedAnimationCurve(plant.branchesDistribution);
		
		branchBirthDateDistribution = new SerializedAnimationCurve(plant.branchBirthDateDistribution);

		
		branchGrowthDuration = plant.branchGrowthDuration;
		
		branchInitialRadiusMultiplier = plant.branchInitialRadiusMultiplier;



		
		branchesTangencityOverLength = new SerializedAnimationCurve(plant.branchesTangencityOverLength);

		
		branchesOnlyOnSections = plant.branchesOnlyOnSections;

		
		branchLengthOverTrunkLength = new SerializedAnimationCurve(plant.branchLengthOverTrunkLength);

		
		brancheLengthRatio = plant.brancheLengthRatio;
	}

	/*
	public void DeserializedPlant( Plant plant)
	{
		
		plant.plantSeed = plantSeed;
		plant.time = time;
		
		plant.maxDuration = maxDuration;

		
		plant.trunkGrowthDuration = trunkGrowthDuration;


        plant.smooth = smooth;
		
		plant.smoothCoef = smoothCoef;
		plant.noise = noise;
		
		plant.noiseCoef = noiseCoef;

		
		plant.initialForce = initialForce;
		
		plant.initialForceCoef = initialForceCoef;

		
		plant.nbOfSides = nbOfSides;
		
		plant.nbOfSegments = nbOfSegments;

		
		plant.initialRadius = initialRadius;
		
		plant.initialDirection = initialDirection;
		
		plant.initialNormal = initialNormal;

		
		plant.sectionsPerUvUnit = sectionsPerUvUnit;

		
		plant.initialShapeOverLength = initialShapeOverLength;
		
		plant.finalShapeOverLength = finalShapeOverLength;
		
		plant.shapeOverTime = shapeOverTime;


		
		plant.initialSegmentLength = initialSegmentLength;
		
		plant.finalSegmentLength = finalSegmentLength;
		
		plant.lengthOverTime = lengthOverTime;
		
		plant.segmentLengthOverLength = segmentLengthOverLength;


		
		plant.hasGravity = hasGravity;
		
		plant.gravityForce = gravityForce;
		
		plant.gravityOverLength = gravityOverLength;


		
		plant.hasNoise = hasNoise;
		
		plant.noiseForce = noiseForce;
		
		plant.noiseSize = noiseSize;
		
		plant.noiseOverLength = noiseOverLength;

		
		plant.hasCollisions = hasCollisions;

		
		plant.hasLeaves = hasLeaves;
		
		Leaf l = new Leaf();
		leafPrefab.DeserializedLeaf(l);
		
		plant.leafPrefab = l;

		
		plant.leafSize = leafSize;

		
		plant.leavesOnlyOnSections = leavesOnlyOnSections;

		
		plant.leavesDensity = leavesDensity;
		
		plant.leavesDistribution = leavesDistribution;
		
		plant.leavesBirthdateDistribution = leavesBirthdateDistribution;
		
		plant.leafGrowthDuration = leafGrowthDuration;



		
		plant.sunOrientationIntensity = sunOrientationIntensity;
		
		plant.sunOrientationIntensityOverTime = sunOrientationIntensityOverTime;
		
		plant.leafGrowthOverTime = leafGrowthOverTime;


		//RECURSIONS

		
		plant.hasRecursions = hasRecursions;
		
		Plant b = new Plant();
		branchPrefab.DeserializedPlant(b);
		
		plant.branchPrefab = b;

		
		plant.brancheAngleDelta = brancheAngleDelta;

		
		plant.nbOfBranches = nbOfBranches;
		
		plant.branchesDistribution = branchesDistribution;
		
		plant.branchBirthDateDistribution = branchBirthDateDistribution;

		
		plant.branchGrowthDuration = branchGrowthDuration;
		
		plant.branchInitialRadiusMultiplier = branchInitialRadiusMultiplier;



		
		plant.branchesTangencityOverLength = branchesTangencityOverLength;

		
		plant.branchesOnlyOnSections = branchesOnlyOnSections;

		
		plant.branchLengthOverTrunkLength = branchLengthOverTrunkLength;

		
		plant.brancheLengthRatio = brancheLengthRatio;

		
		plant.branchForceParameters = branchForceParameters;

	}
*/


}

[System.Serializable]
class SerializedLeaf
{
	#region variables

	

	public float time;

	public int nbOfTangentSegments;
	public int nbOfNormalSegments;


	public SerializedVector3 initialDirection;
	public SerializedVector3 upDirection;


	public float width;
	public float length;


	public SerializedAnimationCurve widthOverTime;
	public SerializedAnimationCurve lengthOverTime;


	public SerializedAnimationCurve pointsDistributionOverLength;
	public SerializedAnimationCurve pointsDistributionOverTime;

	public SerializedAnimationCurve topDownShape;

	public bool coronalVariations;
	public float coronalCurvatureIntensity;
	public SerializedAnimationCurve coronalCurvature;
	public SerializedAnimationCurve coronalCurvatureIntensityOverLength;
	public SerializedAnimationCurve coronalCurvatureIntensityOverTime;


	public bool sagittalVariations;
	public float sagittalCurvatureIntensity;
	public SerializedAnimationCurve sagittalCurvature;
	public SerializedAnimationCurve sagittalCurvatureIntensityOverLength;
	public SerializedAnimationCurve sagittalCurvatureIntensityOverTime;

	#endregion

	public SerializedLeaf(Leaf leaf)
	{
		if (leaf == null)
			return;

		time = leaf.time;

		nbOfTangentSegments = leaf.nbOfTangentSegments;
		nbOfNormalSegments = leaf.nbOfNormalSegments;


		initialDirection = new SerializedVector3(leaf.initialDirection);
		upDirection = new SerializedVector3(leaf.upDirection);

		width = leaf.width;
		length = leaf.length;


		widthOverTime = new SerializedAnimationCurve(leaf.widthOverTime);
		lengthOverTime = new SerializedAnimationCurve(leaf.lengthOverTime);


		pointsDistributionOverLength = new SerializedAnimationCurve(leaf.pointsDistributionOverLength);
		pointsDistributionOverTime = new SerializedAnimationCurve(leaf.pointsDistributionOverTime);

		topDownShape = new SerializedAnimationCurve(leaf.topDownShape);

		coronalVariations = leaf.coronalVariations;
		coronalCurvatureIntensity = leaf.coronalCurvatureIntensity;
		coronalCurvature = new SerializedAnimationCurve(leaf.coronalCurvature);
		coronalCurvatureIntensityOverLength = new SerializedAnimationCurve(leaf.coronalCurvatureIntensityOverLength);
		coronalCurvatureIntensityOverTime = new SerializedAnimationCurve(leaf.coronalCurvatureIntensityOverTime);


		sagittalVariations = leaf.sagittalVariations;
		sagittalCurvatureIntensity = leaf.sagittalCurvatureIntensity;
		sagittalCurvature = new SerializedAnimationCurve(leaf.sagittalCurvature);
		sagittalCurvatureIntensityOverLength = new SerializedAnimationCurve(leaf.sagittalCurvatureIntensityOverLength);
		sagittalCurvatureIntensityOverTime = new SerializedAnimationCurve(leaf.sagittalCurvatureIntensityOverTime);
		
	}
	/*
	public void DeserializedLeaf(Leaf leaf)
	{
		leaf.time = time;

		leaf.nbOfTangentSegments = nbOfTangentSegments;
		leaf.nbOfNormalSegments = nbOfNormalSegments;


		leaf.initialDirection = initialDirection;
		leaf.upDirection = upDirection;

		leaf.width = width;
		leaf.length = length;


		leaf.widthOverTime = widthOverTime;
		leaf.lengthOverTime = lengthOverTime;


		leaf.pointsDistributionOverLength = pointsDistributionOverLength;
		leaf.pointsDistributionOverTime = pointsDistributionOverTime;

		leaf.topDownShape = topDownShape;

		leaf.coronalVariations = coronalVariations;
		leaf.coronalCurvatureIntensity = coronalCurvatureIntensity;
		leaf.coronalCurvature = coronalCurvature;
		leaf.coronalCurvatureIntensityOverLength = coronalCurvatureIntensityOverLength;
		leaf.coronalCurvatureIntensityOverTime = coronalCurvatureIntensityOverTime;


		leaf.sagittalVariations = sagittalVariations;
		leaf.sagittalCurvatureIntensity = sagittalCurvatureIntensity;
		leaf.sagittalCurvature = sagittalCurvature;
		leaf.sagittalCurvatureIntensityOverLength = sagittalCurvatureIntensityOverLength;
		leaf.sagittalCurvatureIntensityOverTime = sagittalCurvatureIntensityOverTime;
		
	}
	*/

}

 