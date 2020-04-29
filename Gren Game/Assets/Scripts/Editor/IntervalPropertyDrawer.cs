using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(Interval))]
public class IntervalPropertyDrawer : PropertyDrawer {

	public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
	{

		position.width /= 3f;

		EditorGUI.LabelField (position, property.displayName);
		position.x += position.width;

		Rect subPos2 = new Rect (position.x + position.width * 0.05f, position.y, position.width * 0.20f, position.height);
		Rect subPos1 = new Rect (position.x + position.width * 0.25f, position.y, position.width * 0.70f, position.height);

		//EditorGUI.DrawRect (subPos2, Color.red);

		EditorGUI.LabelField (subPos2, "Min");
		EditorGUI.PropertyField (subPos1, property.FindPropertyRelative ("min"), GUIContent.none);

		subPos1.x += position.width;
		subPos2.x += position.width;

		position.x += position.width;
		EditorGUI.LabelField (subPos2, "Max");
		EditorGUI.PropertyField (subPos1, property.FindPropertyRelative ("max"), GUIContent.none);


	}

	public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
	{
		return 16f;
	}
}
