using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moss : MonoBehaviour {

	[Range(0f, 1f)]
	public float time;

	public float finalRadius;

	public float textureSize;

	public AnimationCurve radiusOverTime;

	private MeshRenderer mr;

	private bool init;

	public void UpdateMoss()
	{
		if (!init)
			Init ();
		
		float radius = finalRadius * radiusOverTime.Evaluate (time);

		transform.localScale = Vector3.one * radius;

		mr.material.mainTextureScale = Vector2.one * radius / textureSize;
		mr.material.mainTextureOffset = (- Vector2.one * radius * 0.5f / textureSize) + 0.5f * Vector2.one;

	}

	public void Init()
	{
		init = true;
		mr = GetComponent<MeshRenderer> ();
	}

	// Update is called once per frame
	void Update () {
		
	}
}
