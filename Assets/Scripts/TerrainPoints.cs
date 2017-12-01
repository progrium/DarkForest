using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainPoints : MonoBehaviour {

	public Terrain terrain;
	public GameObject pointPrefab;
	public int resolution = 32;

	void Start() {
		var size = terrain.terrainData.size;
		for (float x = 0; x < size.x; x += size.x / resolution) {
			for (float y = 0; y < size.z; y += size.z / resolution) {
				var point = new Vector3 (x, 0, y);
				Instantiate (pointPrefab, point.WithY(terrain.SampleHeight(point)), Quaternion.identity, transform);
			}
		}
	}
}
