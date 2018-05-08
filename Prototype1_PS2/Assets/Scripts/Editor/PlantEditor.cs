#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Plant))]
public class PlantEditor : Editor {


	private bool displayLeafParameters;
	private bool displayFlowerParameters;
	private bool displayBranchParameters;

	private Editor leafEditor;
	private Editor flowerEditor;
	private Editor branchEditor;

	SerializedProperty flowerGrowthDuration;
	SerializedProperty leafGrowthDuration;
	SerializedProperty branchGrowthDuration;
	SerializedProperty branchInitialRadiusMultiplier;
	SerializedProperty brancheLengthRatio;
	SerializedProperty trunkGrowthDuration;
	SerializedProperty leafSize;
	SerializedProperty flowerSize;

	SerializedProperty fruitSize;
	SerializedProperty fruitGrowthDuration;

	SerializedProperty fruitPrefab;


	Rect brancheRect;

	Vector2 scrollZone;

	bool updateOnChange;

	void OnEnable()
	{
		leafGrowthDuration = serializedObject.FindProperty ("leafGrowthDuration");
		flowerGrowthDuration = serializedObject.FindProperty ("flowerGrowthDuration");
		branchGrowthDuration = serializedObject.FindProperty ("branchGrowthDuration");
		branchInitialRadiusMultiplier = serializedObject.FindProperty ("branchInitialRadiusMultiplier");
		brancheLengthRatio = serializedObject.FindProperty ("brancheLengthRatio");
		trunkGrowthDuration = serializedObject.FindProperty ("trunkGrowthDuration");
		leafSize = serializedObject.FindProperty ("leafSize");
		flowerSize = serializedObject.FindProperty ("flowerSize");

		fruitSize = serializedObject.FindProperty ("fruitSize");
		fruitGrowthDuration = serializedObject.FindProperty ("fruitGrowthDuration");
		fruitPrefab = serializedObject.FindProperty ("fruitPrefab");
	}

