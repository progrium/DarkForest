using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSeekerWanderer : MonoBehaviour {

	public float range = 30f;
	public float speed = 1f;
	public float stopDistance = 1f;

	private CharacterController controller;

	private bool seeking = false;
	public Vector3 target;

	void Awake() {
		controller = GetComponent<CharacterController> ();
		PickTarget ();
	}

	void PickTarget() {
		var x = Random.Range (0, 256);
		var y = Random.Range (0, 256);
		target = new Vector3 (x, 0, y);
	}

	void Update() {
		if (Vector3.Distance (Player.main.transform.position.WithY (0), transform.position.WithY (0)) > range) {
			if (Vector3.Distance (transform.position.WithY (0), target.WithY(0)) < 5f) {
				PickTarget ();
			}
			transform.LookAt (target.WithY(transform.position.y));
			controller.Move (transform.forward * Time.deltaTime * speed);
		} else {
			transform.LookAt (Player.main.transform.position.WithY (transform.position.y));
			if (Vector3.Distance (Player.main.transform.position.WithY (0), transform.position.WithY (0)) > stopDistance) {
				if (!seeking) {
					SendMessage ("OnSeekEnter", SendMessageOptions.DontRequireReceiver);
					seeking = true;
				}
				controller.Move (transform.forward * Time.deltaTime * speed);
			} else {
				if (seeking) {
					SendMessage ("OnSeekExit", SendMessageOptions.DontRequireReceiver);
					seeking = false;
				}
			}
		}
	}
}
