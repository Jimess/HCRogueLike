using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarBumperOnCollision : MonoBehaviour {

	void OnCollisionEnter2D (Collision2D col) {
		if (col.gameObject.tag == "Obstacle") {
			print("Bumper Hit!");
			col.gameObject.GetComponent<ObstacleOnCollision>().ObstactleDestroy();
		}
	}
}
