//PlayerMovement.cs -- Samuel López Hernández
//Clase que controla los movimientos del personaje 
//principal en base a las animaciones.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : GameCharacter {

    [Header("Controls")]
    public GameObject combatUI;
    public TouchControls input;

    
    public enum states
    {
        goToPosition,
        attackEnemy
    }
    [Header("States")]
    public states currentState;
    
    private Vector3 positionToGo;
    public GameObject currentEnemy;


    public float defenseCoolDown = 1f;
    private float defenseCoolDownTimer = 0;

    bool isInCombat;
    private void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        positionToGo = transform.position;
    }
 
    private float yAxis;
    // Update is called once per frame
    void Update ()
    {

        isInCombat = currentEnemy != null && Vector3.Distance(transform.position, currentEnemy.transform.position) < 5f;
        combatUI.SetActive(isInCombat);

        float speedMagnitude;

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

        if (!isInCombat)
        {

            enableAgent(true);

            speedMagnitude = nav.desiredVelocity.normalized.magnitude;
           


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
        }
        else
        {
            enableAgent(false);


            float h = input.Horizontal();
            float v = input.Vertical();
            
            speedMagnitude = Mathf.Abs(h + v);
            animator.SetFloat("vertical", v);
            animator.SetFloat("horizontal", h);

            if (speedMagnitude > 0)
                faceToPosition(currentEnemy.transform.position);

            if (!isInAnimatorState(0, "Attack") && !isBlocking)
            {
                if (Input.GetKeyDown(KeyCode.RightArrow) || input.SwipeX() > 0)
                {
                    animator.SetTrigger("attack");
                }
                if (Input.GetKeyDown(KeyCode.LeftArrow) || input.SwipeX()<0)
                {
                    isBlocking = true;
                }
            }


        }



        animator.SetFloat("speedMagnitude", speedMagnitude);
        animator.SetBool("inCombat", isInCombat);
        
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

    public void enableAgent(bool enabled)
    {
        if (enabled)
        {
            nav.enabled = true;
            GetComponent<CharacterController>().enabled = false;
        }
        else
        {
            nav.enabled = false;
            GetComponent<CharacterController>().enabled = true;
        }
    }

    public void unlockEnemy()
    {
        currentEnemy = null;
    }
}
