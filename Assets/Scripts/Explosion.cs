using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {

	public float duration = 0.5f;

	private float created;
	private SphereCollider collider;

	void Start () {
		created = Time.time;
		collider = GetComponent<SphereCollider> ();
	}

	void Update () {
		if (collider.enabled && Time.time > created + duration) {
			collider.enabled = false;
		}
	}
}
