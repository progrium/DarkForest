using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBar : MonoBehaviour {

	public RectTransform progress;
	public GameObject target;

	public float speed = 15f;

	private float maxValue;

	// Use this for initialization
	void Start () {
		maxValue = GetComponent<RectTransform> ().sizeDelta.x;
	}

	void OnEnable() {
		var size = progress.sizeDelta;
		size.x = 0;
		progress.sizeDelta = size;
	}
	
	// Update is called once per frame
	void Update () {
		var size = progress.sizeDelta;
		if (size.x < maxValue) {
			size.x += Time.deltaTime * speed;
			progress.sizeDelta = size;
		}
		if (size.x >= maxValue) {
			target.SendMessage ("OnComplete", SendMessageOptions.DontRequireReceiver);
		}
	}
}
