//CameraOrbit.cs -- Algun dia le cambiaré el nombre.


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Camera-Control/Mouse Orbit with zoom")]
public class CameraOrbit : MonoBehaviour {

    public Transform player;
	
    public float distance = 5.0f;


    public bool playerHasEnemy;
    public Vector3 positionInCombat;
    

    public float playerHeight;

    public float cameraSpeed;

    public Vector3 positionInMovement;

    void Update()
    {

        Vector3 position = Vector3.zero;
        Quaternion rotation = transform.rotation;

        playerHasEnemy = player.GetComponent<PlayerMovement>().currentEnemy != null;

        if (playerHasEnemy)
        {
            position = player.position + player.TransformDirection(new Vector3(1, 1, 1) + positionInCombat);
            

            rotation = Quaternion.LookRotation((player.position + player.forward * 1.5f + Vector3.up) - transform.position);
        }
        else
        {
            position = player.position + positionInMovement;
            rotation = Quaternion.LookRotation(player.transform.position - transform.position);
        }

        transform.position = Vector3.Lerp (transform.position, position, Time.deltaTime * cameraSpeed);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * cameraSpeed);
    }


   


}
