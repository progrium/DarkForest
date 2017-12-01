using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour {

	public Terrain terrain;

	public GameObject[] treePrefabs;
	public GameObject enemyPrefab;

	public GameObject player;

	public int enemySpawns = 48;
	public int treeSpawns = 512;

	static private World world = null;
	static public World main {
		get {
			if (world == null) {
				world = GameObject.Find("World").GetComponent<World>();
			}
			return world;
		}
	}

	void Start () {
		var size = terrain.terrainData.bounds.size;

		var trees = new List<GameObject> ();
		for (var i = 0; i < treeSpawns; i++) {
			var x = Random.Range (0, size.x);
			var y = Random.Range (0, size.z);
			while (treeNotOk(x,y)) {
				x = Random.Range (0, size.x);
				y = Random.Range (0, size.z);
			}
			var r = Random.Range (0, 360);
			var ii = Random.Range(0, treePrefabs.Length);
			var pos = new Vector3 (x, 0, y);
			pos.y = terrain.SampleHeight (pos) -2f;
			var tree = Instantiate (treePrefabs[ii], pos, Quaternion.Euler(0, r, 0));
			trees.Add (tree);
		}
		for (var i = 0; i < enemySpawns; i++) {
			var x = Random.Range (0, size.x);
			var y = Random.Range (0, size.z);
			while (nontreeNotOk(x,y, trees) || treeNotOk(x,y)) {
				x = Random.Range (0, size.x);
				y = Random.Range (0, size.z);
			}
			Instantiate (enemyPrefab, new Vector3 (x, 0, y), Quaternion.identity);
		}
	}
	
	bool treeNotOk(float x, float y) {
		var v = new Vector3 (x, 0, y);
		var middle = Vector3.Distance (v, new Vector3 (256, 0, 256));
		var bottomLeft = Vector3.Distance (v, new Vector3 (64, 0, 64));
		var bottomRight = Vector3.Distance (v, new Vector3 (448, 0, 64));
		var topLeft = Vector3.Distance (v, new Vector3 (64, 0, 448));
		var topRight = Vector3.Distance (v, new Vector3 (448, 0, 448));
		float playerDistance;
		if (player != null) {
			playerDistance = Vector3.Distance (v, player.transform.position.WithY (0));
		} else {
			playerDistance = 1024; // no player, pretend distance is super high
		}
		return (middle < 70 || bottomLeft < 40 || bottomRight < 40 || topLeft < 40 || topRight < 40 || playerDistance < 10);
	}

	bool nontreeNotOk(float x, float y, List<GameObject> trees) {
		var v = new Vector3 (x, 0, y);
		var notOk = false;
		foreach (var tree in trees) {
			if (Vector3.Distance (v, tree.transform.position.WithY(0)) < 10f) {
				notOk = true;
				break;
			}
			if (player != null) {
				if (Vector3.Distance (v, player.transform.position.WithY(0)) < 120f) {
					notOk = true;
					break;
				}
			}
		}
		return notOk;
	}

}
