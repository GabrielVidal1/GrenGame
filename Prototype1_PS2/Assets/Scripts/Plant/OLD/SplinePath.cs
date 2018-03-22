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


	public static Vector3[][] GenerateTrajectory(Vector3 initialPosition, Vector3 initialDirection, int arrayLength)
	{
		//PARAMETERS

		float amplitude = 5f;

		float frequency = 10f;


		Vector3[][] positionsNormals = new Vector3[2][];

		positionsNormals [0] = new Vector3[arrayLength + 1]; //POSITIONS
		positionsNormals [1] = new Vector3[arrayLength]; //NORMALS

		Vector3 position = initialPosition;

		positionsNormals [0] [0] = initialDirection;

		for (int i = 0; i < arrayLength; i++) {

			Vector3 newDirection = (positionsNormals[0] [i] + Noise3D.PerlinNoise3D (frequency * position)).normalized;

			positionsNormals[0][i + 1] = newDirection;
			positionsNormals [1] [i] = Vector3.up;
			position += newDirection;

			//Debug.Log (position);

		}



		return positionsNormals;

	}


	public float Evaluate(float t)
	{







		throw new UnityException ("Not implemented yet");
	}


}
