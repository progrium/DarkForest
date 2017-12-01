using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using Rewired;

public class Boss : MonoBehaviour {

	public GameObject spawnPrefab;

	public float speed = 8f;
	public GameObject waypoints;

	public Material normalMaterial;
	public Material fadeMaterial;
	public SkinnedMeshRenderer meshRenderer;
	public GameObject skull;
	public GameObject jaw;
	public GameObject spawner;
	public GameObject weakpoint;

	public GameObject wireSkull;
	public GameObject wireJaw;

	public GameObject playerHitExplosion;

	public AudioMixer mixer;
	public float startVolume = -9f;

	public GameObject hpBar;

	private float lastSpawn;

	private float lastPlayerHit;

	public Vector3 target;
	public Vector3 nextTarget;
	private bool hasNextTarget = false;
	private Vector3 startPos;
	private float startTime;
	private float journeyDistance;
	private bool playerFocus = true;

	private Coroutine bossRoutine;

	void Awake() {
		weakpoint.SetActive (false);
		meshRenderer.enabled = false;
		wireSkull.SetActive (false);
		weakpoint.SetActive (false);
	}

	void Start() {
		startPos = transform.position;
		nextTarget = Vector3.zero;
		mixer.SetFloat ("MasterVolume", startVolume);
		bossRoutine = StartCoroutine (boss ());
	}

//	void OnTriggerEnter(Collider col) {
//		if (col.gameObject == Player.main.gameObject) {
//			Game.main.Die ();
//		}
//	}

	// Player died
	void Die() {
		StartCoroutine (die ());
	}

	IEnumerator die() {
		mixer.SetFloat ("MasterVolume", -80f);
		StopCoroutine (bossRoutine);
		Player.main.freeze = true;
		Game.main.black.FadeIn (2f);
		yield return new WaitForSeconds (2f);
		Game.main.gameOverMessage.SetActive (true);
		var input = ReInput.players.GetPlayer (0);
		while (true) {
			if (input.GetButtonDown("Fire")) {
				break;
			}
			yield return null;
		}
		Cursor.lockState = CursorLockMode.None;
		SceneManager.LoadScene (1);
	}


	IEnumerator boss() {
		yield return new WaitForSeconds (1f);


		// TODO: relocate near player if in forest

		while (true) {
			// fade in
			LookAtPlayer();
			meshRenderer.material = fadeMaterial;
			meshRenderer.enabled = true;
			wireJaw.SetActive (true);
			wireSkull.SetActive (true);
			yield return fade (0f, 1f, 1f);
			meshRenderer.material = normalMaterial;
			meshRenderer.GetComponent<MeshCollider> ().enabled = true;
			weakpoint.SetActive (true);
			yield return new WaitForSeconds (0.5f);

			// charges
			LookAtPlayer();
			skull.transform.Rotate (20, 0, 0);
			var charges = Random.Range (1, 3);
			for (var i = 0; i < charges; i++) {
				yield return new WaitForSeconds (0.5f);
				yield return charge (90f, 90f);
				yield return new WaitForSeconds (2f);
				LookAtPlayer();
			}
			skull.transform.Rotate (-20, 0, 0);
			yield return new WaitForSeconds (1f);

			// spawn enemies
			jaw.transform.Rotate (-40, 0, 0);
			LookAtPlayer();
			var strafe = StartCoroutine (strafeLoop ());
			yield return new WaitForSeconds (1f);
			for (var i = 0; i < 3; i++) {
				Instantiate (spawnPrefab, spawner.transform.position, spawner.transform.rotation);
				yield return new WaitForSeconds (0.5f);
				LookAtPlayer();
			}
			StopCoroutine (strafe);
			yield return new WaitForSeconds (1f);
			jaw.transform.Rotate (40, 0, 0);

			// fade out
			meshRenderer.material = fadeMaterial;
			meshRenderer.GetComponent<MeshCollider> ().enabled = false;
			wireJaw.SetActive (false);
			wireSkull.SetActive (false);
			weakpoint.SetActive (false);
			yield return fade (1f, 0f, 1f);

			// relocate
			var target = RandomWaypoint();
			var startTime = Time.time;
			var startPos = transform.position;
			var journeyDistance = Vector3.Distance (startPos, target);
			var distanceCovered = 0f;
			while (distanceCovered < journeyDistance) {
				distanceCovered = (Time.time - startTime) * 50f;
				transform.position = Vector3.Lerp (startPos, target, distanceCovered / journeyDistance);
				yield return null;
			}

		}
	}

	IEnumerator strafeLoop() {
		var right = transform.position + (transform.right * 10);
		var left = transform.position + (transform.right * -10);
		var start = transform.position;
		var progress = 0.5f;
		while (true) {
			transform.position = Vector3.Lerp (left, right, Mathf.PingPong (progress, 1f));
			progress = progress + (Time.deltaTime * 1.5f);
			yield return null;
		}
	}

	void LookAtPlayer() {
		transform.LookAt (Player.main.transform.position.WithY(transform.position.y));
	}

	IEnumerator charge(float distance, float speed) {
		LookAtPlayer ();
		var startPos = transform.position;
		while (Vector3.Distance (transform.position, startPos) < distance) {
			transform.Translate (Vector3.forward * speed * Time.deltaTime);
			yield return null;
		}
	}

	void SetAlpha(float a) {
		var c = fadeMaterial.color;
		c.a = a;
		fadeMaterial.color = c;
	}

	IEnumerator fade(float start, float end, float time = 3f) {
		var startTime = Time.time;
		var endTime = Time.time + time;
		while (Time.time < endTime) {
			var progress = (Time.time - startTime) / (endTime - startTime);
			SetAlpha (Mathf.Lerp (start, end, progress));
			yield return new WaitForSeconds(0.01f);
		}
		SetAlpha (end);
	}
		

	void Update() {
		if (Player.main.freeze) {
			return;
		}
		if (weakpoint.activeInHierarchy && Time.time > (lastPlayerHit+1f) && Vector3.Distance (transform.position, Player.main.transform.position) < 6f) {
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

	Vector3 RandomWaypoint() {
		var idx = Random.Range (0, waypoints.transform.childCount);
		return waypoints.transform.GetChild(idx).transform.position;
	}
}
