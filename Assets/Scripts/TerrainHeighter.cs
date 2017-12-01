using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainHeighter : MonoBehaviour {

	public float offset = 5f;
	public float minHeight = 0f;
	public bool lockHeight = false;

	void LateUpdate () {
		var pos = transform.position;
		minHeight = World.main.terrain.SampleHeight(transform.position) + offset;
		if (pos.y < minHeight || lockHeight)
			pos.y = minHeight;
		transform.position = pos;
	}
}
