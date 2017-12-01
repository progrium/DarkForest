using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour {

	public AudioClip collectSound;

	public GameObject[] ghostKeys;

	void OnAttracted() {
		Player.main.audio.PlayOneShot (collectSound);
		Game.main.gate.collectedKeys++;
		Game.main.gate.UpdateHud ();
		foreach (var key in ghostKeys) {
			key.SetActive (false);
		}
		Destroy (gameObject);
	}
}
