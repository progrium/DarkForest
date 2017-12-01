using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumper : MonoBehaviour {


	public float gravityForce = 800f;
	public float jumpForceLow = 80f;
	public float jumpForceHigh = 120f;

	private bool jumping = false;
	private float velocity = 0f;

	private float nextJumpTime = 0f;


	void Update() {
		var groundHeight = GetComponent<TerrainHeighter> ().minHeight;
		if (jumping) {
			transform.position = transform.position.WithY(transform.position.y + (velocity*Time.deltaTime));
			if (transform.position.y > groundHeight) {
				velocity -= gravityForce * Time.deltaTime;
			} else {
				jumping = false;
			}
		}
		if (!jumping) {
			transform.position = transform.position.WithY (groundHeight);
			velocity = 0f;
		}
		if (Time.time > nextJumpTime) {
			if (Random.Range (0, 2) == 1) {
				Jump ();
			}
			nextJumpTime = Time.time + 1f + (Random.value * 2);
		}
	}

	void Jump() {
		if (!jumping) {
			velocity = Random.Range (jumpForceLow, jumpForceHigh);
			jumping = true;
		}
	}
		
}
