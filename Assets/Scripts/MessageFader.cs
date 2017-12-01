using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageFader : MonoBehaviour {

	public Text text;
	public Image image;

	public float timeout = 2f;
	public float fadeDuration = 1f;


	private Coroutine co;

	void Awake() {
		if (image == null) {
			image = GetComponent<Image> ();
		}
	}

	void OnEnable() {
		ResetTimeout ();
		SetAlpha (0);
		FadeIn (fadeDuration);
	}

	void ResetTimeout() {
		if (co != null) {
			StopCoroutine (co);
		}
		co = StartCoroutine (activeTimeout ());
	}

	IEnumerator activeTimeout() {
		yield return new WaitForSeconds (timeout);
		//gameObject.SetActive (false);
		FadeOut(fadeDuration);
	}

	void SetAlpha(float alpha) {
		var color = image.color;
		if (alpha > 0) {
			color.a = alpha / 3f;
		} else {
			color.a = alpha;	
		}
		image.color = color;
		color = text.color;
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
		if (end == 0f) {
			gameObject.SetActive (false);
		}
	}
}
