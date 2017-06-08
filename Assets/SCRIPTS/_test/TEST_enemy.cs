//TEST_enemy.cs -- Samuel López Hernández
//IA enemiga de prueba basada en una máquina
//de estados finita.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent))]
public class TEST_enemy : GameCharacter {
    public float espera;
    private float esperapri = 0;

    [Header("Enemy")]
    private GameObject player;


    [Header("lineSight")]
    public Transform eyePosition;
    public float fieldOfView;
    public float distanceOfView;
    public LayerMask notMyLayer;

    //Variables
    [Header("Routes")]
    public Transform patrolRoute;
    public Transform[] wayPoints;
    private int contador;

    public bool canSeePlayer = false;

    public enum ENEMY_STATE
    {
        IDLE,
        CHASE,
        ATTACK
    }

    public ENEMY_STATE currentState
    {
        get { return CurrentState; }

        set
        {
            CurrentState = value;

            StopAllCoroutines();
            switch (CurrentState)
            {
                case ENEMY_STATE.IDLE:
                    StartCoroutine(AIIdle());
                    break;
                case ENEMY_STATE.CHASE:
                    StartCoroutine(AIChase());
                    break;

                case ENEMY_STATE.ATTACK:
                    StartCoroutine(AIAttack());
                    break;
            }
        }
    }

    [SerializeField]
    private ENEMY_STATE CurrentState = ENEMY_STATE.IDLE;

    public float attackDistance;
    private float distanceToPlayer;



    void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        getWayPoints();
        currentState = ENEMY_STATE.IDLE;
        player = closestObject("PlayerRoot");
    }

    void Update()
    {
        animator.SetFloat("speed", Mathf.Abs(nav.desiredVelocity.magnitude));
        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
    }

    private void LateUpdate()
    {
        canSeePlayer = canSeeTarget();
    }




    //--------------------------------------------ESTADOS---------------------------------------------//
    public IEnumerator AIIdle()
    {
        while (currentState == ENEMY_STATE.IDLE)
        {

            if (canSeePlayer)
            {
                nav.isStopped = false;
                currentState = ENEMY_STATE.CHASE;
                yield break;
            }


            if (patrolRoute)
            {

                if (isInPosition(wayPoints[contador].position))
                {
                    esperapri += Time.deltaTime;
                    nav.isStopped = true;
                    if (esperapri >= espera)
                    {

                        esperapri = 0;

                        if (contador == wayPoints.Length - 1)
                            contador = 0;
                        else
                            contador++;
                    }

                }
                else
                {
                    nav.isStopped = false;
                    nav.SetDestination(wayPoints[contador].position);
                }
            }
            yield return null;
        }

    }
    

    //Perseguir al jugador.
    public IEnumerator AIChase()
	{
		while (currentState == ENEMY_STATE.CHASE) {
			if (player != null) {
                nav.isStopped = false;
				if (distanceToPlayer <= attackDistance) {
					nav.isStopped = true;
					currentState = ENEMY_STATE.ATTACK;
					yield break;
				}


                nav.isStopped = false;
                nav.SetDestination (player.transform.position);

				//Mientras no ha cargado la ruta (Puede que no sea necesario, la ruta es muy simple)
				while (nav.pathPending) {
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
    //------------------------------------------------------------------------------------------------//


    //---------------------------------------------VISIÓN---------------------------------------------//

    public bool canSeeTarget()
    {
        if (inFieldOfView() && clearLineOfSight())
        {
            return true;
        }

        return false;
    }

    bool inFieldOfView()
    {
        if (player)
        {
            Vector3 directionToTarget = player.transform.position - eyePosition.position;

            
            Debug.DrawRay(eyePosition.position, Quaternion.AngleAxis(fieldOfView, transform.up) * transform.forward * distanceOfView, Color.red);
            Debug.DrawRay(eyePosition.position, Quaternion.AngleAxis(-fieldOfView, transform.up) * transform.forward * distanceOfView, Color.red);

            float angle = Vector3.Angle(transform.forward, directionToTarget);
            if (angle <= fieldOfView)
                return true;
        }
        return false;
    }
    
    bool clearLineOfSight()
    {
        if (player)
        {
            RaycastHit hit;

            Vector3 directionToTarget = (player.transform.position - eyePosition.position).normalized;

            Debug.DrawRay(eyePosition.position, (directionToTarget + Vector3.up / 10) * distanceOfView, Color.green);

            if (Physics.Raycast(eyePosition.position, (directionToTarget + Vector3.up / 10) * distanceOfView, out hit, notMyLayer))
            {
                if (hit.collider.tag == "PlayerRoot")
                {    
                    return true;
                }
            }

        }
        return false;
    }
    //------------------------------------------------------------------------------------------------//

    void getWayPoints()
    {
        wayPoints = new Transform[patrolRoute.transform.childCount];
        int i = 0;

        foreach (Transform t in patrolRoute)
        {
            wayPoints[i++] = t;
        }
    }



    //............................................________
    //....................................,.-'"...................``~.,
    //.............................,.-"..................................."-.,
    //.........................,/...............................................":,
    //.....................,?......................................................,
    //.................../...........................................................,}
    //................./......................................................,:`^`..}
    //.............../...................................................,:"........./
    //..............?.....__.........................................:`.........../
    //............./__.(....."~-,_..............................,:`........../
    //.........../(_...."~,_........"~,_....................,:`........_/
    //..........{.._$;_......"=,_......."-,_.......,.-~-,},.~";/....}
    //...........((.....* ~_......."=-._......";,,./`..../"............../
    //...,,,___.`~,......"~.,....................`.....}............../
    //............(....`=-,,.......`........................(......;_,,-"
    //............/.`~,......`-...................................../
    //.............`~.*-,.....................................|,./.....,__
    //,,_..........}.>-._...................................|..............`=~-,
    //.....`=~-,__......`,.................................
    //...................`=~-,,.,...............................
    //................................`:,,...........................`..............__
    //.....................................`=-,...................,%`>--==``
    //........................................_..........._,-%.......`
    //..................................., 






}
