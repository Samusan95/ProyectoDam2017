//TorchFlickering.cs -- Samuel López Hernández
//Hace que una luz parpadee variando la intensidad
//-aleatoriamente entre un valor minimo y uno maximo
//a cada frame del juego.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchFlickering : MonoBehaviour {

	private Light l;
	[Range (0,8)]
	public float minIntensity;
	[Range (0,8)]
	public float maxIntensity;

	void Awake () {
		l = GetComponent <Light> ();
        
	}
	
	void Update () {
		//l.intensity = Random.Range (minIntensity, maxIntensity);
	}
}
