using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyHint : MonoBehaviour {

	static public bool shown = false;

	public GameObject hintMessage;

	void OnTriggerEnter(Collider col) {
		if (col.gameObject != Player.main.gameObject) {
			return;
		}
		if (!shown) {
			hintMessage.SetActive (true);
			shown = true;
		}
	}
}
