using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarMover : MonoBehaviour {
	public bool debugger = true;

	public float rotSpeed = 10f;
	public float maxSpeed = 2000f;

	private float currentMotorSpeed = 0f;

	public WheelJoint2D backwheel;
	public WheelJoint2D frontwheel;

	public IsWheelGrounded wheelScript;

	private float ver = 0f;
	private float hor = 0f;
	public Rigidbody2D rb;
	JointMotor2D motor;

	bool isInput = true;


	// Use this for initialization
	void Start () {
		motor = new JointMotor2D { maxMotorTorque = backwheel.motor.maxMotorTorque};
	}


	// Update is called once per frame
	void Update () {
		ver = -Input.GetAxisRaw ("Vertical") * maxSpeed;
		hor = Input.GetAxisRaw ("Horizontal");
		if (rb.velocity.x > 0) {
			currentMotorSpeed = rb.velocity.magnitude / 0.00288f;
		} else {
			currentMotorSpeed = -rb.velocity.magnitude / 0.00288f;
		}
	}

	void FixedUpdate () {

		if (isInput) {
			if (wheelScript.GetGrounded ()) { // if the back wheel is grounded, let it work
				if (ver < 0) {
					// print ("Is ON");
	//				motor.motorSpeed += ver;
					motor.motorSpeed = ver;
					backwheel.useMotor = true;
					backwheel.motor = motor;
				} else if (ver > 0) {
					// print ("BACK is ON");
					motor.motorSpeed = ver;
					backwheel.useMotor = true;
					backwheel.motor = motor;
				} else {
					// print ("Is OFF");
					backwheel.useMotor = false;
	//			backwheel.motor = motor;
				}
			} else { // if the back wheel is not grounded, not let the wheel spin
				backwheel.useMotor = false;
			}


			//updating the rotor speeds


			// car rotation
			rb.AddTorque (-hor * rotSpeed * Time.fixedDeltaTime);
		}

	}

	public void StopInput () {
		isInput = false;
	}

	public void StopMovement () {
		motor.motorSpeed = 0;
		backwheel.useMotor = true;
		backwheel.motor = motor;
	}

	void OnGUI () {
		if (debugger) {
			GUI.TextField (new Rect (10, 10, 200, 20), "MotorSpeed" + Mathf.Abs(motor.motorSpeed).ToString());
			GUI.TextField (new Rect (10, 30, 200, 20), "Velocity Vector: " + rb.velocity.ToString());
			GUI.TextField (new Rect (10, 50, 200, 20), "Velocity mag: " + rb.velocity.magnitude.ToString());
			GUI.TextField (new Rect (10, 70, 200, 20), "Vertical Input: " + ver.ToString ());
			GUI.TextField (new Rect (10, 90, 200, 20), "Motor State: " + backwheel.useMotor.ToString ());
			GUI.TextField (new Rect (10, 110, 200, 20), "Velocity to motorspeed: " + currentMotorSpeed);
//			GUI.TextField (new Rect (10, 130, 200, 20), "rotation: " + Mathf.Cos(rb.transform.eulerAngles.z) * Mathf.PI );
//			GUI.TextField (new Rect (10, 110, 200, 20), "Min: " + currentMinSpeed.ToString());
		}
	}
}
