using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsWheelGrounded : MonoBehaviour {
	public bool isGrounded = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionStay2D (Collision2D col) {
		if (col.transform.tag == "Ground") {
			isGrounded = true;
		} else {
			isGrounded = false;
		}
	}

	void OnCollisionExit2D (Collision2D col) {
		isGrounded = false;
	}

	public bool GetGrounded () {
		return isGrounded; 
	}
}
