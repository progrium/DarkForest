using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySpaceBounder : MonoBehaviour {


	public BoxCollider playspace;

	void Awake() {
		if (playspace == null) {
			playspace = Game.main.GetComponent<BoxCollider> ();
		}
	}


	void LateUpdate () {
		var pos = transform.position;
		if (!playspace.bounds.Contains(pos)) {
			pos = playspace.bounds.ClosestPoint (pos).WithY (pos.y);
		}
		transform.position = pos;
	}
}
