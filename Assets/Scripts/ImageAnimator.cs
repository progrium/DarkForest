using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageAnimator : MonoBehaviour {

	public Sprite[] frames;
	public int fps = 8;

	private Image image;

	void Awake () {
		image = GetComponent<Image> ();
	}

	void Start () {
		StartCoroutine (CycleAnimation ());
	}

	IEnumerator CycleAnimation() {
		var i = 0;
		while (true) {
			image.sprite = frames [i];
			i = (i+1) % frames.Length;
			yield return new WaitForSeconds(1f / fps);
		}
	}
}
