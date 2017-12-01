using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	public float FireForce = 500f;
	public float DestroyTimeout = 3f;


	public GameObject bombExplosionPrefab;
	public AudioClip bombExplosionSound;

	//public GameObject sparksPrefab;

	private Rigidbody rigidbody_;
	private bool hit = false;

	// Use this for initialization
	void Awake () {
		rigidbody_ = GetComponent<Rigidbody> ();
	}

	void OnCollisionEnter(Collision col) {
		if (hit) {
			return;
		}
		hit = true;

		if (col.collider.tag == "Tree" || col.collider.tag == "BossSkull") {
			rigidbody_.detectCollisions = false;
			rigidbody_.isKinematic = true;
			rigidbody_.velocity = Vector3.zero;
			GetComponent<AudioSource> ().Play ();

		}
		if (col.collider.tag == "LittleEnemy" || col.collider.name == "BigEnemy") {
			col.gameObject.SendMessage ("OnHit");
			//col.collider.gameObject.SetActive (false);
			if (!Game.main.bombShot && !Game.main.bombShotExplode) {
				Game.main.comboHits++;
				// TODO: replace with pending combo hits
				if (Game.main.comboHits == 3) {
					Game.main.bombShot = true;
					Game.main.comboHits = 0;
				}
			}
			Destroy (gameObject);
		} else {
			Game.main.comboHits = 0;
		}
		if (Game.main.bombShot && Game.main.bombShotExplode) {
			Game.main.bombShot = false;
			Game.main.bombShotExplode = false;
			Instantiate (bombExplosionPrefab, transform.position, transform.rotation);
			GetComponent<AudioSource> ().clip = bombExplosionSound;
			GetComponent<AudioSource> ().pitch = 0.75f;
			GetComponent<AudioSource> ().volume = 1f;
			GetComponent<AudioSource> ().Play ();
			//Game.main.ShakeCamera ();
		}
//		if (col.gameObject.layer == LayerMask.NameToLayer("Enemies")) {
//			col.gameObject.SendMessage ("OnBulletHit");
//		} else {
//			Instantiate (sparksPrefab, transform.position, transform.rotation * Quaternion.Euler(0,180f,0));
//		}
//		if (hit) {
//			Destroy (gameObject);
//		}
	}

	public void Fire() {
		rigidbody_.AddForce (transform.forward * FireForce, ForceMode.Impulse);
		//Destroy (gameObject, DestroyTimeout);
	}
}
