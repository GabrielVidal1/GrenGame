#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Plant))]
public class PlantEditor : Editor {


	private bool displayLeafParameters;

	private Editor leafEditor;

	public override void OnInspectorGUI ()
	{
		Plant myObject = (Plant)target;


		EditorGUI.BeginChangeCheck ();


		//base.OnInspectorGUI ();
		//EditorGUILayout.Space ();

		myObject.time = EditorGUILayout.Slider ("Time", myObject.time, 0f, 1f, GUILayout.Height (20f));

		EditorGUILayout.Space ();

		EditorGUILayout.LabelField ("Global Characteristics", EditorStyles.toolbarButton);

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


		EditorGUILayout.LabelField ("Mesh Characteristics", EditorStyles.toolbarButton);
		myObject.nbOfSegments = EditorGUILayout.IntSlider ("Number Of Segments", myObject.nbOfSegments, 1, 2000);
		myObject.nbOfSides = EditorGUILayout.IntSlider ("Number Of Sides", myObject.nbOfSides, 3, 64);



		EditorGUILayout.LabelField ("Initial Condition", EditorStyles.toolbarButton);

		myObject.initialRadius = EditorGUILayout.FloatField ("Initial Radius", myObject.initialRadius);
		if (myObject.initialRadius < 0f) {
			Debug.LogError ("The Radius Can't Be Null Or Negative!");
			myObject.initialRadius = 0.01f;
		}

		myObject.initalDirection = EditorGUILayout.Vector3Field ("Initial Direction", myObject.initalDirection);

		if (myObject.initalDirection == Vector3.zero) {
			Debug.LogError ("The Initial Direction Can't Be Vector3.zero!");
			myObject.initalDirection = Vector3.up;
		}

		EditorGUILayout.Space ();
		EditorGUILayout.LabelField ("UV Mapping", EditorStyles.toolbarButton);

		myObject.sectionsPerUvUnit = EditorGUILayout.IntSlider ("Section Per Unit", myObject.sectionsPerUvUnit, 1, 20);



		EditorGUILayout.Space ();
		EditorGUILayout.LabelField ("Shape", EditorStyles.toolbarButton);

		myObject.initialShapeOverLength = EditorGUILayout.CurveField("Initial Shape Over Length", myObject.initialShapeOverLength, Color.green, new Rect(0, 0, 1, 2));
		if (myObject.initialShapeOverLength == null)
			myObject.initialShapeOverLength = AnimationCurve.Linear (0f, 0f, 1f, 1f);

		myObject.finalShapeOverLength = EditorGUILayout.CurveField("Final Shape Over Length", myObject.finalShapeOverLength, Color.green, new Rect(0, 0, 1, 2));
		if (myObject.finalShapeOverLength == null)
			myObject.finalShapeOverLength = AnimationCurve.Linear (0f, 0f, 1f, 1f);

		myObject.shapeOverTime = EditorGUILayout.CurveField("Shape Transition Over Length", myObject.shapeOverTime, Color.green, new Rect(0, 0, 1, 1));
		if (myObject.shapeOverTime == null)
			myObject.shapeOverTime = AnimationCurve.Linear (0f, 0f, 1f, 1f);

		EditorGUILayout.Space ();

		//EditorGUILayout.LabelField ("Variation Over Time", EditorStyles.toolbarButton);

		EditorGUILayout.LabelField ("Length", EditorStyles.toolbarButton);

		myObject.lengthOverTime = EditorGUILayout.CurveField("Length Over Length", myObject.lengthOverTime, Color.green, new Rect(0, 0, 1, 1));
		if (myObject.lengthOverTime == null)
			myObject.lengthOverTime = AnimationCurve.Linear (0f, 0f, 1f, 1f);

		//EditorGUILayout.LabelField ("Segment Length", EditorStyles.toolbarButton);

		myObject.initialSegmentLength = EditorGUILayout.Slider("Initial Segment Length", myObject.initialSegmentLength, 0.01f, 10f);
		myObject.finalSegmentLength = EditorGUILayout.Slider("Final Segment Length", myObject.finalSegmentLength, 0.01f, 10f);

		myObject.segmentLengthOverLength = EditorGUILayout.CurveField("Segment Length Over Length", myObject.segmentLengthOverLength, Color.green, new Rect(0, 0, 1, 1));
		if (myObject.segmentLengthOverLength == null)
			myObject.segmentLengthOverLength = AnimationCurve.Linear (0f, 0f, 1f, 1f);


		EditorGUILayout.Space ();

		EditorGUILayout.LabelField ("External Forces", EditorStyles.toolbarButton);


		myObject.hasGravity = EditorGUILayout.Toggle ("Gravity", myObject.hasGravity);
		if (myObject.hasGravity) {

			myObject.gravityForce = EditorGUILayout.FloatField ("Gravity Intensity", myObject.gravityForce);

			myObject.gravityOverLength = EditorGUILayout.CurveField("Gravity Intensity Over Length", myObject.gravityOverLength, Color.green, new Rect(0, 1, -1, 1));
			if (myObject.gravityOverLength == null)
				myObject.gravityOverLength = AnimationCurve.Linear (0f, 0f, 1f, 1f);

			EditorGUILayout.Space ();
		}


		myObject.hasNoise = EditorGUILayout.Toggle ("Trajectory Noise", myObject.hasNoise);
		if (myObject.hasNoise) {

			myObject.noiseForce = EditorGUILayout.Slider ("Noise Intensity", myObject.noiseForce, 0.01f, 10f);
			myObject.noiseSize = EditorGUILayout.Slider ("Noise Size", myObject.noiseSize, 0.01f, 10f);

			myObject.noiseOverLength = EditorGUILayout.CurveField("Noise Intensity Over Length", myObject.noiseOverLength, Color.green, new Rect(0, 0, 1, 1));
			if (myObject.noiseOverLength == null)
				myObject.noiseOverLength = AnimationCurve.Linear (0f, 0f, 1f, 1f);

			EditorGUILayout.Space ();

		}


		EditorGUILayout.LabelField ("Leaves", EditorStyles.toolbarButton);


		myObject.hasLeaves = EditorGUILayout.Toggle ("Leaves", myObject.hasLeaves);
		if (myObject.hasLeaves) {

			myObject.leavesIteration = EditorGUILayout.IntSlider ("Number of Leaves", myObject.leavesIteration, 0, 200);

			myObject.leavesOnlyOnSections = EditorGUILayout.Toggle ("Leaves Only On Sections", myObject.leavesOnlyOnSections);

			myObject.leavesDistribution = EditorGUILayout.CurveField ("Leaves Distribution Over Length", myObject.leavesDistribution, Color.green, new Rect (0, 0, 1, 1));
			if (myObject.leavesDistribution == null)
				myObject.leavesDistribution = AnimationCurve.Linear (0f, 0f, 1f, 1f);


			EditorGUILayout.BeginHorizontal ();
			myObject.leafMinSize = EditorGUILayout.Slider ("Leaf Min Size", myObject.leafMinSize, 0f, 1f); 
			myObject.leafMaxSize = EditorGUILayout.Slider ("Leaf Max Size", myObject.leafMaxSize, 0f, 1f); 
			EditorGUILayout.EndHorizontal ();


			myObject.leafGrowthOverTime = EditorGUILayout.CurveField ("Leaves Growth Over Time", myObject.leafGrowthOverTime, Color.green, new Rect (0, 0, 1, 1));
			if (myObject.leafGrowthOverTime == null)
				myObject.leafGrowthOverTime = AnimationCurve.Linear (0f, 0f, 1f, 1f);

			myObject.sunOrientationIntensity = EditorGUILayout.Slider ("Sun Orientation Coef", myObject.sunOrientationIntensity, 0f, 1f);

			myObject.leafGrowthDurationRatio = EditorGUILayout.Slider ("Leaf Growth Duration", myObject.leafGrowthDurationRatio, 0f, 1f);

			myObject.sunOrientationIntensityOverTime = EditorGUILayout.CurveField ("Sun Orientation Coef Over Time", myObject.sunOrientationIntensityOverTime, Color.green, new Rect (0, 0, 1, 1));
			if (myObject.sunOrientationIntensityOverTime == null)
				myObject.sunOrientationIntensityOverTime = AnimationCurve.Linear (0f, 0f, 1f, 1f);

			EditorGUILayout.Space ();

			myObject.leafPrefab = EditorGUILayout.ObjectField ("Leaf Prefab", myObject.leafPrefab, typeof(Leaf), false) as Leaf;

			if (myObject.leafPrefab != null) {

				displayLeafParameters = EditorGUILayout.Foldout (displayLeafParameters, "Leaf Prefab Informations");
				if (displayLeafParameters) {

					EditorGUI.indentLevel++;
	
					if (leafEditor == null)
						leafEditor = CreateEditor (myObject.leafPrefab);
				
					//leafEditor.DrawDefaultInspector ();

					EditorGUI.indentLevel--;
				}
				EditorGUILayout.Space ();
			}
		}

		EditorGUILayout.LabelField ("Recursion", EditorStyles.toolbarButton);

		myObject.hasRecursions = EditorGUILayout.Toggle ("Has Recursions", myObject.hasRecursions);
		if (myObject.hasRecursions) {





		}

		if (EditorGUI.EndChangeCheck ()) {
			myObject.Initialize ();
		}




		EditorGUILayout.Separator ();

		if (GUILayout.Button("Initialize", GUILayout.Height(32f)))
			myObject.Initialize ();

		if (GUILayout.Button("Update", GUILayout.Height(32f)))
			myObject.UpdateMesh ();

		if (GUILayout.Button ("Destroy Leaves", GUILayout.Height (32f))) {

			for (int i = 0; i < myObject.transform.childCount; i++) {
				DestroyImmediate (myObject.transform.GetChild (i).gameObject);
			}


		}
	}
}


enum BackgroundColor
{
	Black,
	White,
	CustomColor
}
#endif