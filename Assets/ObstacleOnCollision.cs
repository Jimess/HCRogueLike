using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleOnCollision : MonoBehaviour {

	public GameObject explosion;

	// void OnCollisionEnter2D (Collision2D col) {
	// 	print("Colliding with: " + col.gameObject.tag);
	// 	if (col.gameObject.tag == "CarBumper") {
	// 		print("Bumper Hit!");
	// 	}
	// }

	//if a correct collider hits and this needs to be destroyed
	public void ObstactleDestroy() {
		Destroy(gameObject);
		Destroy (Instantiate (explosion, transform.position, transform.rotation), 4f);
		//instantiate particles
	}
}