	public override void OnInspectorGUI ()
	{

		EditorGUILayout.Space ();

		EditorGUI.BeginChangeCheck ();


		Plant myObject = (Plant)target;



		if (!myObject.isBranch) {

			EditorGUILayout.LabelField ("Index In Plant Manager", myObject.plantTypeIndex.ToString());

			//scrollZone = EditorGUILayout.BeginScrollView (scrollZone);
			myObject.pointValue = EditorGUILayout.IntField ("Point Value", myObject.pointValue);
		}

		EditorGUILayout.LabelField ("Seed", myObject.plantSeed.ToString ());

		//myObject.isBranch = EditorGUILayout.Toggle ("Is Branch", myObject.isBranch);

		if (!myObject.isBranch) {
			myObject.time = EditorGUILayout.Slider ("Time", myObject.time, 0f, 1f, GUILayout.Height (20f));

			EditorGUILayout.PropertyField (trunkGrowthDuration, new GUIContent ("Trunk Growth Duration"), true);

			EditorGUILayout.Space ();
		}

		EditorGUILayout.LabelField ("Global Characteristics", EditorStyles.toolbarButton);

		//COLLISION
		myObject.hasCollisions = EditorGUILayout.Toggle ("Has Collisions", myObject.hasCollisions);

		//TANGENTS
		serializedObject.FindProperty("offsetTangents").boolValue = EditorGUILayout.Toggle("Offset Tangents", serializedObject.FindProperty("offsetTangents").boolValue);

		//SMOOTH
		myObject.smooth = EditorGUILayout.Toggle ("Smoothing", myObject.smooth);
		if (myObject.smooth) {
			myObject.smoothCoef = EditorGUILayout.Slider ("Smooth Coef", myObject.smoothCoef, 0f, 1f);
			//EditorGUILayout.Separator ();
		}

		//NOISE
		myObject.noise = EditorGUILayout.Toggle ("Noising", myObject.noise);
		if (myObject.noise) {
			myObject.noiseCoef = EditorGUILayout.Slider ("Noise Coef", myObject.noiseCoef, 0f, 1f);
			//EditorGUILayout.Separator ();
		}

		myObject.initialForce = EditorGUILayout.Toggle ("Initial Force", myObject.initialForce);
		if (myObject.initialForce) {
			myObject.initialForceCoef = EditorGUILayout.Slider ("Initial Force Coef", myObject.initialForceCoef, 0f, 1f);
			//EditorGUILayout.Separator ();
		}

		//INITIALISATION AFTER CHANGING SEGMENTS OR TRIANGLES


		EditorGUILayout.LabelField ("Mesh Characteristics", EditorStyles.toolbarButton);



		if (myObject.isBranch) {
			myObject.brancheIndependentLength = EditorGUILayout.Toggle ("Independent length", myObject.brancheIndependentLength);
		}
		EditorGUI.BeginDisabledGroup (myObject.isBranch && !myObject.brancheIndependentLength);
		myObject.nbOfSegments = EditorGUILayout.IntSlider ("Number Of Segments", myObject.nbOfSegments, 2, 500);
		EditorGUI.EndDisabledGroup ();

		myObject.nbOfSides = EditorGUILayout.IntSlider ("Number Of Sides", myObject.nbOfSides, 3, 64);



		EditorGUILayout.LabelField ("Initial Condition", EditorStyles.toolbarButton);

		if (myObject.isBranch) {
			myObject.brancheIndependentRadius = EditorGUILayout.Toggle ("Independent Radius", myObject.brancheIndependentRadius);
		}
		EditorGUI.BeginDisabledGroup (myObject.isBranch && !myObject.brancheIndependentRadius);

		myObject.initialRadius = EditorGUILayout.FloatField ("Initial Radius", myObject.initialRadius);
		if (myObject.initialRadius <= 0f) {
			Debug.LogError ("The Radius Can't Be Null Or Negative!");
			myObject.initialRadius = 0.01f;
		}

		myObject.initialDirection = EditorGUILayout.Vector3Field ("Initial Direction", myObject.initialDirection);

		if (myObject.initialDirection == Vector3.zero) {
			Debug.LogError ("The Initial Direction Can't Be Vector3.zero!");
			myObject.initialDirection = Vector3.up;
		}

		EditorGUI.EndDisabledGroup ();

		EditorGUILayout.Space ();
		EditorGUILayout.LabelField ("UV Mapping", EditorStyles.toolbarButton);

		myObject.sectionsPerUvUnit = EditorGUILayout.IntSlider ("Section Per Unit", myObject.sectionsPerUvUnit, 1, 20);



		EditorGUILayout.Space ();
		EditorGUILayout.LabelField ("Shape", EditorStyles.toolbarButton);

		myObject.initialShapeOverLength = EditorGUILayout.CurveField ("Initial Shape Over Length", myObject.initialShapeOverLength, Color.green, new Rect (0, 0, 1, 2));
		if (myObject.initialShapeOverLength == null) {
			myObject.initialShapeOverLength = AnimationCurve.Linear (0f, 1f, 1f, 0f);
		}

		myObject.finalShapeOverLength = EditorGUILayout.CurveField ("Final Shape Over Length", myObject.finalShapeOverLength, Color.green, new Rect (0, 0, 1, 2));
		if (myObject.finalShapeOverLength == null) {
			myObject.finalShapeOverLength = AnimationCurve.Linear (0f, 1f, 1f, 0f);
		}

		myObject.shapeOverTime = EditorGUILayout.CurveField ("Shape Transition Over Length", myObject.shapeOverTime, Color.green, new Rect (0, 0, 1, 1));
		if (myObject.shapeOverTime == null) {
			myObject.shapeOverTime = AnimationCurve.Linear (0f, 0f, 1f, 1f);
		}

		EditorGUILayout.Space ();

		serializedObject.FindProperty("radiusOverAngle").animationCurveValue = EditorGUILayout.CurveField ("Radius Over Angle", serializedObject.FindProperty("radiusOverAngle").animationCurveValue, Color.green, new Rect (0, 0, 1, 1));
		if (serializedObject.FindProperty("radiusOverAngle").animationCurveValue == null) {
			serializedObject.FindProperty("radiusOverAngle").animationCurveValue = AnimationCurve.Linear (0f, 1f, 1f, 1f);
		}

		serializedObject.ApplyModifiedProperties ();

		EditorGUILayout.Space ();

		//EditorGUILayout.LabelField ("Variation Over Time", EditorStyles.toolbarButton);

		EditorGUILayout.LabelField ("Length", EditorStyles.toolbarButton);

		myObject.lengthOverTime = EditorGUILayout.CurveField ("Length Over Time", myObject.lengthOverTime, Color.green, new Rect (0, 0, 1, 1));
		if (myObject.lengthOverTime == null) {
			myObject.lengthOverTime = AnimationCurve.Linear (0f, 0f, 1f, 1f);
		}



		//EditorGUI.BeginDisabledGroup (myObject.isBranch || myObject.branchForceParameters);
		myObject.initialSegmentLength = EditorGUILayout.Slider ("Initial Segment Length", myObject.initialSegmentLength, 0.01f, 10f);
		myObject.finalSegmentLength = EditorGUILayout.Slider ("Final Segment Length", myObject.finalSegmentLength, 0.01f, 10f);
		//EditorGUI.EndDisabledGroup ();

		myObject.segmentLengthOverLength = EditorGUILayout.CurveField ("Segment Length Over Length", myObject.segmentLengthOverLength, Color.green, new Rect (0, 0, 1, 1));
		if (myObject.segmentLengthOverLength == null) {
			myObject.segmentLengthOverLength = AnimationCurve.Linear (0f, 1f, 1f, 1f);
		}


		EditorGUILayout.Space ();

		EditorGUILayout.LabelField ("External Forces", EditorStyles.toolbarButton);


		myObject.hasGravity = EditorGUILayout.Toggle ("Gravity", myObject.hasGravity);
		if (myObject.hasGravity) {

			myObject.gravityForce = EditorGUILayout.Slider ("Gravity Intensity", myObject.gravityForce, -5f, 5f);

			myObject.gravityOverLength = EditorGUILayout.CurveField ("Gravity Intensity Over Length", myObject.gravityOverLength, Color.green, new Rect (0, 0, 1, 1));
			if (myObject.gravityOverLength == null) {
				myObject.gravityOverLength = AnimationCurve.Linear (0f, 1f, 1f, 1f);
			}

			EditorGUILayout.Space ();
		}


		myObject.hasNoise = EditorGUILayout.Toggle ("Trajectory Noise", myObject.hasNoise);
		if (myObject.hasNoise) {

			myObject.noiseForce = EditorGUILayout.Slider ("Noise Intensity", myObject.noiseForce, 0f, 5f);
			myObject.noiseSize = EditorGUILayout.Slider ("Noise Size", myObject.noiseSize, 0.01f, 1f);

			myObject.noiseOverLength = EditorGUILayout.CurveField ("Noise Intensity Over Length", myObject.noiseOverLength, Color.green, new Rect (0, 0, 1, 1));
			if (myObject.noiseOverLength == null) {
				myObject.noiseOverLength = AnimationCurve.Linear (0f, 1f, 1f, 1f);
			}

			EditorGUILayout.Space ();

		}

		//myObject.hasCollisions = EditorGUILayout.Toggle ("Has Collisions", myObject.hasCollisions);

		EditorGUILayout.LabelField ("Leaves", EditorStyles.toolbarButton);
		myObject.hasLeaves = EditorGUILayout.Toggle ("Has Leaves", myObject.hasLeaves);
		
		if (myObject.hasLeaves) {

			EditorGUILayout.PropertyField (serializedObject.FindProperty ("leafPrefab"), true);
			//myObject.leafPrefab = EditorGUILayout.ObjectField ("Leaf Prefab", myObject.leafPrefab, typeof(Leaf), true) as Leaf;


			if (myObject.leafPrefab != null) {

				myObject.leavesRepartitionMode = (LeavesRepartitionMode)EditorGUILayout.EnumPopup ("Leaves Repartition Mode", myObject.leavesRepartitionMode);

				if (myObject.leavesRepartitionMode == LeavesRepartitionMode.Density)
					myObject.leavesDensity = EditorGUILayout.Slider ("Leaf Density", myObject.leavesDensity, 0f, 10f);
				if (myObject.leavesRepartitionMode == LeavesRepartitionMode.Number)
					myObject.leavesNumber = EditorGUILayout.IntSlider ("Leaf Number", myObject.leavesNumber, 0, 100);

				EditorGUILayout.PropertyField (leafGrowthDuration, new GUIContent ("Leaf Growth Duration"), true);
				EditorGUILayout.PropertyField (leafSize, new GUIContent ("Leaf Size"), true);
				myObject.leafGrowthOverTime = EditorGUILayout.CurveField ("Leaves Growth Over Time", myObject.leafGrowthOverTime, Color.green, new Rect (0, 0, 1, 1));


				myObject.leavesBirthdateDistribution = EditorGUILayout.CurveField ("Leaves Birth Distribution Over Time", myObject.leavesBirthdateDistribution, Color.green, new Rect (0, 0, 1, 1));
				if (myObject.leavesBirthdateDistribution == null)
					myObject.leavesBirthdateDistribution = AnimationCurve.Linear (0, 0, 1, 1);

				myObject.leavesOnlyOnSections = EditorGUILayout.Toggle ("Leaves Only On Sections", myObject.leavesOnlyOnSections);
				myObject.leavesDistribution = EditorGUILayout.CurveField ("Leaves Distribution Over Length", myObject.leavesDistribution, Color.green, new Rect (0, 0, 1, 1));


				EditorGUILayout.Space ();

				myObject.sunOrientationIntensity = EditorGUILayout.Slider ("Sun Orientation Coef", myObject.sunOrientationIntensity, 0f, 1f);
				myObject.sunOrientationIntensityOverTime = EditorGUILayout.CurveField ("Sun Orientation Coef Over Time", myObject.sunOrientationIntensityOverTime, Color.green, new Rect (0, 0, 1, 1));

				EditorGUILayout.Space ();


				if (myObject.leafPrefab != null) {

					displayLeafParameters = EditorGUILayout.Foldout (displayLeafParameters, "Leaf Prefab Informations");
					if (displayLeafParameters) {

						EditorGUI.indentLevel++;
	
						if (leafEditor == null)
							leafEditor = CreateEditor (myObject.leafPrefab);
				
						leafEditor.DrawDefaultInspector ();

						EditorGUI.indentLevel--;
					}
					EditorGUILayout.Space ();
				}

			}
		}

		EditorGUILayout.LabelField ("Flowers", EditorStyles.toolbarButton);
		myObject.hasFlowers = EditorGUILayout.Toggle ("Has Flowers", myObject.hasFlowers);

		if (myObject.hasFlowers) {

			EditorGUILayout.PropertyField (serializedObject.FindProperty ("flowerPrefab"), true);
			//myObject.flowerPrefab = EditorGUILayout.ObjectField ("Flower Prefab", myObject.flowerPrefab, typeof(Flower), true) as Flower;



			if (myObject.flowerPrefab != null) {

				myObject.uniqueEndFlower = EditorGUILayout.Toggle ("Unique Ending Flower", myObject.uniqueEndFlower);


				EditorGUILayout.PropertyField (flowerGrowthDuration, new GUIContent ("Flower Growth Duration"), true);
				EditorGUILayout.PropertyField (flowerSize, new GUIContent ("Flower Size"), true);
				myObject.flowerGrowthOverTime = EditorGUILayout.CurveField ("Flower Growth Over Time", myObject.flowerGrowthOverTime, Color.green, new Rect (0, 0, 1, 1));
				myObject.flowersBirthdateDistribution = EditorGUILayout.CurveField ("Flowers Birth Distribution Over Time", myObject.flowersBirthdateDistribution, Color.green, new Rect (0, 0, 1, 1));
				if (myObject.flowersBirthdateDistribution == null)
					myObject.flowersBirthdateDistribution = AnimationCurve.Linear (0, 0, 1, 1);
				
				//EditorGUI.BeginDisabledGroup (myObject.uniqueEndFlower);

				myObject.hasFixedDirection = EditorGUILayout.Toggle ("Has Fixed Direction", myObject.hasFixedDirection);

				if (myObject.hasLeaves) {
					myObject.leafChanceOfBeingFlower = EditorGUILayout.Slider ("Leaf Chance Of Being A Flower", myObject.leafChanceOfBeingFlower, 0f, 1f);
				} else {
					myObject.leafChanceOfBeingFlower = 1f;

					myObject.leavesDistribution = EditorGUILayout.CurveField ("Flowers Distribution Over Length", myObject.leavesDistribution, Color.green, new Rect (0, 0, 1, 1));

					myObject.leavesRepartitionMode = (LeavesRepartitionMode)EditorGUILayout.EnumPopup ("Flower Repartition Mode", myObject.leavesRepartitionMode);

					if (myObject.leavesRepartitionMode == LeavesRepartitionMode.Density)
						myObject.leavesDensity = EditorGUILayout.Slider ("Flower Density", myObject.leavesDensity, 0f, 10f);
					if (myObject.leavesRepartitionMode == LeavesRepartitionMode.Number)
						myObject.leavesNumber = EditorGUILayout.IntSlider ("Flower Number", myObject.leavesNumber, 0, 100);

				}


				//EditorGUI.EndDisabledGroup ();


				if (myObject.leafPrefab != null) {

					displayFlowerParameters = EditorGUILayout.Foldout (displayFlowerParameters, "Flower Prefab Informations");
					if (displayFlowerParameters) {

						EditorGUI.indentLevel++;

						if (flowerEditor == null)
							flowerEditor = CreateEditor (myObject.flowerPrefab);

						flowerEditor.DrawDefaultInspector ();

						EditorGUI.indentLevel--;
					}
					EditorGUILayout.Space ();
				}

			}
		}

		EditorGUILayout.LabelField ("Fruits", EditorStyles.toolbarButton);

		myObject.hasFruits = EditorGUILayout.Toggle ("Has Fruits", myObject.hasFruits);

		if (myObject.hasFruits) {

			//myObject.Trunk.hasFruits = true;


			EditorGUILayout.PropertyField (fruitPrefab, true);

			myObject.fruitPrefab = (Fruit)EditorGUILayout.ObjectField ("Fruit Prefab", myObject.fruitPrefab, typeof(Fruit));

			//Debug.Log (serializedObject.FindProperty ("fruitPrefab").exposedReferenceValue);

			if (myObject.fruitNumber != null) {

				if (myObject.isBranch)
					EditorGUILayout.HelpBox ("Fruit number and repartition is controlled by the trunk", MessageType.Info);

				EditorGUI.BeginDisabledGroup (myObject.isBranch);
				myObject.fruitRepartitionMode = (LeavesRepartitionMode)EditorGUILayout.EnumPopup ("Fruits Repartition Mode", myObject.fruitRepartitionMode);
				if (myObject.fruitRepartitionMode == LeavesRepartitionMode.Density) {
					myObject.fruttiFlowersRatio = EditorGUILayout.Slider ("Flowers With Fruit Ratio", myObject.fruttiFlowersRatio, 0f, 1f);
				} else {
					myObject.fruitNumber = EditorGUILayout.IntSlider ("Fruit Number", myObject.fruitNumber, 0, 20);
				}

				EditorGUILayout.PropertyField (fruitSize);
				EditorGUILayout.PropertyField (fruitGrowthDuration);
				EditorGUI.EndDisabledGroup ();
			}
		}

		EditorGUILayout.LabelField ("Recursion", EditorStyles.toolbarButton);

		myObject.hasRecursions = EditorGUILayout.Toggle ("Has Recursions", myObject.hasRecursions);
		if (myObject.hasRecursions) {

			EditorGUILayout.PropertyField (serializedObject.FindProperty ("branchPrefab"), true);
			//myObject.branchPrefab = EditorGUILayout.ObjectField ("Branch Prefab", myObject.branchPrefab, typeof(Plant), true) as Plant;

			if (myObject.branchPrefab != null) {

				myObject.nbOfBranches = EditorGUILayout.IntSlider ("Branches Number", myObject.nbOfBranches, 0, 300);

				myObject.branchesDistribution = EditorGUILayout.CurveField ("Branches Distribution", myObject.branchesDistribution, Color.green, new Rect (0, 0, 1, 1));
				serializedObject.FindProperty ("branchesDistributionOverLength").animationCurveValue = EditorGUILayout.CurveField ("Branches Distribution Over Length", serializedObject.FindProperty ("branchesDistributionOverLength").animationCurveValue, Color.green, new Rect (0, 0, 1, 1));
				if (serializedObject.FindProperty ("branchesDistributionOverLength").animationCurveValue.keys.Length == 0) {
					serializedObject.FindProperty ("branchesDistributionOverLength").animationCurveValue = AnimationCurve.Linear (0f, 0f, 1f, 1f);
				}
				myObject.brancheAngleDelta = EditorGUILayout.Slider ("Topinal Branche Angulation", myObject.brancheAngleDelta, -2 * Mathf.PI, 2 * Mathf.PI);

				myObject.branchBirthDateDistribution = EditorGUILayout.CurveField ("Branches Birth Distribution Over Time", myObject.branchBirthDateDistribution, Color.green, new Rect (0, 0, 1, 1));

				EditorGUILayout.PropertyField (branchGrowthDuration, new GUIContent ("Branch Growth Duration"));

				EditorGUILayout.PropertyField (branchInitialRadiusMultiplier, new GUIContent ("Branch Radius Multiplier"));

				EditorGUILayout.Space ();

				myObject.branchesTangencityOverLength = EditorGUILayout.CurveField ("Branches Tangency Over Length", myObject.branchesTangencityOverLength, Color.green, new Rect (0, 0, 1, 1));

				EditorGUILayout.PropertyField (brancheLengthRatio, new GUIContent ("Branch Length Ratio"));

				if (!myObject.branchPrefab.brancheIndependentLength)
					myObject.branchLengthOverTrunkLength = EditorGUILayout.CurveField ("Branches Length Over Trunk Length", myObject.branchLengthOverTrunkLength, Color.green, new Rect (0, 0, 1, 1));

				if (myObject.branchPrefab != null) {
				
					if (myObject.branchPrefab == myObject) {
						myObject.branchPrefab = null;
						Debug.LogError ("The Branche can't be the trunk!");

					}

					myObject.branchPrefab.isBranch = true;
					//displayBranchParameters = EditorGUILayout.Foldout (displayBranchParameters, "Branch Prefab Informations");
					displayBranchParameters = true;
					if (displayBranchParameters) {


						if (branchEditor == null)
							branchEditor = CreateEditor (myObject.branchPrefab);

					
						brancheRect = EditorGUILayout.GetControlRect ();

						EditorGUI.indentLevel++;
						branchEditor.OnInspectorGUI ();

						brancheRect.yMax = EditorGUILayout.GetControlRect ().yMax;
						brancheRect.xMax = brancheRect.x + 10f * EditorGUI.indentLevel;
						EditorGUI.indentLevel--;

						EditorGUI.DrawRect (brancheRect, Color.grey);
						branchEditor.serializedObject.ApplyModifiedProperties ();

					}
					EditorGUILayout.Space ();
				}
			}




		}


		bool update = EditorGUI.EndChangeCheck ();


		if (!myObject.isBranch) {

			EditorGUILayout.Separator ();

			if (updateOnChange && update) {
				Debug.Log ("Update Plant");
				myObject.InitializePlant ();
			}
			updateOnChange = EditorGUILayout.Toggle ("Update On Change", updateOnChange);

			if (GUILayout.Button ("Initialize", GUILayout.Height (32f))) {
				Debug.Log ("Update Plant");
				myObject.InitializePlant ();
			}

			if (GUILayout.Button ("Update", GUILayout.Height (32f)))
				myObject.UpdatePlant ();

			if (GUILayout.Button ("Destroy Leaves", GUILayout.Height (32f))) {

				for (int i = 0; i < myObject.transform.childCount; i++) {
					DestroyImmediate (myObject.transform.GetChild (i).gameObject);
				}
			}

			if (GUILayout.Button ("Save plant as a file"))
				PlantSerializer.SavePlant (myObject);
		}

		serializedObject.ApplyModifiedProperties ();
	}


}





enum BackgroundColor
{
	Black,
	White,
	CustomColor
}
#endif