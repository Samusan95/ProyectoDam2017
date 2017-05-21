using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_Marker : MonoBehaviour {

	public CS_Weapon weapon;
	public string targetTag = "Untagged";

	void Awake()
	{
		weapon = GetComponentInParent <CS_Weapon> ();
		targetTag = weapon.targetTag;

	
	}
	void OnTriggerEnter (Collider other){
		if (other.tag == targetTag) {
			weapon.onChildCollisionDetected (other.gameObject);

		}
	}

	void OnDrawGizmos ()
	{
		Gizmos.DrawMesh(GetComponent <Mesh>());
	}
}

