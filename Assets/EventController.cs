using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EventController : MonoBehaviour {

	// Cusomization screen
	public GameObject customPanel;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ButtonStart () {
		SceneManager.LoadScene ("DriveScene");
	}

	public void EnableCusomization() {
		print("Enabling..");
		customPanel.SetActive(true);
		gameObject.SetActive(false);
	}

	public void DisableCustomization() {
		customPanel.SetActive(false);
		gameObject.SetActive(true);
		GameController.controller.Save();
	}
}
