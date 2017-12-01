using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttractor : MonoBehaviour {

	public float attractTime = 1f;

	private bool attracting = false;
	private Vector3 startPos;
	private float startTime;

	void Update () {
		if (attracting) {
			var progress = (Time.time - startTime) / (startTime + attractTime - Time.time);
			var target = (Player.main.transform.position + startPos) / 2;
			transform.position = Vector3.Lerp (startPos, target, Mathf.Pow(progress, 2)).WithY(startPos.y);
			if (Vector3.Distance (transform.position.WithY(0), target.WithY(0)) < 1f) {
				SendMessage ("OnAttracted");
				attracting = false;
			}
		}
	}

	void OnTriggerEnter(Collider collider) {
		if (!attracting && collider.transform == Player.main.transform) {
			startPos = transform.position;
			startTime = Time.time;
			attracting = true;
		}
	}
}
