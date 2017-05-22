//DamageInfo.cs -- Samuel López Hernández
//Clase que define un ataque.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageInfo {

	GameObject attacker;
	float damage;

	public DamageInfo (GameObject attacker, float damage){
		this.attacker = attacker;
		this.damage = damage;
	}

	public GameObject getAttacker(){
		return attacker;
	}

	public float getDamage(){
		return damage;
	}

}
