using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionCurve : MonoBehaviour {

	public float gravityForce;

	public AnimationCurve gravityOverLength;

	public float initialSegmentLength;
	public float finalSegmentLength;


	public AnimationCurve segmentLengthOverLength;


	public float noiseForce;

	[Range(0.01f, 100f)]
	public float noiseSize;

	public AnimationCurve noiseOverLength;


	public PositionsAndNormals GenerateTrajectory(Vector3 initialPosition, Vector3 initialDirection, int arrayLength)
	{

		float rayDuration = 10f;


		//PARAMETERS

		initialDirection.Normalize ();

		PositionsAndNormals positionsNormals = new PositionsAndNormals (arrayLength);

		Vector3 currentDirection = initialDirection;
		Vector3 currentPosition = initialPosition;
		Vector3 currentNormal = Vector3.Cross(initialDirection, new Vector3(0.21f, 0.656f, 0.254f));

		Vector3 prevPosition = initialPosition;

		positionsNormals.pos [0] = initialPosition;
		positionsNormals.nor [0] = currentNormal;


		for (int i = 1; i < arrayLength; i++) {


			float lengthRatio = i / (float)arrayLength;

			//DIRECTION CALCULATION

			Vector3 gravity = Vector3.down * gravityForce * gravityOverLength.Evaluate (lengthRatio);

			Vector3 noise = noiseForce * noiseOverLength.Evaluate (lengthRatio) * Noise3D.PerlinNoise3D (currentPosition / noiseSize);

			float distance = Mathf.Lerp (initialSegmentLength, finalSegmentLength, segmentLengthOverLength.Evaluate (lengthRatio));

			currentDirection = distance * (currentDirection + gravity + noise).normalized;

			PosDir pd = Collision(new PosDir(currentPosition, currentDirection, currentNormal), distance);

			for (int k = 0; k < 10; k++) 
			{
				pd = Collision(new PosDir(currentPosition, pd.dir, pd.normal), distance);
				
			}



			currentPosition = pd.pos;
			currentDirection = pd.dir;

			positionsNormals.pos [i] = currentPosition;
			positionsNormals.nor [i] = pd.normal;

			prevPosition = currentPosition;

		}

		return positionsNormals;

	}

	PosDir Collision(PosDir pd, float distance)
	{



		Ray ray = new Ray (pd.pos, pd.dir);

		RaycastHit hit;
		if (Physics.Raycast (ray, out hit, distance)) {

			//print (hit.collider.name);


			//CROSS PRODUCT - DIRECTION AJUSTEMENT

			pd.normal = 0.25f * hit.normal + 0.75f * pd.normal;

			float d = Vector3.Dot (hit.normal.normalized, hit.point - pd.pos);

			Vector3 H = pd.pos + d * hit.normal;

			pd.dir = (hit.point - H).normalized * distance;

			float dist = Vector3.Distance (pd.pos, hit.point);

			float dd = Mathf.Sqrt (distance * distance - d * d);

			pd.pos = (hit.point - H).normalized * dd + H + 0.05f * hit.normal;

		} else {

			pd.pos += pd.dir;

		}

		return pd;

	}


}
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