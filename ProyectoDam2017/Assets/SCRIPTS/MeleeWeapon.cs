using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour {

	public GameObject owner;
	public string EnemyTag;

	public bool makeDamage;


	void OnTriggerEnter (Collider other) {
		if (other.tag == EnemyTag && makeDamage) 
		{
			other.gameObject.SendMessageUpwards ("receiveDamage",owner,SendMessageOptions.DontRequireReceiver);
		
		}
	}

}
