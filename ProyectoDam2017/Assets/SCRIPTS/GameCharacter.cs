//GameCharacter.cs -- Samuel López Hernández
//Clase base que define a un personaje del juego.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Animator))]
public abstract class GameCharacter : MonoBehaviour {


	protected bool isDead;
	protected Animator animator;
	protected bool isBlocking;


	[Header ("Estadisticas")]
	public float currentHealth;
	public float currentStamina;


	public void SetHealth(float health){
		currentHealth = health;

		if (currentHealth > 100) {
			currentHealth = 100;
		}

		if (currentHealth < 0) {
			currentHealth = 0;
		}
	}	
		
	public void SetStamina (float stamina){
		currentStamina = stamina;

		if (currentStamina > 100) {
			currentStamina = 100;
		}

		if (currentStamina < 0) {
			currentStamina = 0;
		}
	}

	public float GetHealth () {
		return currentHealth;
	}

	public float GetStamina (){
		return currentStamina;
	}

	void Awake ()
	{
		animator = GetComponent <Animator>();

	}
		
	//Comprueba si está en un estado determinado del Animator Controller
	public bool isInAnimatorState(int layer, string name)
	{
		if (animator.GetCurrentAnimatorStateInfo(layer).IsName(name))
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	//Busca el objeto mas cercano con un tag
	protected GameObject closestObject(string tag)
	{
		GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(tag);
		GameObject closestObject = null;

		if (gameObjects.Length > 0)
		{
			closestObject = gameObjects[0];
			foreach (GameObject obj in gameObjects)
			{
				float distanceToObj = Vector3.Distance(transform.position, obj.transform.position);
				float distanceToClosestObject = Vector3.Distance(transform.position, closestObject.transform.position);
				if (distanceToObj < distanceToClosestObject)
					closestObject = obj;
			}
		}
		else
		{
			return null;
		}   
		return closestObject;
	}

	//---------------------------------------------------------------------------COMBATE-------------------------------------------------------------------------//


	private void receiveDamage(DamageInfo info){
		if (checkBlock (info.getAttacker().transform)) {
			animator.SetTrigger ("shieldImpact");
		} else {
			currentHealth -= info.getDamage();

			GetComponent <GameCharacterSounds> ().hitSound ();

			if (currentHealth <= 0) {
				death ();
			} else {
				animator.SetTrigger ("stunt");
			}
		}
	}

	//Chequea desde donde le han atacado.
	//Si le golpean de frente
	//(en un ángulo de 60º tomando como el angulo 0 
	//la direccion donde está mirando) 
	//devuelve true.
	private bool checkBlock (Transform target)
	{
		if (isBlocking) {
			float angle = Vector3.Angle (target.position - transform.position, transform.TransformDirection (Vector3.forward));
			if (angle <= 60) {
				return true;
			}
			return false;
		}
		return false;
	}

	//----------------------------------------------------------------------------------------------------------------------------------------------------------------//




	void death ()
	{
		animator.enabled = false;
		tag = "Untagged";

		Component[] components = gameObject.GetComponents <Component> ();
		foreach (Component c in components) {
			if (c.GetType() != typeof(Transform))
			Destroy (c);
		}
	}



}
