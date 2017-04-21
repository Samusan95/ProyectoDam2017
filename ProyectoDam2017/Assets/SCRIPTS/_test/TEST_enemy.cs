//TEST_enemy.cs -- Samuel López Hernández
//IA enemiga de prueba basada en una máquina
//de estados finita.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent))]
public class TEST_enemy : GameCharacter {


	[Header ("Enemy")]
	private NavMeshAgent agent;
	private GameObject player;

	public enum ENEMY_STATE
	{
		CHASE,
		ATTACK
	}

	public ENEMY_STATE currentState
	{
		get {return CurrentState;}

		set 
		{
			CurrentState = value;

			StopAllCoroutines ();
			switch (CurrentState)
			{
			case ENEMY_STATE.CHASE:
				StartCoroutine (AIChase ());
					break;

			case ENEMY_STATE.ATTACK:
				StartCoroutine (AIAttack ());
					break;
			}
		} 
	}

	[SerializeField]
	private ENEMY_STATE CurrentState = ENEMY_STATE.CHASE;

	public float attackDistance;
	private float distanceToPlayer;



	void Awake ()
	{
		agent = GetComponent<NavMeshAgent> ();
		animator = GetComponent<Animator> ();
	}

	void Start ()
	{
		currentState = ENEMY_STATE.CHASE;
	}

	void Update()
	{
		animator.SetFloat ("speed", Mathf.Abs (agent.desiredVelocity.magnitude));
		distanceToPlayer = Vector3.Distance (transform.position, player.transform.position);
	}

	//Perseguir al jugador.
	public IEnumerator AIChase()
	{
		while (currentState == ENEMY_STATE.CHASE) {
			if (player != null) {
				
				if (distanceToPlayer <= attackDistance) {
					agent.isStopped = true;
					currentState = ENEMY_STATE.ATTACK;
					yield break;
				}


                agent.isStopped = false;
                agent.SetDestination (player.transform.position);

				//Mientras no ha cargado la ruta (Puede que no sea necesario, la ruta es muy simple)
				while (agent.pathPending) {
					yield return null;
				}



			} else {
				player = closestObject ("PlayerRoot");
			}
			yield return null;
		}
	}

	//Atacar al jugador
	public IEnumerator AIAttack()
	{
		while (currentState == ENEMY_STATE.ATTACK) {
			

			if (distanceToPlayer > attackDistance) {

				currentState = ENEMY_STATE.CHASE;
				yield break;
			} else {
				if (!isInAnimatorState (0, "Attack")) {
					animator.SetTrigger ("attack");
					transform.LookAt (new Vector3 (player.transform.position.x, transform.position.y, player.transform.position.z));
				}
			}

			yield return null;
		}
	}
		

}
