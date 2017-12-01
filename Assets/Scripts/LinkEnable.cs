using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkEnable : MonoBehaviour {

	public GameObject other;

	void OnEnable() {
		other.SetActive (true);
	}

	void OnDisable() {
		if (other != null)
			other.SetActive (false);
	}
}
