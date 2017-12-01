using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextFader : MonoBehaviour {

	public Text text;

	// Use this for initialization
	void Awake () {
		if (text == null) {
			text = GetComponent<Text> ();
		}
	}

	public void SetAlpha(float alpha) {
		var color = text.color;
		color.a = alpha;
		text.color = color;
	}

	public void FadeIn(float time = 3f) {
		StartCoroutine (fade (0f, 1f, time));
	}

	public void FadeOut(float time = 3f) {
		StartCoroutine (fade (1f, 0f, time));
	}

	IEnumerator fade(float start, float end, float time = 3f) {
		var startTime = Time.time;
		var endTime = Time.time + time;
		while (Time.time < endTime) {
			var progress = Mathf.Floor ((Time.time - startTime) / (endTime - Time.time) * 30) / 30;
			SetAlpha (Mathf.Lerp (start, end, progress));
			yield return null;
		}
		SetAlpha (end);
	}
}
