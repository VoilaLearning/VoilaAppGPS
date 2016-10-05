using UnityEngine;
using System.Collections;

public class EnvironmentGenerator : MonoBehaviour {

	public float xMin;
	public float xMax;

	public float yMin;
	public float yMax;

	public float zMin;
	public float zMax;

	//public GameObject cloud1;
	//public GameObject cloud2;
	//public GameObject cloud3;

	public GameObject grass1;
	public GameObject grass2;
	public GameObject grass3;



	void Start () 
	{
	//	SpawnClouds ();
		SpawnGrass ();
	}
	/*
	void SpawnClouds ()
	{
		{
			Vector3 pos1 = new Vector3 (Random.Range (xMin, xMax), Random.Range (yMin, yMax), Random.Range (zMin, zMax));
			Instantiate (cloud1, pos1, transform.rotation, transform);
		}

		{
			Vector3 pos2 = new Vector3 (Random.Range (xMin, xMax), Random.Range (yMin, yMax), Random.Range (zMin, zMax));
			Instantiate (cloud2, pos2, transform.rotation, transform);
		}	

		{
			Vector3 pos3 = new Vector3 (Random.Range (xMin, xMax), Random.Range (yMin, yMax), Random.Range (zMin, zMax));
			Instantiate (cloud3, pos3, transform.rotation, transform);
		}
	}
	*/
	void SpawnGrass ()
	{
		{
			Vector3 pos1 = new Vector3 (Random.Range (xMin, xMax), 1.1f, Random.Range (zMin, zMax));
			Instantiate (grass1, pos1, transform.rotation, transform);
		}

		{
			Vector3 pos2 = new Vector3 (Random.Range (xMin, xMax), 1.1f, Random.Range (zMin, zMax));
			Instantiate (grass2, pos2, transform.rotation, transform);
		}	

		{
			Vector3 pos3 = new Vector3 (Random.Range (xMin, xMax), 1.1f, Random.Range (zMin, zMax));
			Instantiate (grass3, pos3, transform.rotation, transform);
		}
	}
}
