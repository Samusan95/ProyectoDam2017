//GameCharacterSounds.cs - Samuel López Hernández
//Clase que controla la ejecucion de los sonidos
//de cada personaje del juego.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(AudioSource))]
public class GameCharacterSounds : MonoBehaviour {

	private AudioSource aud;
	public AudioClip [] stepClips;
	public AudioClip [] shieldHittedClips;
	public AudioClip [] hitClips;


	void Start () {
		aud = GetComponent <AudioSource> ();
	}
	
	public void stepSound()
	{
		if (aud) {
			aud.volume = 0.2f;
			aud.clip = chooseSound (stepClips);
			aud.Play ();
		}
	}

	public void shieldHittedSound()
	{
		if (aud) {
			aud.volume = 1f;
			aud.clip = chooseSound(shieldHittedClips);
			aud.Play();
		}
	}

	public void hitSound ()
	{
		if (aud) {
			aud.volume = 0.5f;
			aud.clip = chooseSound (hitClips);
			aud.Play ();
		}
	}

	//Devuelve un sonido aleatorio dentro 
	//de un Array de Audioclips.
	private AudioClip chooseSound (AudioClip [] clips) 
	{
		if (clips.Length == 0)
			return null;
		else if (clips.Length == 1)
			return clips [0];
		else
			return clips [Random.Range (0, clips.Length)];
	}
}
