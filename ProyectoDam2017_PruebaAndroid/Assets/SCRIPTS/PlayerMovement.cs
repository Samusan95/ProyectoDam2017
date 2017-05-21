//PlayerMovement.cs -- Samuel López Hernández
//Clase que controla los movimientos del personaje 
//principal en base a las animaciones.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : GameCharacter {

    

    public enum states
    {
        goToPosition,
        attackEnemy
    }

    public states currentState;
    
    public Vector3 positionToGo;
    public GameObject currentEnemy;


    public float defenseCoolDown = 1f;
    public float defenseCoolDownTimer = 0;

    private void Start()
    {
       
        nav = GetComponent<NavMeshAgent>();
        positionToGo = transform.position;
    }
 
    private float yAxis;
    // Update is called once per frame
    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            currentEnemy = null;
        }

        if (isBlocking)
        {
            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 1, Time.deltaTime * 5));
            defenseCoolDownTimer += Time.deltaTime;
            if (defenseCoolDownTimer >= defenseCoolDown)
            {
                defenseCoolDownTimer = 0;
                isBlocking = false;
            }
        }
        else
        {
            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 0, Time.deltaTime * 5));
        }


        if (Input.GetMouseButtonDown(0))
        {
            sendOrder();
        }

        switch (currentState)
        {
            case states.goToPosition:
                goToPosition();
                break;
            case states.attackEnemy:
                if (currentEnemy)
                    attackEnemy();
                break;
        }

        animator.SetFloat ("speedMagnitude", nav.desiredVelocity.normalized.magnitude);
		
	
	}

    private void goToPosition()
    {
        if (!isInPosition(positionToGo))
        {
            nav.SetDestination(positionToGo);
            nav.isStopped = false;
        }
        else
        {
            nav.isStopped = true;
        }
    }

    private void attackEnemy()
    {
        if (!isInPosition(currentEnemy.transform.position))
        {
            nav.SetDestination(currentEnemy.transform.position);
            nav.isStopped = false;
        }
        else
        {
            nav.isStopped = true;
            faceToPosition(currentEnemy.transform.position);
            if (!isInAnimatorState(0, "Attack") && !isBlocking)
            {
                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    animator.SetTrigger("attack");
                }
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    isBlocking = true;
                }
            }
        }
    }
    private void sendOrder()
    {
        RaycastHit hit;
        Ray ray;
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            switch (hit.collider.tag)
            {
                case "Enemy":
                    currentEnemy = hit.collider.gameObject;
                    currentState = states.attackEnemy;
                    faceToPosition(currentEnemy.transform.position);
                    break;
                default:
                    positionToGo = hit.point;
                    positionToGo.y = yAxis;
                    currentState = states.goToPosition;
                    faceToPosition(positionToGo);
                    break;
            }

        }

        
    }

    

}
