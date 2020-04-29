using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class Flower : MonoBehaviour {

	[Range(0f, 1f)]
	public float time;

	public float radius;
	public float Radius
	{ get { return radius; } set { radius = value; } }

	public AnimationCurve radiusOverTime;

	[Range(3, 20)]
	public int nbOfPetals;
	public int NbOfPetals
	{ get { return nbOfPetals; } set { nbOfPetals = value; } }

	public int nbOfSegments;
	public int NbOfSegments
	{ get { return nbOfSegments; } set { nbOfSegments = value; } }

	public AnimationCurve segmentPointDistribution;
	public AnimationCurve radiusOverAngle;

	public Vector3 initialDirection;
	public Vector3 InitialDirection
	{ get { return initialDirection; } set { initialDirection = value; } }

	public bool hasSideShape;
	public bool HasSideShape
	{ get { return hasSideShape; } set { hasSideShape = value; } }

	public AnimationCurve sideShape;
	public AnimationCurve SideShape
	{ get { return sideShape; } set { sideShape = value; } }

	public float sideShapeCoef;
	public float SideShapeCoef
	{ get { return sideShapeCoef; } set { sideShapeCoef = value; } }



	public AnimationCurve sideShapeOverTime;

	public bool hasClosure;
	public bool HasClosure
	{ get { return hasClosure; } set { hasClosure = value; } }

	public AnimationCurve closureForce;
	public AnimationCurve ClosureForce
	{ get { return closureForce; } set { closureForce = value; } }

	public float closureForceCoef;
	public float ClosureForceCoef
	{ get { return closureForceCoef; } set { closureForceCoef = value; } }

	public bool flipTexture;

	[Range(0f, 2f)]
	public float textureSize;

	public bool hasAFruit;
	public Fruit fruit;

	public float fruitGrowthDuration;
	public float thisGrowthDuration;

	private float fruitTimeDelay;

	private MeshFilter mf;
	private Vector3[] points;
	private int[] triangles;
	private Vector2[] uvs;

	private AnimationCurve localTimeOverTime;

	private float totalGrowthTime;

	private AnimationCurve fruitTimeOverTime;

	private Plant trunk;

	public void SetPlantRoot(Plant trunk)
	{
		this.trunk = trunk;
	}

	public void LoseFruit()
	{
		trunk.fruitSequence = trunk.fruitSequence & ~(1 << fruit.fruitIndex);
		hasAFruit = false;

	}


	//INITIALIZE THE TAB OF TRIANGLES AND POINTS
	public void Initialize()
	{
		totalGrowthTime = thisGrowthDuration + fruitGrowthDuration;

		fruitTimeDelay = fruitGrowthDuration / totalGrowthTime;

		fruitTimeOverTime = new AnimationCurve (new Keyframe[] {
			new Keyframe (0, 0),
			new Keyframe (1f - fruitTimeDelay, 0f),
			new Keyframe (1, 1)
		});

		localTimeOverTime = new AnimationCurve (new Keyframe[] {
			new Keyframe (0, 0),
			new Keyframe (1f - fruitTimeDelay, 1f),
			new Keyframe (1, 1)
		});

		mf = GetComponent<MeshFilter> ();

		//TRIANGLES
		triangles = new int[6*nbOfPetals*(1+(nbOfSegments-1)*2)];
		GenerateTriangles (triangles);

		//POINTS
		points = new  Vector3[(1 + (nbOfPetals + 1) * nbOfSegments) * 2 ];
		//GeneratePoints (points);

		//UVs
		uvs = new Vector2[points.Length];
		GenerateUVMap(uvs);

		if (hasAFruit) {
			fruit.flowerMother = this;
			fruit.time = fruitTimeOverTime.Evaluate (time);
			fruit.Initialize ();
		}

		UpdateMesh ();
	}

	public void UpdateMesh()
	{

		//UPDATE POINTS
		GeneratePoints (points);

		//MODIFY MESH
		Mesh m = mf.mesh;
		m.Clear ();

		m.vertices = points;
		m.triangles = triangles;
		m.uv = uvs;

		m.RecalculateNormals ();

		if (hasAFruit) {
			fruit.time = fruitTimeOverTime.Evaluate (time);
			if (fruit.time > 0f)
				fruit.UpdateMesh();
		}
	}

	void GeneratePoints(Vector3[] points)
	{
		points [0] = Vector3.zero;
		points [1] = Vector3.zero;

		Vector3 u = Vector3.Cross (initialDirection, Noise3D.RandomVector()).normalized;	
		Vector3 v = Vector3.Cross (u, initialDirection).normalized;
		Vector3 w = initialDirection;

		Vector3 directionNorm = initialDirection.normalized;

		float constant = 2f * Mathf.PI / (float)nbOfPetals; 

		float localTime = localTimeOverTime.Evaluate (time);

		for (int i = 0; i < nbOfPetals + 1; i++) 
		{

			float angleRatio = i * constant;

			for (int j = 0; j < nbOfSegments; j++) 
			{

				float ratio = (j + 1) / (float)nbOfSegments ;



				Vector3 dir = (Mathf.Cos (angleRatio) * u +
					Mathf.Sin (angleRatio) * v).normalized;

				dir *= radiusOverAngle.Evaluate (angleRatio / (2f * Mathf.PI));

				Vector3 point = dir * radius * segmentPointDistribution.Evaluate (ratio) * radiusOverTime.Evaluate(localTime);

				if (hasClosure)
					point -=   dir * radius * closureForce.Evaluate(ratio) * closureForceCoef;

				if (hasSideShape)
					point += w * sideShape.Evaluate (ratio) * sideShapeCoef * sideShapeOverTime.Evaluate(localTime);
					


				int index = 1 + j + i * nbOfSegments;

				points [2 * index] = point;
				points [2 * index + 1] = point;

			}
		}
	}

	void GenerateTriangles(int[] triangles)
	{
		int actual_index = 0;
		for (int i = 0; i < nbOfPetals; i++) 
		{
				for (int j = 0; j < nbOfSegments; j++) {

				int index = i * nbOfSegments + j;

				//Debug.Log (i + "*" + nbOfSegments + "+" + j + " = " + index + "        " + (index * 12 + 12));
				//Debug.Log ("actual index : " + actual_index);

				if (j == 0) {
					
					int p1 = 0;
					int p2 = nbOfSegments * i + 1;
					int p3 = nbOfSegments * (i + 1) + 1;

					triangles [actual_index] = 2 * p1;
					actual_index++;
					triangles [actual_index] = 2 * p2;
					actual_index++;
					triangles [actual_index] = 2 * p3;
					actual_index++;

					triangles [actual_index] = 2 * p1 + 1;
					actual_index++;
					triangles [actual_index] = 2 * p3 + 1;
					actual_index++;
					triangles [actual_index] = 2 * p2 + 1;
					actual_index++;
					/*
					triangles [12 * index + 0] = 2 * p1;
					triangles [12 * index + 1] = 2 * p3;
					triangles [12 * index + 2] = 2 * p2;

					triangles [12 * index + 3] = 2 * p1 + 1;
					triangles [12 * index + 4] = 2 * p2 + 1;
					triangles [12 * index + 5] = 2 * p3 + 1;
*/
				} else {


					int p1 = nbOfSegments * i + j;
					int p2 = nbOfSegments * i + (j + 1);

					int p3 = nbOfSegments * (i + 1) + j;
					int p4 = nbOfSegments * (i + 1) + (j + 1);

					triangles [actual_index] = 2 * p1;
					actual_index++;
					triangles [actual_index] = 2 * p2;
					actual_index++;
					triangles [actual_index] = 2 * p3;
					actual_index++;

					triangles [actual_index] = 2 * p2;
					actual_index++;
					triangles [actual_index] = 2 * p4;
					actual_index++;
					triangles [actual_index] = 2 * p3;
					actual_index++;

					triangles [actual_index] = 2 * p1 + 1;
					actual_index++;
					triangles [actual_index] = 2 * p3 +1;
					actual_index++;
					triangles [actual_index] = 2 * p2 + 1;
					actual_index++;

					triangles [actual_index] = 2 * p2 + 1;
					actual_index++;
					triangles [actual_index] = 2 * p3 +1;
					actual_index++;
					triangles [actual_index] = 2 * p4 + 1;
					actual_index++;

					/*
					triangles [12 * index + 0] = 2 * p1;
					triangles [12 * index + 1] = 2 * p2;
					triangles [12 * index + 2] = 2 * p3;

					triangles [12 * index + 3] = 2 * p2;
					triangles [12 * index + 4] = 2 * p4;
					triangles [12 * index + 5] = 2 * p3;

					triangles [12 * index + 6] = 2 * p1 + 1;
					triangles [12 * index + 7] = 2 * p3 + 1;
					triangles [12 * index + 8] = 2 * p2 + 1;

					triangles [12 * index + 9] = 2 * p2 + 1;
					triangles [12 * index + 10] = 2 * p3 + 1;
					triangles [12 * index + 11] = 2 * p4 + 1;
					*/
				}
			}
		}
	}

	void GenerateUVMap(Vector2[] uvs)
	{
		float constant = 2f * Mathf.PI / (float)nbOfPetals; 

		Vector2 p1 = new Vector2 (0.5f, 0.25f);
		Vector2 p2 = new Vector2 (0.5f, 0.75f);

		if (flipTexture) {
			Vector2 temp = p1;
			p1 = p2;
			p2 = temp;
		}


		uvs [0] = p1;
		uvs [1] = p2;

		for (int i = 0; i < nbOfPetals + 1; i++) 
		{

			float angleRatio = i * constant;

			for (int j = 0; j < nbOfSegments; j++) 
			{


				float ratio = (j + 1) / (float)nbOfSegments ;



				Vector2 dir = Mathf.Cos (angleRatio) * new Vector2 (0f, 0.25f) +
				              Mathf.Sin (angleRatio) * new Vector2 (0.5f, 0f);


				Vector2 point = dir * segmentPointDistribution.Evaluate (ratio) * textureSize;

				//point -= dir * radius * closureForce.Evaluate(ratio) * closureForceCoef;

				//point += w * sideShape.Evaluate (ratio) * sideShapeCoef;



				int index = 1 + j + i * nbOfSegments;

				//Debug.Log ("p_" + index + " : " + point);
				//Debug.DrawRay (transform.position + point, Vector3.up * 0.1f, Color.red, 10f);

				uvs [2 * index ] = p1 + point;
				uvs [2 * index + 1] = p2 + point;
			}
		}



	}

}
