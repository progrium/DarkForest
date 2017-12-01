using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveTimeout : MonoBehaviour {

	public float timeout = 2f;

	private Coroutine co;

	void OnEnable() {
		ResetTimeout ();
	}

	void ResetTimeout() {
		if (co != null) {
			StopCoroutine (co);
		}
		co = StartCoroutine (activeTimeout ());
	}

	IEnumerator activeTimeout() {
		yield return new WaitForSeconds (timeout);
		gameObject.SetActive (false);
	}
}
