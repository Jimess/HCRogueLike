using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarDriveEnd : MonoBehaviour {

	private LevelGenerator _gen;
	public GameObject endPosition;

	// Use this for initialization
	void Start () {
		_gen = GameObject.FindGameObjectWithTag ("LevelController").GetComponent<LevelGenerator>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D (Collider2D other) {
		if (other.tag == "Car") {
			print("Ending works");
			// send the car
			_gen.EndGameCarMove (other.gameObject);
		}
	}
}
