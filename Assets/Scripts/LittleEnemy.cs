using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LittleEnemy : MonoBehaviour {

	public GameObject explosionPrefab;
	public AudioClip[] idleClips;
	public AudioClip attackClip;

	public GameObject healthSpawn;

	public AudioClip[] explosionSounds;

	public float detectFOV = 90f;
	public Vector3 lastSpotted;

	public float attackSpeed = 30f;

	private bool attacking = false;
	private float nextIdle;

	private List<GameObject> nearbyEnemies = new List<GameObject>();

	private bool playerSpotted = false;

	private CharacterController controller;

	void Awake() {
		controller = GetComponent<CharacterController> ();
	}

	void Start() {
		ScheduleIdle ();
	}

	void ScheduleIdle() {
		nextIdle = Time.time + Random.Range (10, 20);
	}

	void OnHit() {
		var explosion = Instantiate (explosionPrefab, transform.position, transform.rotation);
		var explosionSound = explosion.GetComponent<AudioSource> ();
		explosionSound.clip = explosionSounds [0]; //Random.Range (0, explosionSounds.Length)
		explosionSound.pitch = 1 + (Game.main.comboHits * 0.25f);
		explosionSound.Play ();
		foreach (var enemy in nearbyEnemies) {
			if (enemy.activeInHierarchy) {
				enemy.SendMessage ("OnNearbySound", transform, SendMessageOptions.DontRequireReceiver);
			}

		}
		if (Player.main.health < 4 && Vector3.Distance (transform.position, Player.main.transform.position) > 4f && Random.Range(0,3) != 2) {
			Instantiate (healthSpawn, transform.position, transform.rotation);
		}
		//Destroy (gameObject);
		gameObject.SetActive(false);
	}

	void OnNearbySound(Transform obj) {
		transform.LookAt (obj);
	}

	void Update() {
		if (Player.main.freeze) {
			return;
		}

		if (!attacking && Time.time > nextIdle) {
			var targetRotation = Random.Range (45f, 180f);
			var rotation = transform.eulerAngles;
			rotation.y = targetRotation;
			transform.eulerAngles = rotation;
			GetComponent<AudioSource> ().PlayOneShot (idleClips[Random.Range(0, idleClips.Length)]);
			ScheduleIdle ();
		}
		if (attacking) {
			controller.Move (transform.forward * Time.deltaTime * attackSpeed);
			if (Vector3.Distance (transform.position, Player.main.transform.position) < 3f) {
				OnHit ();
				Player.main.SendMessage ("OnDamage");
			}
		}
	}

	void OnTriggerStay(Collider other) {
		var previouslySpotted = playerSpotted;
		if (other.tag == "Player") {
			playerSpotted = false;

			Vector3 dir = other.transform.position - transform.position;
			float angle = Vector3.Angle (dir, transform.forward);
			if (angle < detectFOV * 0.5f) {
				RaycastHit hit;
				var col = GetComponent<SphereCollider> ();
				if (Physics.Raycast (transform.position, dir.normalized, out hit, col.radius)) {
					if (hit.collider.tag == "Player") {
						playerSpotted = true;
						lastSpotted = hit.collider.transform.position;
						transform.LookAt (hit.collider.transform);
						if (!previouslySpotted) {
							GetComponent<AudioSource> ().PlayOneShot (attackClip);
							attacking = true;
						}
					}
				}
			}
		}
	}

	void OnTriggerEnter(Collider other) {
		if (other.tag == "LittleEnemy") {
			nearbyEnemies.Add (other.gameObject);
		}
		if (other.tag == "Player") {
			Player.main.SendMessage ("OnEnemyEnter", gameObject, SendMessageOptions.DontRequireReceiver);
		}
		if (!Game.main.bossMode && other.tag == "Explosion") {
			Invoke("OnHit", Random.Range(0.05f, 0.25f));
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.tag == "Player") {
			playerSpotted = false;
			attacking = false;
			Player.main.SendMessage ("OnEnemyExit", gameObject, SendMessageOptions.DontRequireReceiver);
		}
		if (other.tag == "LittleEnemy") {
			nearbyEnemies.Remove (other.gameObject);
		}
	}
}
