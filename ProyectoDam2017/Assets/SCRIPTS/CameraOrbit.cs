//CameraOrbit.cs -- Algun dia le cambiaré el nombre.


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Camera-Control/Mouse Orbit with zoom")]
public class CameraOrbit : MonoBehaviour {

    public Transform player;
	private GameObject enemy;
	[Header ("Orbit")]
    public float distance = 5.0f;
    public float height = 1.5f;
    public float xSpeed = 120.0f;
    public float ySpeed = 120.0f;

    public float yMinLimit = -20f;
    public float yMaxLimit = 80f;

    public float distanceMin = .5f;
    public float distanceMax = 15f;

  

    float x = 0.0f;
    float y = 0.0f;
	[Header ("Look at enemy")]
	public Vector3 cameraOffset;
	public float enemyHeightOffset;

    // Use this for initialization
    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;

     
       
    }

    void LateUpdate()
    {
		if (player.GetComponent<PlayerMovement> ()) {
			if (player.GetComponent<PlayerMovement> ().lookAtEnemy && player.GetComponent<PlayerMovement> ().lockedEnemy) {
				lookAtEnemy ();
			} else {
				orbit ();
			}
		}

    }

	void lookAtEnemy(){
		if (player) {
			enemy = player.GetComponent <PlayerMovement> ().lockedEnemy;

			if (enemy) {
			


				Vector3 position = player.position + player.TransformDirection(new Vector3 (1,1,1) + cameraOffset);
				transform.position = Vector3.Lerp (transform.position, position, Time.deltaTime * 5);

				transform.LookAt (enemy.transform.position + Vector3.up * enemyHeightOffset);

			} 
		}
	}

    void orbit()
    {
        if (player)
        {
            x += Input.GetAxis("Mouse X") * xSpeed * distance * 0.02f;
            y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;

            y = ClampAngle(y, yMinLimit, yMaxLimit);

            Quaternion rotation = Quaternion.Euler(y, x, 0);

            distance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel") * 5, distanceMin, distanceMax);

            Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
            Vector3 position = rotation * negDistance + player.position;


			transform.position = Vector3.Lerp(transform.position, position + Vector3.up * height, Time.deltaTime * 5);
			transform.rotation = Quaternion.Lerp (transform.rotation, rotation, Time.deltaTime * 5);
        }
    }


    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }

}
