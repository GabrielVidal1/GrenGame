using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Interval {

	public float min;
	public float max;

	public Interval(float min, float max)
	{
		this.max = max;
		this.min = min;
	}

	public float RandomValue()
	{
		return Mathf.Lerp (min, max, Random.value);
	}
}


