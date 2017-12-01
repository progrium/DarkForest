using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour {

	public GameObject pauseScreen;
	public GameObject music;

	public GameObject bombSparks;
	public GameObject cameraHarness;

	public GameObject gameOverMessage;
	public GameObject gameWinMessage;

	public GameObject gameDelegate;

	public Gate gate;

	public ImageFader black;

	public bool bossMode = false;


	public int comboHits = 0;
	public bool bombShot = false;
	public bool bombShotExplode = false;


	static private Game game = null;
	static public Game main {
		get {
			if (game == null) {
				try {
					game = GameObject.Find("Game").GetComponent<Game>();
				} catch {
					game = null;
				}
			}
			return game;
		}
	}

	public void ShakeCamera() {
		cameraHarness.GetComponent<CameraShake> ().ShakeCamera (5f, 0.005f);
	}

	void Awake() {
		black = GameObject.Find ("Black").GetComponent<ImageFader> ();
	}

	void Start() {
		black.SetAlpha (1f);
		black.FadeOut (3f);
		Cursor.lockState = CursorLockMode.Locked;
	}

	void Update() {


		var input = ReInput.players.GetPlayer (0);

		if (Input.GetKeyDown (KeyCode.Escape) || input.GetButtonDown ("Reset")) {
			pauseScreen.SetActive (!pauseScreen.activeInHierarchy);
		}

		if (Input.GetKeyDown (KeyCode.Slash)) {
			music.SetActive (!music.activeInHierarchy);
		}

		if (Player.main.godMode && input.GetButtonDown ("RealmUp") && Player.main.health < 4) {
			Player.main.SendMessage ("OnHealth");
		}

		if (Player.main.godMode && input.GetButtonDown ("RealmDown") && Player.main.health > 1) {
			Player.main.SendMessage ("OnDamage");
		}

		if (bombShot && !bombSparks.activeInHierarchy && !bombShotExplode) {
			bombSparks.SetActive (true);
		}

	}

	IEnumerator win() {
		Player.main.freeze = true;
		black.FadeIn (2f);
		yield return new WaitForSeconds (2f);
		gameWinMessage.SetActive (true);
		var input = ReInput.players.GetPlayer (0);
		while (true) {
			if (input.GetButtonDown("Fire")) {
				break;
			}
			yield return null;
		}
		Cursor.lockState = CursorLockMode.None;
		SceneManager.LoadScene (0);
	}

	public void Win() {
		StartCoroutine (win ());
	}

	public void Die() {
		if (gameDelegate != null) {
			gameDelegate.SendMessage ("Die", SendMessageOptions.DontRequireReceiver);
			return;
		}
		StartCoroutine (die());
	}

	IEnumerator die() {
		Player.main.freeze = true;
		black.FadeIn (2f);
		yield return new WaitForSeconds (2f);
		gameOverMessage.SetActive (true);
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

}
