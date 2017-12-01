using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

	public GameObject[] hide;

	private List<GameObject> hidden = new List<GameObject>();

	void OnEnable() {
		foreach (var obj in hide) {
			if (obj != null && obj.activeInHierarchy) {
				hidden.Add (obj);
				obj.SetActive (false);
			}
		}
		Time.timeScale = 0;
		Player.main.freeze = true;
		Cursor.lockState = CursorLockMode.None;
	}

	void OnDisable() {
		foreach (var obj in hidden) {
			obj.SetActive (true);
		}
		Time.timeScale = 1;
		Player.main.freeze = false;
		Cursor.lockState = CursorLockMode.Locked;
	}

	public void ExitToMenu() {
		Cursor.lockState = CursorLockMode.None;
		SceneManager.LoadScene (0);
	}
}
