using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meteoriteSpawner : MonoBehaviour {

	public GameObject meteorite;

	public Collider zone;

	public float timeBetweenSpawns = 10f;

	private bool isSpawning;

	// Use this for initialization
	void Start () {

		isSpawning = false;

	}
	
	// Update is called once per frame
	void Update () {
		if(!isSpawning)
		{
			isSpawning = true;
			StartCoroutine("SpawnMeteorite");
		}
	}

	IEnumerator SpawnMeteorite()
	{
		Instantiate(meteorite, getSpawnLocation(), Quaternion.identity);
		yield return new WaitForSeconds(timeBetweenSpawns);
		isSpawning = false;
	}

	private Vector3 getSpawnLocation()
	{
		return new Vector3(Random.Range(zone.bounds.min.x, zone.bounds.max.x),
			Random.Range(zone.bounds.min.y, zone.bounds.max.y),
			Random.Range(zone.bounds.min.z, zone.bounds.max.z));
	}
}
