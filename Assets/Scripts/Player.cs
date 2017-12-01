using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {


	public float speed = 20f;

	public bool freeze = false;
	public int health = 100;

	public bool godMode = false;
	public bool inverted = false;

	public AudioSource audio;

	public GameObject cameraRig;

	public Camera[] realms;

	public GameObject hearts;

	public List<GameObject> nearbyEnemies = new List<GameObject> ();

	private float gravity;

	static private Player player = null;
	static public Player main {
		get {
			if (player == null) {
				player = Camera.main.transform.GetComponentInParent<Player> ();
			}
			return player;
		}
	}


	void Awake() {
		audio = GetComponent<AudioSource> ();
	}


	void OnEnemyEnter(GameObject enemy) {
		nearbyEnemies.Add (enemy);
	}

	void OnEnemyExit(GameObject enemy) {
		nearbyEnemies.Remove (enemy);
	}


	// Update is called once per frame
	void Update () {
		if (Time.timeSinceLevelLoad < 0.5f || freeze) {
			return;
		}
		var player = ReInput.players.GetPlayer (0);

		var controller = GetComponent<CharacterController>();
		var moveDeltaX = player.GetAxis ("MoveX") * speed * 2f;
		var moveDeltaZ = player.GetAxis ("MoveY") * speed * 2f;
		var turnDeltaY = player.GetAxis ("LookX") * speed * 15;
		var turnDeltaX = player.GetAxis ("LookY") * speed * 15;
		if (inverted) {
			turnDeltaX = turnDeltaX * -1;
		}
		gravity -= 50f * Time.deltaTime;

		controller.Move (transform.TransformDirection(new Vector3(moveDeltaX, gravity, moveDeltaZ)) * Time.deltaTime);
		transform.Rotate (0, turnDeltaY * Time.deltaTime, 0);
		cameraRig.transform.Rotate (turnDeltaX * Time.deltaTime, 0, 0);

		var rot = cameraRig.transform.localEulerAngles;
		if (rot.x < 320f && rot.x > 180f) {
			rot.x = 320f;
		}
		if (rot.x > 40f && rot.x < 180f) {
			rot.x = 40f;
		}
		rot.y = 0f;
		rot.z = 0f;
		cameraRig.transform.localEulerAngles = rot;


		if ( controller.isGrounded ) gravity = 0f;

		if (Input.GetKeyDown (KeyCode.Backslash)) {
			inverted = !inverted;
		}
	}

	void OnHealth() {
		if (health < 4 && health > 0) {
			realms [health].gameObject.SetActive (true);
			realms [health - 1].gameObject.SetActive (false);
			health++;
		}
		hearts.SetActive (true);
		hearts.SendMessage ("ResetTimeout");
		for (var i = 0; i < 4; i++) {
			var heart = hearts.transform.GetChild (i);
			heart.gameObject.SetActive (i < health);
		}
	}

	void OnDamage() {
		if (godMode && health == 1) {
			return;
		}
		health--;
		var input = ReInput.players.GetPlayer (0);
		input.SetVibration (0, 1f, 0.5f);
		if (health <= 0) {
			Game.main.Die ();
		} else {
			realms [health].gameObject.SetActive(false);
			realms [health - 1].gameObject.SetActive(true);
		}
		hearts.SetActive (true);
		hearts.SendMessage ("ResetTimeout");
		for (var i = 0; i < 4; i++) {
			var heart = hearts.transform.GetChild (i);
			heart.gameObject.SetActive (i < health);
		}
	}


}
