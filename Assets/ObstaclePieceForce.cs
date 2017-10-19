using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclePieceForce : MonoBehaviour {

	private float colForce = 1f;
	// Use this for initialization
	void Start () {
		GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-1f, 1f) * colForce, Random.Range(0.1f, 1f) * colForce), ForceMode2D.Impulse);
	}

	void OnCollisionEnter2D (Collision2D collision) {
		if (collision.gameObject.tag == "Car") {
			// print("Ignoring collision with: " + collision.collider.tag);
			Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision.collider);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
