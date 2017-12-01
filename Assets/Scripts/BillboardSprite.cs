using UnityEngine;
using System.Collections;

public class BillboardSprite : MonoBehaviour {


	public bool rotateY = true;
	public bool rotateX = false;
	public bool flipY = true;

	void Update () {
		var position = Camera.main.transform.position;
		if (rotateY) {
			position.y = transform.position.y;
		}
		if (rotateX) {
			position.x = transform.position.x;
		}
		if (rotateX && rotateY) {
			transform.LookAt (position);
		} else {
			if (rotateX) {
				transform.LookAt (position, Vector3.forward);
			} else {
				transform.LookAt (position, Vector3.up);
			}
		}
		if (flipY) {
			transform.Rotate (Vector3.up, 180);
		}

	}
}
