using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarInfo : MonoBehaviour {

	public GameObject car_bumper;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	//returns true if the bumper is existant
	public bool GetBumperState() {
		return car_bumper != null;
	}
}
