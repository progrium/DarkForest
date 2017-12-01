using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class BossWeakpoint : MonoBehaviour {

	public GameObject hpBar;
	public int hits = 0;

	void OnTriggerEnter(Collider col) {
		if (col.gameObject.layer == LayerMask.NameToLayer ("Bullet")
			&& col.gameObject.tag == "BombArrow") {
			hits++;
			var input = ReInput.players.GetPlayer (0);
			input.SetVibration (0, 1f, 1f);
			var hit = hpBar.transform.GetChild (4 - hits);
			hit.gameObject.SetActive (false);
			if (hits >= 4) {
				Game.main.Win ();
			}
		}
	}
}
