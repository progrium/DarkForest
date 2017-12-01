using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceCuller : MonoBehaviour {

	public float cutoff = 256f;

	private SpriteRenderer renderer;

	// Use this for initialization
	void Awake () {
		renderer = GetComponent<SpriteRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Vector3.Distance (transform.position, Player.main.transform.position) > cutoff) {
			renderer.enabled = false;
		} else {
			renderer.enabled = true;
		}
	}
}
