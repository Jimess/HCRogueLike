using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarBumperOnCollision : MonoBehaviour {

	void OnTriggerEnter2D (Collider2D other) {
		if (other.tag == "Obstacle") {
			print("Bumper Hit!");
			StartCoroutine(SlowMotionForSeconds(0.5f, 0.2f));
			other.GetComponent<ObstacleOnCollision>().ObstactleDestroy();
		}
	}

	IEnumerator SlowMotionForSeconds (float time, float scale) {
		float timeStamp = Time.time + time;
		Time.timeScale = scale;
		Time.fixedDeltaTime = 0.02f * Time.timeScale;
		while (timeStamp > Time.time) {
			yield return null;
		}
		Time.timeScale = 1f;
		Time.fixedDeltaTime = 0.02f * Time.timeScale;
	}
}
