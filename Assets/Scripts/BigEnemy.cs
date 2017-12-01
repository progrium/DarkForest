using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigEnemy : MonoBehaviour {

	public float spawnInterval = 10f;
	public GameObject spawnPrefab;

	public float speed = 8f;
	public GameObject waypoints;



	private float lastSpawn;

	public Vector3 target;
	public Vector3 nextTarget;
	private bool hasNextTarget = false;
	private Vector3 startPos;
	private float startTime;
	private float journeyDistance;

	private float lastPlayerHit;
	public GameObject playerHitExplosion;

	void Start() {
		startPos = transform.position;
		nextTarget = Vector3.zero;
		SetTarget (NearestWaypoint ());
	}

//	void OnTriggerEnter(Collider col) {
//		if (col.gameObject == Player.main.gameObject) {
//			Game.main.Die ();
//		}
//	}

	void OnHit() {
		GetComponent<PlayerSeekerWanderer> ().target = Player.main.transform.position;
	}

	void Update() {
		if (Player.main.freeze) {
			return;
		}


		if (Time.time > lastSpawn + spawnInterval) {
			Instantiate (spawnPrefab, transform.position, transform.rotation);
			lastSpawn = Time.time;
		}

		if (Vector3.Distance(transform.position.WithY(0), target.WithY(0)) < 0.5f) {
			if (hasNextTarget) {
				SetTarget (nextTarget);
				hasNextTarget = false;
			} else {
				SetTarget (NearestWaypoint ());
			}
		}
			
		float distanceCovered = (Time.time - startTime) * speed;
		transform.position = Vector3.Lerp (startPos, target, distanceCovered / journeyDistance);
		transform.LookAt (target.WithY (transform.position.y));

		if (Time.time > (lastPlayerHit+1f) && Vector3.Distance (transform.position, Player.main.transform.position) < 15f) {
			var explosion = Instantiate (playerHitExplosion, Player.main.transform.position, Player.main.transform.rotation);
			explosion.GetComponent<AudioSource> ().Play ();
			Player.main.SendMessage ("OnDamage");
			lastPlayerHit = Time.time;
		}
	}

	public void SetTarget(Vector3 target) {
		this.target = target;
		startTime = Time.time;
		startPos = transform.position;
		journeyDistance = Vector3.Distance (startPos, target);
	}

	public void SetNextTarget(Vector3 target) {
		this.nextTarget = target;
		hasNextTarget = true;
	}

	Vector3 NearestWaypoint() {
		var nearestPoint = Vector3.positiveInfinity;
		var nearestPointDistance = float.PositiveInfinity;
		foreach (Transform waypoint in waypoints.transform) {
			var waypointDistance = Vector3.Distance (waypoint.position, transform.position);
			if (waypointDistance < nearestPointDistance) {
				nearestPoint = waypoint.position;
				nearestPointDistance = waypointDistance;
			}
		}
		return nearestPoint;
	}
}
