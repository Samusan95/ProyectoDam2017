using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_AnimationEvents : MonoBehaviour {

	public CS_Weapon weapon;

	void Awake () 
	{
		weapon = GetComponentInChildren <CS_Weapon> ();
	}
	void enableCollision () 
	{
		weapon.makeDamage = true;
	}

	void disableCollision () 
	{
		weapon.makeDamage = false;
	}
}
