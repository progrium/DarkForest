using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Gate : MonoBehaviour {

	public GameObject collectMessage;
	public GameObject collectedMessage;
	public GameObject progressBar;
	public BigEnemy boss;

	public GameObject hudKeys;
	public Material hudKeyCollectedMaterial;

	public int requiredKeys = 4;
	public int collectedKeys = 0;

	public GameObject[] keyPickups;

	public bool activated = false;

	void OnTriggerEnter(Collider col) {
		if (col.tag != "Player") {
			return;
		}
		boss.SetTarget (transform.position);
		if (!activated) {
			hudKeys.SetActive (true);
			collectMessage.SetActive (true);
			foreach (var key in keyPickups) {
				key.SetActive (true);
			}
		} else {
			progressBar.SetActive (true);
		}
	}

	void OnTriggerExit(Collider col) {
		if (col.tag != "Player") {
			return;
		}
		if (!activated) {
			//message.FadeOut (1);
		} else {
			progressBar.SetActive (false);
		}
	}

	void Update() {
		if (!activated && collectedKeys == requiredKeys) {
			activated = true;
			collectedMessage.SetActive (true);
		}
	}

	public void UpdateHud() {
		for (var i = 0; i < 4; i++) {
			var key = hudKeys.transform.GetChild (i);
			if (i+1 <= collectedKeys) {
				var r = key.GetComponent<MeshRenderer> ();
				r.material = hudKeyCollectedMaterial;
			} 
		}
	}

	void OnComplete() {
		progressBar.SetActive (false);
		hudKeys.SetActive (false);
		Player.main.freeze = true;
		StartCoroutine (transition());
	}

	IEnumerator transition() {
		Game.main.black.FadeIn (1);
		yield return new WaitForSeconds (1f);
		SceneManager.LoadScene (2);
	}
}
