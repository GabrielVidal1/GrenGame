using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Noise3D : MonoBehaviour {

	public static Vector3 PerlinNoise3D(Vector3 position)
	{

		return new Vector3 (
			2*Mathf.PerlinNoise (position.x, position.y)-1,
			2*Mathf.PerlinNoise (position.y, position.z)-1,
			2*Mathf.PerlinNoise (position.z, position.x)-1
		);
	}

	public static float PerlinNoise(Vector3 position)
	{
		return Mathf.PerlinNoise (position.x - position.z, position.y + position.z);
	}

}
