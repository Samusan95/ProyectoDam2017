using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

	public GameObject enemy;
	public float spawnTime;

	// Use this for initialization
	void Start () {
		InvokeRepeating ("Spawn", 5f, spawnTime);
	}


	void Spawn () 
	{
		Instantiate(enemy, transform.position + Vector3.up, Quaternion.identity);
	}
}
