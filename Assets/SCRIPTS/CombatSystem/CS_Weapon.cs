using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_Weapon : MonoBehaviour {

	public GameObject owner;
	public bool makeDamage;

	public float weaponDamage;

	public string targetTag = "Untagged";

	public float rate;
	private float rateTimer;

	public CS_Marker[] csMarkers;

	void Awake (){
		owner = GetComponentInParent <GameCharacter> ().gameObject;
		csMarkers = GetComponentsInChildren <CS_Marker> ();
	}

	void Update () {
		if (rateTimer <= rate)
			rateTimer += Time.deltaTime;

	}

	public void onChildCollisionDetected(GameObject obj){
		if (rateTimer >= rate) {
			if (makeDamage) {
				damage (obj);
				rateTimer = 0;
			}
		}	
	}

	void damage (GameObject obj){
		obj.SendMessage ("receiveDamage", new DamageInfo (owner.gameObject, weaponDamage), SendMessageOptions.DontRequireReceiver);
	}
		
}
