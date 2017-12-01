using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class Bow : MonoBehaviour {

	public GameObject arrow;
	public GameObject bulletPrefab;
	public Transform target;

	public float noiseRange = 32f;

	public AudioSource bowPull;
	public AudioSource bowFire;

	private bool fired = false;
	public float canFireNext;

	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {
		if (Time.timeSinceLevelLoad < 1f || Player.main.freeze) {
			return;
		}

		transform.LookAt (target.position.WithY(target.position.y+0.5f));

		var input = ReInput.players.GetPlayer (0);
		if (input.GetButtonDown ("Fire") && Time.time > canFireNext) {
			arrow.SetActive (true);
			bowPull.Play ();
			foreach (var enemy in Player.main.nearbyEnemies) {
				if (enemy.activeInHierarchy && Vector3.Distance(transform.position, enemy.transform.position) < noiseRange) {
					enemy.SendMessage ("OnNearbySound", Player.main.transform, SendMessageOptions.DontRequireReceiver);
				}
			}
			GetComponent<Animator> ().SetBool ("Draw", true);
		}
		if (input.GetButtonUp ("Fire")  & arrow.activeInHierarchy) {
			canFireNext = Time.time + 0.2f;
			GetComponent<Animator> ().SetBool ("Draw", false);
			bowFire.Play ();
			fired = true;
		}
	}

	void BowReset() {
		arrow.SetActive (false);
		if (fired) {
			var bullet = Instantiate (bulletPrefab, arrow.transform.position, transform.rotation);
			if (Game.main.bombShot) {
				bullet.gameObject.tag = "BombArrow";
				Game.main.bombShotExplode = true;
				Game.main.bombSparks.SetActive (false);
			}
			bullet.GetComponent<Bullet> ().Fire ();
			fired = false;
		}
	}
}
