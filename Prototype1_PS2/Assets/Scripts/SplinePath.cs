using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplinePath {



	public List<Vector3> points;

	public float totalLength;



	public float CalculateTotalLength()
	{





		throw new UnityException ("Not implemented yet");


	}


	public static Vector3[] GenerateTrajectory(Vector3 initialPosition, Vector3 initialDirection, int arrayLength)
	{
		//PARAMETERS

		float amplitude = 5f;

		float frequency = 10f;


		List<Vector3> vectors = new List<Vector3> ();

		vectors.Add (initialDirection);

		Vector3 position = initialPosition;


		for (int i = 0; i < arrayLength; i++) {

			Vector3 newDirection = (vectors [i] + Noise3D.PerlinNoise3D (frequency * position)).normalized;

			vectors.Add (newDirection);
			position += newDirection;

			//Debug.Log (position);

		}



		return vectors.ToArray ();

	}


	public float Evaluate(float t)
	{







		throw new UnityException ("Not implemented yet");
	}


}
