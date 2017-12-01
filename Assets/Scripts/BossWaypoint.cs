using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWaypoint : MonoBehaviour {

	public GameObject nextWaypoint;

	void OnTriggerEnter(Collider col) {
		if (col.tag != "BigEnemy") {
			return;
		}
		var enemy = col.gameObject.GetComponent<BigEnemy> ();
		enemy.SetNextTarget (nextWaypoint.transform.position);
		var boss = col.gameObject.GetComponent<Boss> ();
		if (boss != null) {
			boss.SetNextTarget (nextWaypoint.transform.position);
		}
	}
}
