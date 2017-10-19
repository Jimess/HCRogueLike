using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowCarScript : MonoBehaviour {

	public WheelJoint2D backWheel;
	public WheelJoint2D frontWheel;

	private JointSuspension2D backSuspension;
	private JointSuspension2D frontSuspension;


	void OnEnable() { // on enable place the car a bit up so it does not touch the ground
		// RectTransform.
		RectTransform rect = GetComponent<RectTransform>();
		rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, 220f);
	}

	// Use this for initialization
	void Start () {
		backSuspension = new JointSuspension2D() {angle = 90f};
		frontSuspension = new JointSuspension2D() {angle = 90f};
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ChangeBackSuspension (float freq, float damp) {
		backSuspension.dampingRatio = damp;
		backSuspension.frequency = freq;

		backWheel.suspension = backSuspension;
	}

	public void ChangeFrontSuspension (float freq, float damp) {
		frontSuspension.dampingRatio = damp;
		frontSuspension.frequency = freq;

		frontWheel.suspension = frontSuspension;
	}
}
