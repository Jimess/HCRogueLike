using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleOnCollision : MonoBehaviour {

	public GameObject explosion;
	public GameObject obstaclePieces;

	void OnCollisionEnter2D (Collision2D col) {
		if (col.gameObject.tag == "Car") {
			if (col.gameObject.GetComponent<CarInfo>().GetBumperState()) {
				Destroy(gameObject);
				Instantiate (obstaclePieces, transform.position, transform.rotation);
				col.gameObject.GetComponentInChildren<CarBumperOnCollision>().OnHitSlowMotion();
				Destroy (Instantiate (explosion, transform.position, transform.rotation), 4f);
			}
		}
	}

	//if a correct collider hits and this needs to be destroyed
	public void ObstactleDestroy() {
		Destroy(gameObject);
		Instantiate (obstaclePieces, transform.position, transform.rotation);
		Destroy (Instantiate (explosion, transform.position, transform.rotation), 4f);
		//instantiate particles
	}
}
