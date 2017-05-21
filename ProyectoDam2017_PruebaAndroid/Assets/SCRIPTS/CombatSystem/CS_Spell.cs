using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_Spell : MonoBehaviour {

	public enum SPELL_TYPE
	{
		self,
		area
	}

	public SPELL_TYPE spellType;

	public string spellName;

	public float modCurHealth;
	public float modCurStamina;

    public GameObject visualEffect;

	public void castSpell () 
	{
		if (spellType == SPELL_TYPE.self) {
			modValues (GetComponent <GameCharacter> ());
            GameObject effect = Instantiate(visualEffect, transform.position, Quaternion.identity);
            effect.transform.parent = this.transform;
            Destroy(effect, 5);
        }
	}

	void modValues (GameCharacter character){
		character.SetHealth (character.GetHealth () + modCurHealth);
		character.SetStamina (character.GetStamina () + modCurStamina);
	}
		
}
