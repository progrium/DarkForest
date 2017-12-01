using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour {

	public AudioClip heartSound;

	void OnAttracted() {
		Player.main.audio.PlayOneShot (heartSound);
		Player.main.SendMessage ("OnHealth");
		Destroy (gameObject);
	}
}
